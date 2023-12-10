using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class M_Enemy_01 : MonoBehaviour
{
    // いろいろ弄るのはここから ----------------------------------------------------------------------------------------------------
    // 停止状態の秒数
    [SerializeField] float stopSeconds;
    // 巡回状態の秒数
    //[SerializeField] float patrolSeconds;
    // 追尾状態になってから追尾を開始する秒数
    [SerializeField] float chaseStartSeconds;
    // 攻撃状態になってから攻撃判定発生までの秒数
    [SerializeField] float attackStartSeconds;
    // 攻撃判定発生してから終わるまでの秒数
    [SerializeField] float attackStopSeconds;

    // 巡回速度
    [SerializeField] float patrolSpeed;
    // 追尾速度
    [SerializeField] float chaseSpeed;
    //初期位置に戻る速度
    [SerializeField] float backSpeed;

    // 経路の中継点
    [SerializeField] Vector3[] wayPoints;
    // どのような順番で中継点を通るか
    [SerializeField] int[] patrolRoute;
    // 中継点間を移動する秒数
    [SerializeField] float[] patrolSeconds;
    // 中継点でどれだけ停止させるか
    [SerializeField] float patrolStopSeconds;

    // 索敵範囲
    [SerializeField] float searchRange;
    // 攻撃開始範囲
    [SerializeField] float attackRange;
    // いろいろ弄るのはここまで ----------------------------------------------------------------------------------------------------

    // 初期位置
    [SerializeField] Vector3 firstPos;// インスペクタで見たいのでSerializeField
    // 行動モード
    [SerializeField] string state = "行動モード";// インスペクタで見たいのでSerializeField
    // 状態が遷移してからの経過秒数（状態遷移カウント）
    [SerializeField] float stateTransitionCount;// インスペクタで見たいのでSerializeField
    // パトロールの経過秒数
    [SerializeField] float patrolCount;// インスペクタで見たいのでSerializeField
    // 目標地の座標
    [SerializeField] Vector3 targetPos;// インスペクタで見たいのでSerializeField
    // 目標中継点
    [SerializeField] int nextTargetNum;// インスペクタで見たいのでSerializeField

    // プレイヤーGameObject
    private GameObject Player;
    // プレイヤーの平面座標
    private Vector3 playerPos;
    // 自分のRigidbodyコンポーネント
    private Rigidbody RB;
    // 攻撃判定GameObject
    private GameObject AttackCollision;

    // 初回のみ処理
    void Start()
    {
        // 初期は停止状態
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
        //
        targetPos = wayPoints[1];
        // 目標中継点初期化
        nextTargetNum = 1;

        // プレイヤーオブジェクト取得
        Player = GameObject.FindGameObjectWithTag("Player");
        // 自分のRigidbodyコンポーネント取得
        RB = GetComponent<Rigidbody>();
        // 攻撃判定GameObject取得
        AttackCollision = transform.Find("AttackCollision").gameObject;
    }

    // フレーム毎処理
    void Update()
    {

        // プレイヤーの平面座標の取得
        playerPos.x = Player.transform.position.x;
        playerPos.y = transform.position.y;
        playerPos.z = Player.transform.position.z;

        // ターゲット位置の更新
        if (patrolCount >= patrolSeconds[LoopNumCal(nextTargetNum, -1, patrolSeconds.Length)])
        {
            patrolCount = 0.0f;
            nextTargetNum = LoopNumCal(nextTargetNum, +1, patrolSeconds.Length);
            targetPos = wayPoints[patrolRoute[nextTargetNum]];
        }
        
        // 状態によって条件分岐
        switch (state)
        {
            // 停止状態の処理 ----------------------------------------------------------------------------------------------------
            case "stop":

                // 速度が一定以上の場合、抵抗をかけて止めさせる
                if (RB.velocity.magnitude > 0.01f)
                {
                    RB.AddForce(-RB.velocity, ForceMode.Acceleration);
                }
                else
                {
                    RB.velocity = Vector3.zero;
                }

                // 自分から一定範囲内にプレイヤーが入ってきた場合、追尾状態に遷移
                if ((transform.position - playerPos).magnitude <= 15.0f)
                {
                    state = "chase";
                    stateTransitionCount = 0.0f;
                }

                // 状態遷移カウントが一定以上になった場合、巡回状態に遷移
                if (stateTransitionCount >= stopSeconds)
                {
                    state = "patrol";
                    stateTransitionCount = 0.0f;
                    //transform.LookAt(targetPos);
                }

                break;

            // 巡回状態の処理 ----------------------------------------------------------------------------------------------------
            case "patrol":

                // 目標中継地点から遠い間は目標中継地点に向かって進行
                if ((transform.position - wayPoints[patrolRoute[nextTargetNum]]).magnitude > 0.5f)
                {
                    transform.LookAt(new Vector3(targetPos.x, transform.position.y, targetPos.z));
                    RB.AddForce(transform.forward * 2.0f, ForceMode.Acceleration);
                }
                // 目標中継地点に近づいたら、目標中継地点を次の中継地点に更新し、停止状態に遷移
                else
                {
                    RB.AddForce(-RB.velocity, ForceMode.Acceleration);
                    if (RB.velocity.magnitude < 0.01f)
                    {
                        RB.velocity = Vector3.zero;
                    }
                }

                // 自分から一定範囲内にプレイヤーが入ってきた場合、追尾状態に遷移
                if ((transform.position - playerPos).magnitude <= searchRange)
                {
                    state = "chase";
                    stateTransitionCount = 0.0f;
                }

                break;

            // 追尾状態の処理 ----------------------------------------------------------------------------------------------------
            case "chase":

                // 追尾状態に遷移してから一定時間以内の場合、プレイヤーの方を向きながら減速し停止する
                if (stateTransitionCount <= chaseStartSeconds)
                {
                    //transform.LookAt(playerPos);
                    if (RB.velocity.magnitude > 0.01f)
                    {
                        RB.AddForce(-RB.velocity, ForceMode.Acceleration);
                    }
                    else
                    {
                        RB.velocity = Vector3.zero;
                    }
                }
                // 追尾状態に遷移してから一定時間経過した場合、プレイヤーの方を向き、追いかけてくる
                else
                {
                    transform.LookAt(playerPos);
                    RB.velocity = (playerPos - transform.position).normalized * chaseSpeed;
                }

                // 自分から一定範囲内にプレイヤーが入ってきた場合、攻撃状態に遷移
                if ((transform.position - playerPos).magnitude < attackRange)
                {
                    state = "attack";
                    stateTransitionCount = 0.0f;
                }
                // 自分の一定範囲内からプレイヤーが出た場合、停止状態に遷移
                else if ((transform.position - playerPos).magnitude > searchRange)
                {
                    state = "stop";
                    stateTransitionCount = 0.0f;
                }

                break;

            /*追尾状態後の処理 ----------------------------------------------------------------------------------------------------
            case "back to patrol":

                RB.velocity = Vector3.zero;
                transform.LookAt(firstPos);
                transform.position = Vector3.MoveTowards(transform.position, firstPos, backSpeed * Time.deltaTime);
                //初期位置に戻る
                if (transform.position == firstPos)
                {
                    state = "stop";
                    stateTransitionCount = 0.0f;
                }
                // 自分から一定範囲内にプレイヤーが入ってきた場合、追尾状態に遷移
                if ((transform.position - playerPos).magnitude <= searchRange)
                {
                    state = "chase";
                    stateTransitionCount = 0.0f;
                }

                break;*/

            // 攻撃状態の処理 ----------------------------------------------------------------------------------------------------
            case "attack":

                // 攻撃状態に遷移してから一定時間以内の場合、減速
                if (stateTransitionCount <= attackStartSeconds)
                {
                    if (RB.velocity.magnitude >= 0.01f)
                    {
                        RB.AddForce(-RB.velocity, ForceMode.Acceleration);
                    }
                    else
                    {
                        RB.velocity = Vector3.zero;
                    }
                }

                // 減速が終わってから一定時間経過した場合、攻撃判定ON
                else if (stateTransitionCount <= (attackStartSeconds + attackStopSeconds))
                {
                    AttackCollision.SetActive(true);
                }

                // 攻撃判定ONから一定時間経過した場合、攻撃判定OFFにし状態遷移
                else
                {
                    AttackCollision.SetActive(false);
                    // 自分から一定範囲内にプレイヤーがいる場合、追尾状態に遷移
                    if ((transform.position - playerPos).magnitude <= searchRange)
                    {
                        state = "chase";
                        stateTransitionCount = 0.0f;
                    }
                    // 自分から一定範囲内にプレイヤーがいない場合、停止状態に遷移
                    else
                    {
                        state = "stop";
                        stateTransitionCount = 0.0f;
                    }
                }

                break;

        }

        // 状態遷移カウントに経過時間を加算
        stateTransitionCount += Time.deltaTime;
        // パトロールの経過秒数に経過時間を加算
        patrolCount += Time.deltaTime;
    }




    /*
    private void OnCollisionEnter(Collision other)
    {
        //プレイヤーに触れた時
        if (other.gameObject.tag == "Player")
        {
            if (ta.ismove_Statue == true)
            {

                //春日追加
                state = "Dead";
                //state = "Damage";
                //ta.ismove_Statue = false;
            }
        }
    }*/


    // 0→1→2→・・・→length-2→length-1（最大値）→0→1→・・・とループするlength個の数のチェーンで
    // cur_numからadd_num進んだ数をとるメソッド
    //
    // 例 0→1→2→3→4→5→6→7→0→1→2→・・・というループ（つまりlength=8）で・・・
    //   5から4個進んだ数を知りたい → LoopNumCal(5, +4, 8) → 1が返る
    //   2から7個戻った数を知りたい → LoopNumCal(2, -7, 8) → 3が返る
    //
    // %演算で上手くいくと思ったが、マイナス値が上手くいかないのと記述が長くなるのでこのメソッドを作った
    // 主に経路移動のnextTargetNum用
    private int LoopNumCal(int cur_num, int add_num, int length)
    {
        cur_num += add_num;
        if (length <= cur_num)
        {
            cur_num = cur_num % length;
        }
        if (cur_num < 0)
        {
            cur_num = cur_num % length + length;
        }
        return cur_num;
    }

    // ギズモ描画
    void OnDrawGizmosSelected()
    {
        var guiStyle = new GUIStyle { fontSize = 20, normal = { textColor = Color.blue } };
        // 索敵範囲＆攻撃開始範囲描画
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // 中継点＆経路の描画
        Gizmos.color = Color.cyan;
        for (int i = 0; i < wayPoints.Length; i++)
        {
            if (UnityEditor.EditorApplication.isPlaying)
            {
                Gizmos.DrawWireSphere(wayPoints[i], 0.5f);
                Handles.Label(wayPoints[i] + new Vector3(0, 1, 0), i + "", guiStyle);
            }
            else
            {
                Gizmos.DrawWireSphere(transform.position + wayPoints[i], 0.5f);
                Handles.Label(transform.position + wayPoints[i] + new Vector3(0, 1, 0), i + "", guiStyle);
            }
        }
        for (int i = 0; i < patrolRoute.Length; i++)
        {
            if (UnityEditor.EditorApplication.isPlaying)
            {
                Gizmos.DrawLine(wayPoints[patrolRoute[i]], wayPoints[patrolRoute[LoopNumCal(i,1, patrolRoute.Length)]]);
            }
            else
            {
                Gizmos.DrawLine(transform.position + wayPoints[patrolRoute[i]], transform.position + wayPoints[patrolRoute[LoopNumCal(i, 1, patrolRoute.Length)]]);
            }
        }

        // 目標地の描画
        if (UnityEditor.EditorApplication.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(targetPos, 0.5f);
        }
    }


    /*

    // インスペクター拡張
    // 参考「https://qiita.com/sango/items/b705980ada56ba8ffa04」
#if UNITY_EDITOR
    [CustomEditor(typeof(M_Enemy_01))]// インスペクター拡張時のおまじない
    public class CharacterEditor : Editor// インスペクター拡張時のおまじない
    {

        public override void OnInspectorGUI()
        {
            // インスペクター拡張時のおまじない
            M_Enemy_01 thisScr = target as M_Enemy_01;




        }
    }
#endif

    */

}
