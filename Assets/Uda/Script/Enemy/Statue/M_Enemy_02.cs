using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_Enemy_02 : MonoBehaviour
{
    // いろいろ弄るのはここから ----------------------------------------------------------------------------------------------------
    // 停止状態の秒数
    [SerializeField] float stopSeconds;
    // 巡回状態の秒数
    [SerializeField] float patrolSeconds;
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

    // 索敵範囲
    [SerializeField] float searchRange;
    // 攻撃開始範囲
    [SerializeField] float attackRange;
    // いろいろ弄るのはここまで ----------------------------------------------------------------------------------------------------

    // 行動モード
    [SerializeField] string state = "行動モード";// インスペクタで見たいのでSerializeField
    // 状態が遷移してからの経過秒数（状態遷移カウント）
    [SerializeField] float stateTransitionCount;// インスペクタで見たいのでSerializeField

    // 初期ポジション
    private Vector3 firstPos;
    // ターゲットのポジション
    private Vector3 targetPos;
    private Vector3 targetPos_x;
    private Vector3 targetPos_z;

    //x軸上を周回する場合
    [SerializeField] bool Patrol_x;
    //y軸上を周回する場合
    [SerializeField] bool Patrol_z;
    //ランダムな方向に進む場合
    [SerializeField] bool Patrol_random;

    //x軸上を動かす場合の初期の向き
    [SerializeField] float Direction;
    private int Count;


    // プレイヤーGameObject
    GameObject Player;
    // プレイヤーの平面座標
    private Vector3 playerPos;
    // 自分のRigidbodyコンポーネント
    private Rigidbody RB;
    // 攻撃判定GameObject
    [SerializeField] GameObject AttackColiison;

    //HPバー関連
    [SerializeField]
    public GameObject HPUI;
    private Slider hpSlider;
    public bool Damage;
    public float DamageSpeed;

    //targetスクリプト取得
    private target ta;
    //プレイヤーのrigidbody取得
    private Rigidbody rb;

    //重力を弱めるタイミング
    public bool gravity_A;

    //突進後にプレイヤーを少し上に
    public float Fly;

    


    // 初回のみ処理
    void Start()
    {
        // 初期は停止状態
        state = "stop";
        stateTransitionCount = 0.0f;

        // 初期ポジションを設定
        firstPos = transform.position;

        //プレイヤーオブジェクト取得
        Player = GameObject.FindGameObjectWithTag("Player");

        //targetスクリプト取得
        ta = Player.GetComponent<target>();


        // 自分のRigidbodyコンポーネント取得
        RB = GetComponent<Rigidbody>();

        //HPバーの取得
        hpSlider = HPUI.GetComponent<Slider>();
        hpSlider.value = 1f;
        Damage = false;
        ta = Player.GetComponent<target>();
        rb = Player.GetComponent<Rigidbody>();
        gravity_A = false;

        //初期の向き指定
        Count = 0;
    }

    // フレーム毎処理
    void Update()
    {

        // プレイヤーの平面座標の取得
        playerPos.x = Player.transform.position.x;
        playerPos.y = Player.transform.position.y;
        playerPos.z = Player.transform.position.z;

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
                    // パトロール状態初期設定
                    if (Patrol_random == true)
                    {
                        targetPos = firstPos + new Vector3(Random.Range(-5.0f, 5.0f), 0.0f, Random.Range(-5.0f, 5.0f));
                        //patrolVec = (targetPos - transform.position).normalized;
                        transform.LookAt(targetPos);
                    }
                    //x軸上を動かす場合の初期の向き
                    if (Patrol_x == true && Count == 0)
                    {
                        targetPos_x = firstPos + new Vector3(-Direction, 0.0f, 0.0f);
                        transform.LookAt(targetPos_x);
                        Count++;
                    }
                    //z軸上を動かす場合の初期の向き
                    if (Patrol_z == true && Count == 0)
                    {
                        targetPos_z = firstPos + new Vector3(0.0f, 0.0f, -Direction);
                        transform.LookAt(targetPos_z);
                        Count++;
                    }
                    if (Patrol_x == true || Patrol_z == true)
                    {
                            transform.Rotate(new Vector3(0, 180, 0));
                    }
                }

                break;

            // 巡回状態の処理 ----------------------------------------------------------------------------------------------------
            case "patrol":

                // 速度が一定以下の場合、前方向に力を加える
                if (RB.velocity.magnitude <= patrolSpeed)
                {
                    RB.AddForce(transform.forward, ForceMode.Acceleration);
                }

                // 自分から一定範囲内にプレイヤーが入ってきた場合、追尾状態に遷移
                if ((transform.position - playerPos).magnitude <= searchRange)
                {
                    state = "chase";
                    stateTransitionCount = 0.0f;
                }

                // 状態遷移カウントが一定以上になった場合、停止状態に遷移
                if (stateTransitionCount >= patrolSeconds)
                {
                    state = "stop";
                    stateTransitionCount = 0.0f;
                   
                }

                break;

            // 追尾状態の処理 ----------------------------------------------------------------------------------------------------
            case "chase":

                // 追尾状態に遷移してから一定時間以内の場合、プレイヤーの方を向きながら減速し停止する
                if (stateTransitionCount <= chaseStartSeconds)
                {
                    transform.LookAt(playerPos);
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
                    state = "back to patrol";
                    stateTransitionCount = 0.0f;
                }

                break;

            //追尾状態後の処理
            case "back to patrol":

                RB.velocity = Vector3.zero;
                transform.LookAt(firstPos);
                transform.position = Vector3.MoveTowards(transform.position, firstPos, backSpeed * Time.deltaTime);
                //初期位置に戻る
                if (transform.position == firstPos)
                {
                    state = "stop";
                    stateTransitionCount = 0.0f;
                    Count = 0;
                }
                // 自分から一定範囲内にプレイヤーが入ってきた場合、追尾状態に遷移
                if ((transform.position - playerPos).magnitude <= searchRange)
                {
                    state = "chase";
                    stateTransitionCount = 0.0f;
                }

                break;

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
                    AttackColiison.SetActive(true);
                }

                // 攻撃判定ONから一定時間経過した場合、攻撃判定OFFにし状態遷移
                else
                {
                    AttackColiison.SetActive(false);
                    // 自分から一定範囲内にプレイヤーがいる場合、追尾状態に遷移
                    if ((transform.position - playerPos).magnitude <= searchRange)
                    {
                        state = "chase";
                        stateTransitionCount = 0.0f;
                    }
                    // 自分から一定範囲内にプレイヤーがいない場合、初期位置に移動
                    else
                    {
                        state = "back to patrol";
                        stateTransitionCount = 0.0f;
                    }
                }

                break;

            //死んだときの処理
            case "Dead":
                Destroy(this.gameObject);
                rb.constraints = RigidbodyConstraints.None;
                Player.transform.position = new Vector3(Player.transform.position.x,Player.transform.position.y + Fly, Player.transform.position.z);
                gravity_A = true;
                ta.ismove_Statue = false;
                ta.isTarget_Statue = false;
                //rb.isKinematic = false;

                break;

            //プレイヤーに当たった時の処理
            case "Damage":

                Damage = true;

                break;
        }

        // 状態遷移カウントに経過時間を加算
        stateTransitionCount += Time.deltaTime;

        //HP減少
        if (Damage == true)
        {
            hpSlider.value -= DamageSpeed * Time.deltaTime;
            if (hpSlider.value <= 0)
            {
                state = "Dead";
            }
        }
       
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (ta.ismove_Statue == true)
            {
                state = "Damage";
                ta.ismove_Statue = false;
                //rb.isKinematic = false;
            }
        }
    }

   
    // ギズモ描画
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
