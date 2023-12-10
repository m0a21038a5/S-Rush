using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StatueEnemyMove : MonoBehaviour
{
    // いろいろ弄るのはここから ----------------------------------------------------------------------------------------------------

    // 経路の中継点
    [SerializeField] public  Vector3[] wayPoints;
    // どのような順番で中継点を通るか
    [SerializeField] public int[] patrolRoute;
    // 中継点間を移動する秒数
    [SerializeField] public float[] patrolSeconds;

    // 巡回時速度
    [SerializeField]public float patrolSpeed;
    // 追尾時速度
    [SerializeField] public float chaseSpeed;
    // 回転速度
    [SerializeField]public float rotateSpeed;
    // 加速力
    [SerializeField]public float accelPower;
    // 減速力
    [SerializeField]public float breakPower;

    // 停止状態になってから止まっている秒数
    [SerializeField]public float stopSeconds;
    // 追尾状態になってから追尾を開始する秒数
    [SerializeField]public float chaseStartSeconds;
    // 攻撃判定発生してからプレイヤーを狙う秒数
    [SerializeField]public float attackAimSeconds;
    // 攻撃状態になってから攻撃判定発生までの秒数
    [SerializeField]public float attackStartSeconds;
    // 攻撃判定が発生してから終わるまでの秒数
    [SerializeField]public float attackStopSeconds;
    // パトロール中の目安停止時間
    [SerializeField]public float patrolStopSeconds;

    // 索敵範囲
    [SerializeField]public float searchRange;
    // 攻撃開始範囲
    [SerializeField]public float attackRange;
    // 中継点の半径
    [SerializeField]public float wayPointsRadius;

    // いろいろ弄るのはここまで ----------------------------------------------------------------------------------------------------

    // 初期位置
    [SerializeField]public Vector3 firstPos;
    // 行動モード
    [SerializeField] public string state = "行動モード";
    // 状態が遷移してからの経過秒数（状態遷移カウント）
    [SerializeField] public float stateTransitionCount;
    // 目標中継点
    [SerializeField] int nextTargetNum;
    // 目標地の座標
    [SerializeField] Vector3 targetPos;
    // パトロールの経過秒数
    [SerializeField] float patrolCount;
    // パトロールを停止するかどうか
    [SerializeField] bool stopPatrol;
    // 床の傾き
    [SerializeField] Vector3 floorNormal;

    // ギズモを常に表示するかどうか
    [SerializeField] bool isDrawPatrolRouteGizmos;
    [SerializeField] bool isDrawWayPointsNumberGizmos;
    [SerializeField] bool isDrawTargetGizmos;
    [SerializeField] bool isDrawStateGizmos;
    [SerializeField] bool isDrawRangeGizmos;
    [SerializeField] bool isDrawFloorNormalGizmos;

    // プレイヤーGameObject
    private GameObject Player;
    // プレイヤーの座標
    private Vector3 playerPos;
    // 自分のRigidbodyコンポーネント
    private Rigidbody RB;
    // 攻撃判定GameObject
    private GameObject AttackCollision;
    private GameObject enemyEffect;
    private GameObject EnemyEffect;

    // 初回のみ処理
    void Start()
    {
        // 初期は巡回状態
        state = "patrol";
        // 状態遷移カウント初期化
        stateTransitionCount = 0.0f;

        // 初期位置取得
        firstPos = transform.position;
        // 経路の中継点を相対座標から絶対座標に
        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] += firstPos;
        }
        // 目標中継点関係を1番目で初期化
        nextTargetNum = 1;
        targetPos = wayPoints[1];
        patrolCount = patrolSeconds[1];

        // パトロールフラグ初期化
        stopPatrol = false;
        // 床の法線ベクトル初期化
        floorNormal = new Vector3(0.0f, -1.0f, 0.0f);

        // プレイヤーオブジェクト取得
        Player = GameObject.FindGameObjectWithTag("Player");
        // 自分のRigidbodyコンポーネント取得
        RB = GetComponent<Rigidbody>();
        // 攻撃判定GameObject取得
        AttackCollision = transform.Find("AttackCollision").gameObject;
        EnemyEffect = transform.Find("da").gameObject;
        enemyEffect = transform.Find("dage").gameObject;
    }

    // 毎フレーム処理
    void Update()
    {
        // プレイヤーの座標の取得
        playerPos = Player.transform.position;


        // もし中継点の遷移までの経過秒数が過ぎてたらターゲットを次の中継点へ
        if (patrolCount < 0.0f)
        {
            nextTargetNum = (nextTargetNum + 1) % patrolRoute.Length;
            patrolCount = patrolSeconds[nextTargetNum] - patrolCount;
            targetPos = wayPoints[patrolRoute[nextTargetNum]];
            stopPatrol = false;
        }

        // 状態遷移カウントに経過時間を減算
        stateTransitionCount -= Time.deltaTime;
        // パトロールの経過秒数に経過時間を減算
        patrolCount -= Time.deltaTime;

        // 状態によって条件分岐（状態遷移など、物理演算が不必要な処理（物理演算等は下のFixedUpdate））
        switch (state)
        {
            // 停止状態の処理 ----------------------------------------------------------------------------------------------------
            case "stop":

                // 自分から一定範囲内にプレイヤーが入ってきた場合、追尾状態に遷移
                if ((transform.position - playerPos).magnitude <= 15.0f)
                {
                    state = "chase";
                    stateTransitionCount = chaseStartSeconds;
                }

                // 状態遷移カウントが一定以上になった場合、巡回状態に遷移
                if (stateTransitionCount < 0.0f)
                {
                    state = "patrol";
                    stateTransitionCount = 0.0f;
                }

                break;

            // 巡回状態の処理 ----------------------------------------------------------------------------------------------------
            case "patrol":

                // 自分から一定範囲内にプレイヤーが入ってきた場合、追尾状態に遷移
                if ((transform.position - playerPos).magnitude <= searchRange)
                {
                    state = "chase";
                    stateTransitionCount = chaseStartSeconds;
                }

                break;

            // 追尾状態の処理 ----------------------------------------------------------------------------------------------------
            case "chase":

                // 自分から一定範囲外にプレイヤーが出た場合、巡回状態に遷移
                if ((transform.position - playerPos).magnitude > searchRange)
                {
                    state = "stop";
                    stateTransitionCount = stopSeconds;
                }

                // 自分から一定範囲内にプレイヤーが入ってきた場合、攻撃状態に遷移
                if ((transform.position - playerPos).magnitude <= attackRange)
                {
                    state = "attack";
                    stateTransitionCount = attackStartSeconds + attackStopSeconds;
                }

                break;

            // 攻撃状態の処理 ----------------------------------------------------------------------------------------------------
            case "attack":

                // attackStartSeconds秒経過したら攻撃判定ON
                if (0.0f <= stateTransitionCount && stateTransitionCount < attackStopSeconds)
                {
                    AttackCollision.SetActive(true);
                    EnemyEffect.SetActive(true);
                    enemyEffect.SetActive(true);
                }
                // attackStartSeconds+attackStopSeconds秒経過したら（カウントダウンが0になったら）攻撃判定OFF＆状態遷移
                else if (stateTransitionCount < 0.0f)
                {
                    AttackCollision.SetActive(false);
                    EnemyEffect.SetActive(false);
                    enemyEffect.SetActive(false);

                    // 自分から一定範囲内にプレイヤーがいる場合、再び攻撃
                    if ((transform.position - playerPos).magnitude <= attackRange)
                    {
                        stateTransitionCount = attackStartSeconds + attackStopSeconds + attackAimSeconds;
                    }
                    // 自分から一定範囲内にプレイヤーがいる場合、追尾状態に遷移
                    else if ((transform.position - playerPos).magnitude <= searchRange)
                    {
                        state = "chase";
                        stateTransitionCount = chaseStartSeconds;
                    }
                    // 自分から一定範囲内にプレイヤーがいない場合、停止状態に遷移
                    else
                    {
                        state = "stop";
                        stateTransitionCount = stopSeconds;
                    }
                }

                break;

        }
    }

    // 物理関係の毎フレーム処理
    void FixedUpdate()
    {
        // ターゲットへのベクトル
        Vector3 ToTargetVec = wayPoints[patrolRoute[nextTargetNum]] - transform.position;
        // プレイヤーへのベクトル
        Vector3 ToPlayerVec = playerPos - transform.position;
        // ターゲットへの角度差を計算（patrolで使用）
        float AngleToTargrt = Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(ToTargetVec.x, ToTargetVec.z));
        // プレイヤーへの角度差を計算（chaseとAttackで使用）
        float AngleToPlayer = Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(ToPlayerVec.x, ToPlayerVec.z));

        // 床の傾きに合わせた重力補正をする
        RB.AddForce((Physics.gravity.magnitude * floorNormal) - Physics.gravity, ForceMode.Acceleration);

        // 状態によって条件分岐（移動など、物理演算が必要な処理（状態遷移等は上のUpdate））
        switch (state)
        {
            // 停止状態の処理 ----------------------------------------------------------------------------------------------------
            case "stop":

                // 速度が一定以上の場合、抵抗をかけて止めさせる
                RB.AddForce(-RB.velocity * breakPower, ForceMode.Acceleration);
                if (RB.velocity.magnitude < 0.01f)
                {
                    RB.velocity = Vector3.zero;
                }

                break;

            // 巡回状態の処理 ----------------------------------------------------------------------------------------------------
            case "patrol":

                // 角度がターゲット方面に合ってない場合は回転
                if ( Mathf.Abs(AngleToTargrt) > 1.0f)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0.0f, Mathf.Atan2(ToTargetVec.x, ToTargetVec.z) * Mathf.Rad2Deg, 0.0f), rotateSpeed);
                }
                // 角度がターゲット方面にだいたい合ってる場合は移動
                else
                {
                    // 目標中継地点から遠い間は目標中継地点に向かって進行
                    if (new Vector2(ToTargetVec.x, ToTargetVec.z).magnitude > wayPointsRadius)
                    {
                        if (new Vector2(RB.velocity.x, RB.velocity.z).magnitude < patrolSpeed)
                        {
                            RB.AddForce(ToTargetVec.normalized * accelPower, ForceMode.Acceleration);
                        }
                    }
                    // 目標中継地点に近づいたらパトロールを停止
                    else
                    {
                        stopPatrol = true;
                    }
                    // パトロール停止状態だったら減速させる
                    if (stopPatrol == true)
                    {
                        RB.AddForce(-RB.velocity * breakPower, ForceMode.Acceleration);
                        if (RB.velocity.magnitude < 0.01f)
                        {
                            RB.velocity = Vector3.zero;
                        }
                    }
                }

                break;

            // 追尾状態の処理 ----------------------------------------------------------------------------------------------------
            case "chase":

                // プレイヤーが索敵範囲に入ってから一定時間は停止してプレイヤーの方を向く
                if (stateTransitionCount >= 0.0f)
                {
                    // 角度がプレイヤー方面に合ってない場合はプレイヤー方面に回転
                    if (Mathf.Abs(AngleToPlayer) > 1.0f)
                    {
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0.0f, Mathf.Atan2(ToPlayerVec.x, ToPlayerVec.z) * Mathf.Rad2Deg, 0.0f), rotateSpeed);
                    }
                }
                // それが過ぎたらプレイヤーを追いかける
                else
                {
                    transform.LookAt(new Vector3(playerPos.x, transform.position.y, playerPos.z));
                    if (new Vector2(RB.velocity.x, RB.velocity.z).magnitude < chaseSpeed)
                    {
                        RB.AddForce(ToPlayerVec.normalized * accelPower, ForceMode.Acceleration);
                    }
                }

                break;

            // 攻撃状態の処理 ----------------------------------------------------------------------------------------------------
            case "attack":

                // 速度が一定以上の場合、抵抗をかけて止めさせる
                if (RB.velocity.magnitude >= 0.01f)
                {
                    RB.AddForce(-RB.velocity, ForceMode.Acceleration);
                }
                else
                {
                    RB.velocity = Vector3.zero;
                }
                // 再び攻撃になった時、attackAimSeconds秒間はプレイヤーを狙う
                if ( stateTransitionCount >= attackStartSeconds + attackStopSeconds)
                {
                    // 角度がプレイヤー方面に合ってない場合はプレイヤー方面に回転
                    if (Mathf.Abs(AngleToPlayer) > 1.0f)
                    {
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0.0f, Mathf.Atan2(ToPlayerVec.x, ToPlayerVec.z) * Mathf.Rad2Deg, 0.0f), rotateSpeed);
                    }
                }

                break;
        }
    }

    // 何かに触れている間の処理
    private void OnCollisionStay(Collision collision)
    {
        // とりあえず接触点の数だけ回す（接触点は配列でしか取得できなかったので）
        foreach (ContactPoint contact in collision.contacts)
        {
            // もし床と触れていたら、床の法線ベクトルを取得
            if (collision.transform.tag == "Floor")
            {
                floorNormal = - contact.normal;
            }
        }
    }

    // ここからギズモ描画＆インスペクター拡張関連（挙動に関係なし） ----------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
    // ギズモ描画（常に）
    void OnDrawGizmos()
    {
        // エディタ実行中
        if (UnityEditor.EditorApplication.isPlaying)
        {
            if (isDrawPatrolRouteGizmos == true)
            {
                DrawPatrolRouteGizmos(Vector3.zero);
            }
            if (isDrawWayPointsNumberGizmos == true)
            {
                DrawWayPointsNumberGizmos(Vector3.zero);
            }
            if (isDrawTargetGizmos == true)
            {
                DrawTargetGizmos();
            }
            if (isDrawStateGizmos == true)
            {
                DrawStateGizmos();
            }
        }
        // エディタ実行中でないとき
        else
        {
            if (isDrawPatrolRouteGizmos == true)
            {
                DrawPatrolRouteGizmos(transform.position);
            }
            if (isDrawWayPointsNumberGizmos == true)
            {
                DrawWayPointsNumberGizmos(transform.position);
            }
        }
        // いつでも
        if (isDrawRangeGizmos == true)
        {
            DrawRangeGizmos();
        }
        if (isDrawFloorNormalGizmos == true)
        {
            DrawFloorNormalGizmos();
        }
    }
    // ギズモ描画（選択中）
    void OnDrawGizmosSelected()
    {
        // エディタ実行中
        if (UnityEditor.EditorApplication.isPlaying)
        {
            if (isDrawPatrolRouteGizmos == false)
            {
                DrawPatrolRouteGizmos(Vector3.zero);
            }
            if (isDrawWayPointsNumberGizmos == false)
            {
                DrawWayPointsNumberGizmos(Vector3.zero);
            }
            if (isDrawTargetGizmos == false)
            {
                DrawTargetGizmos();
            }
            if (isDrawStateGizmos == false)
            {
                DrawStateGizmos();
            }
        }
        // エディタ実行中でないとき
        else
        {
            if (isDrawPatrolRouteGizmos == false)
            {
                DrawPatrolRouteGizmos(transform.position);
            }
            if (isDrawWayPointsNumberGizmos == false)
            {
                DrawWayPointsNumberGizmos(transform.position);
            }
        }
        // いつでも
        if (isDrawRangeGizmos == false)
        {
            DrawRangeGizmos();
        }
        if (isDrawFloorNormalGizmos == false)
        {
            DrawFloorNormalGizmos();
        }
    }

    // 中継点&経路描画メソッド
    void DrawPatrolRouteGizmos(Vector3 offset)
    {
        Gizmos.color = Color.cyan;
        for (int i = 0; i < wayPoints.Length; i++)
        {
            Gizmos.DrawWireSphere(offset + wayPoints[i], wayPointsRadius);
        }
        for (int i = 0; i < patrolRoute.Length; i++)
        {
            Gizmos.DrawLine(offset + wayPoints[patrolRoute[i]], offset + wayPoints[patrolRoute[(i + 1) % patrolRoute.Length]]);
        }
    }

    // 中継点番号描画メソッド
    void DrawWayPointsNumberGizmos(Vector3 offset)
    {
        var guiStyle = new GUIStyle { fontSize = 20, normal = { textColor = Color.blue } };
        for (int i = 0; i < wayPoints.Length; i++)
        {
            Handles.Label(offset + wayPoints[i] + new Vector3(0.0f, 1.0f, 0.0f), i + "", guiStyle);
        }
    }

    // 目標点描画メソッド
    void DrawTargetGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(targetPos, 0.5f);
    }

    // 状態描画メソッド
    void DrawStateGizmos()
    {
        var guiStyle = new GUIStyle { fontSize = 20, normal = { textColor = Color.blue } };
        Handles.Label(transform.position + new Vector3(0.0f, -1.0f, 0.0f), state, guiStyle);
    }

    // 索敵範囲＆攻撃開始範囲描画メソッド
    void DrawRangeGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // 法線ベクトル描画メソッド
    void DrawFloorNormalGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position - (floorNormal * 10.0f));
    }

    // インスペクター拡張
    [CustomEditor(typeof(StatueEnemyMove))]// おまじない
    public class StatueEnemyMoveEditor : Editor// Editorを継承（よくわからん）
    {
        // 折り畳みのフラグ
        bool individualFolding = true;
        bool basicFolding = false;
        bool debugFolding = false;

        // GUI描画メソッド
        public override void OnInspectorGUI()
        {
            StatueEnemyMove thisScript = target as StatueEnemyMove;
            // シリアライズ開始（よくわからんけどCustomEditorに必要）
            serializedObject.Update();

            // 個別設定描画
            if (individualFolding = EditorGUILayout.Foldout(individualFolding, "個別設定"))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("wayPoints"), new GUIContent("中継点の座標"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("patrolRoute"), new GUIContent("中継点を通る順番"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("patrolSeconds"), new GUIContent("中継点を通る秒数"));
                thisScript.isWarning();
                if (GUILayout.Button("秒数の自動設定"))
                {
                    Undo.RecordObject(thisScript, "秒数の自動設定");
                    thisScript.AutoSecondsSetting();
                    EditorUtility.SetDirty(target);
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space();

            // 基本設定描画
            if (basicFolding = EditorGUILayout.Foldout(basicFolding, "共通設定"))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("patrolSpeed"), new GUIContent("巡回中のスピード"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("chaseSpeed"), new GUIContent("追尾中のスピード"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rotateSpeed"), new GUIContent("回転するスピード"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("accelPower"), new GUIContent("加速時のパワー"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("breakPower"), new GUIContent("減速時のパワー"));
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("stopSeconds"), new GUIContent("停止状態の秒数"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("chaseStartSeconds"), new GUIContent("追尾状態の追尾開始秒数"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("attackAimSeconds"), new GUIContent("攻撃状態の狙う秒数"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("attackStartSeconds"), new GUIContent("攻撃開始までの秒数"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("attackStopSeconds"), new GUIContent("攻撃に掛かる秒数"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("patrolStopSeconds"), new GUIContent("巡回中の休憩目安秒数"));
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("searchRange"), new GUIContent("索敵範囲"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("attackRange"), new GUIContent("攻撃開始範囲"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("wayPointsRadius"), new GUIContent("中継点の半径"));
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space();

            // デバッグ情報描画
            if (debugFolding = EditorGUILayout.Foldout(debugFolding, "デバック情報"))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("firstPos"), new GUIContent("初期位置"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("state"), new GUIContent("行動モード"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("stateTransitionCount"), new GUIContent("状態遷移カウント"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("nextTargetNum"), new GUIContent("目標中継地点"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("targetPos"), new GUIContent("目標中継地点の座標"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("patrolCount"), new GUIContent("パトロール経過カウント"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("floorNormal"), new GUIContent("床の法線ベクトル"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("stopPatrol"), new GUIContent("パトロール停止フラグ"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isDrawPatrolRouteGizmos"), new GUIContent("常時経路ギズモ表示"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isDrawWayPointsNumberGizmos"), new GUIContent("常時番号ギズモ表示"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isDrawTargetGizmos"), new GUIContent("常時目標点ギズモ表示"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isDrawStateGizmos"), new GUIContent("常時状態ギズモ表示"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isDrawRangeGizmos"), new GUIContent("常時範囲ギズモ表示"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isDrawFloorNormalGizmos"), new GUIContent("常時床法線ギズモ表示"));
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space();

            // シリアライズ終了（よくわからんけどCustomEditorに必要）
            serializedObject.ApplyModifiedProperties();
        }
    }

    // 自動秒数設定メソッド
    public void AutoSecondsSetting()
    {
        patrolSeconds = new float[patrolRoute.Length];
        patrolSeconds[0] = (wayPoints[patrolRoute[patrolRoute.Length-1]] - wayPoints[patrolRoute[0]]).magnitude / patrolSpeed + patrolStopSeconds;
        for (int i=1; i< patrolSeconds.Length; i++)
        {
            patrolSeconds[i] = (wayPoints[patrolRoute[i]] - wayPoints[patrolRoute[i-1]]).magnitude / patrolSpeed + patrolStopSeconds;
        }
    }

    // 警告表示メソッド
    public void isWarning()
    {
        for (int i=0; i<patrolRoute.Length; i++)
        {
            if (patrolRoute[i] <= -1 || wayPoints.Length <= patrolRoute[i]  )
            {
                EditorGUILayout.HelpBox($"「中継点を通る順番」で存在しない中継点を使おうとしています\nこの場合 0～{wayPoints.Length-1} の範囲で指定するか中継点を増やしてね", MessageType.Warning, true);
            }
        }
        if (patrolRoute.Length != patrolSeconds.Length)
        {
            EditorGUILayout.HelpBox("「中継点を通る順番」と「中継点を通る秒数」は同じ個数にしてください\nとりあえず下の「秒数の自動設定」ボタンを押すと解決できるかも", MessageType.Warning, true);
        }
    }
#endif

}
