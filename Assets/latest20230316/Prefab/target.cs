using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


[System.Serializable]
public class Data
{
    public Vector3 position;

    // その他のデータを追加
}

public class target : MonoBehaviour
{
    //ターゲットしている近接敵
    public GameObject TargetStatue;
    //ターゲットしているビーム敵
    public GameObject TargetBeam;
    //ターゲットしているボス
    public GameObject TargetBoss;
    //突進速度
    public float RushSpeed = 0f;
    ////ターゲットしている近接敵の座標
    public Vector3 StatuePos2;
    //ターゲットしているビーム敵の座標
    public Vector3 BeamPos;
    //ターゲットしているボスの座標
    public Vector3 BossPos;
    public Rigidbody rb;
    public bool isTarget_Statue = false;
    public bool isTarget_Beam = false;
    public bool ismove_Statue = false;
    public bool ismove_Beam = false;
    public bool isTarget_Boss = false;
    public bool ismove_Boss = false;
    public bool clear = false;

    
    //当たった時の重力変更と当たった時のはね
    public float jump = 3f;
    public float JumpGravityY = -10;

    public Vector3 surfacePoint;

    private single si;

    public List<Data> dataList = new List<Data>();

    public Vector2 targetPosition;

    //攻撃時のフラグ
    public bool Attack;

    #region 突進による吹っ飛ばし
    public static target instance;
    public bool isAtacked = false;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    // PlayerSoundsスクリプト--------村岡追加--------
    private PlayerSounds ps;
    private Soundtest st;
    private bool isPlaySPChargeSound;

    [SerializeField]
    private float center;

    public bool jumpwait = false;

    public Vector3 nowPos;
    public float backKnockBackForce;
    public float upKnockBackForce;
    public float knockBackPower;

    multipleTarget mt;
    multiplePlayer mp;

    Combo c;

    public bool SpecialAttack;
    public bool SpecialAtStart;
    public Vector3 SpecialPosition;
    public Vector3 FinalSpecialPosition;

    public bool isMoving = false;

    [SerializeField] float slowMotionScale = 0.2f;
    public Vector3 FirstPosition;
    Vector3 SurfaceNormal;

    [SerializeField] GameObject SpecialEffect;
    float originalTimeScale = 1.0f;


    int enemyLayerMask;
    int BossLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //TargetImage.GetComponent<Image>().enabled = false;

        mt = GameObject.FindGameObjectWithTag("Manager").GetComponent<multipleTarget>();
        mp = GameObject.FindGameObjectWithTag("Player").GetComponent<multiplePlayer>();

        c = this.gameObject.GetComponent<Combo>();

        // PlayerSoundsスクリプト取得--------村岡追加--------
        ps = GetComponent<PlayerSounds>();
        st = GameObject.Find("SEPlayer").GetComponent<Soundtest>();// ここは一番下の行にしてください
        SpecialEffect.SetActive(false);
        originalTimeScale = 1.0f;
        enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        BossLayerMask = 1 << LayerMask.NameToLayer("Boss");
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetBeam != null)
        {
            isTarget_Beam = true;
            isTarget_Statue = false;
            isTarget_Boss = false;
        }
        else if (TargetStatue != null)
        {
            isTarget_Beam = false;
            isTarget_Statue = true;
            isTarget_Boss = false;
        }
        else if (TargetBoss != null)
        {
            isTarget_Beam = false;
            isTarget_Statue = false;
            isTarget_Boss = true;
        }

        if (TargetStatue == null && ismove_Statue)
        {
            ismove_Statue = false;
            isMoving = false;
            isAtacked = false;
            Attack = false;
        }
        else if (TargetBeam == null && ismove_Beam)
        {
            ismove_Beam = false;
            isMoving = false;
            isAtacked = false;
            Attack = false;
        }

        if (isTarget_Statue == true && !mt.multiple)
        {
            //敵まで移動
            if (ismove_Statue == true)
            {
                
                Vector3 Direction = StatuePos2 - this.transform.position;
                Ray ray = new Ray(this.transform.position, Direction);
                isMoving = true;
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, (enemyLayerMask | BossLayerMask)))
                {
                    surfacePoint = hit.point;
                    SurfaceNormal = hit.normal;
                    Debug.Log((surfacePoint - FirstPosition).magnitude);

                    if ((surfacePoint - FirstPosition).magnitude < 3.0f || (surfacePoint - Vector3.zero).magnitude < 3.0f) 
                    {
                        ismove_Statue = false;
                        isMoving = false;
                        rb.freezeRotation = false;
                        //攻撃しています
                        Attack = false;
                        // 突進による吹っ飛ばしのboolをtrueにする
                        isAtacked = false;
                        GetBack();
                    }
                    else
                    {
                        this.transform.position = Vector3.MoveTowards(this.transform.position, surfacePoint, RushSpeed * Time.deltaTime);
                        transform.LookAt(surfacePoint);
                        rb.freezeRotation = true;
                        //攻撃しています
                        Attack = true;
                        // 突進による吹っ飛ばしのboolをtrueにする
                        isAtacked = true;
                    }
                }
               
            }
                
            if (Input.GetMouseButtonDown(0))
            {
                ismove_Statue = true;
                // プレイヤーの突進音をON--------村岡追加--------
                ps.isPlayRushSound = true;
            }
                
            }

        
            if (isTarget_Beam == true)
            {
                BeamPos = new Vector3(TargetBeam.transform.position.x, TargetBeam.transform.position.y, TargetBeam.transform.position.z);
                

                //敵まで移動
                if (ismove_Beam == true)
                {
                    Vector3 Direction = BeamPos - this.transform.position;
                    Ray ray = new Ray(this.transform.position, Direction);
                    isMoving = true;
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, (enemyLayerMask | BossLayerMask)))
                    {
                        surfacePoint = hit.point;
                        SurfaceNormal = hit.normal;

                        if ((surfacePoint - FirstPosition).magnitude < 3.0f || (surfacePoint - Vector3.zero).magnitude < 3.0f)
                        {
                          ismove_Beam = false;
                          isMoving = false;
                          rb.freezeRotation = false;
                          //攻撃しています
                          Attack = false;
                          // 突進による吹っ飛ばしのboolをtrueにする
                          isAtacked = false;
                          GetBack();
                        }
                        else
                        {
                            this.transform.position = Vector3.MoveTowards(this.transform.position, surfacePoint, RushSpeed * Time.deltaTime);
                            transform.LookAt(surfacePoint);
                            rb.freezeRotation = true;
                            //攻撃しています
                            Attack = true;
                            // 突進による吹っ飛ばしのboolをtrueにする
                            isAtacked = true;
                        }
                    }
                

                }

                if (Input.GetMouseButtonDown(0))
                {
                    ismove_Beam = true;
                    // プレイヤーの突進音をON--------村岡追加--------
                    ps.isPlayRushSound = true;
                }
            }
            if (isTarget_Boss == true)
            {
            


                if (ismove_Boss == true)
                {
                    Vector3 Direction = BossPos - this.transform.position;
                    Ray ray = new Ray(this.transform.position, Direction);
                    isMoving = true;
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, (enemyLayerMask | BossLayerMask)))
                    {
                        surfacePoint = hit.point;
                        SurfaceNormal = hit.normal;
                        if ((surfacePoint - FirstPosition).magnitude < 3.0f || (surfacePoint - Vector3.zero).magnitude < 3.0f)
                        {
                           ismove_Boss = false;
                           isMoving = false;
                           rb.freezeRotation = false;
                           //攻撃しています
                           Attack = false;
                           // 突進による吹っ飛ばしのboolをtrueにする
                           isAtacked = false;
                            GetBack();
                        }
                        else
                        {
                           this.transform.position = Vector3.MoveTowards(this.transform.position, surfacePoint, RushSpeed * Time.deltaTime);
                           transform.LookAt(surfacePoint);
                           rb.freezeRotation = true;
                           //攻撃しています
                           Attack = true;
                           // 突進による吹っ飛ばしのboolをtrueにする
                           isAtacked = true;
                        }
                    }

                }

                if (Input.GetMouseButtonDown(0))
                {
                    ismove_Boss = true;
                    // プレイヤーの突進音をON--------村岡追加--------
                    ps.isPlayRushSound = true;
                }
            }
           

            if (c.Special == true && isPlaySPChargeSound == false)
            {
                st.SE_SPChargePlayer();
                isPlaySPChargeSound = true;
            }
            if (c.Special == false && isPlaySPChargeSound == true)
            {
                isPlaySPChargeSound = false;
            }
        }

        
    /// <summary>
    /// 突進後の跳ね返り
    /// </summary>
    /// <param name="collision"></param>
        public void KnockBack(Collision collision)
        {
            /*
            if (knockBackCount < 0.05f) return;
            knockBackCount = 0f;
            */
            nowPos = this.transform.position;
            var boundVec = (nowPos - collision.transform.position);
            boundVec.y = 0.0f;
            boundVec = boundVec.normalized * backKnockBackForce;
            boundVec.y = 1.0f * upKnockBackForce;
            boundVec = boundVec.normalized;
            rb.velocity = Vector3.zero;
            rb.AddForce(boundVec * knockBackPower, ForceMode.Impulse);
        }

        private void GetBack()
        {
           Vector3 First = new Vector3(FirstPosition.x, this.transform.position.y, FirstPosition.z);
           Vector3 movePoint = new Vector3(surfacePoint.x, this.transform.position.y, surfacePoint.z);
           float moveDistance = (movePoint - First).magnitude;
           float moveAmount = 5.0f;
           Vector3 moveDirection = (First - movePoint).normalized;
           this.transform.position = Vector3.Lerp(this.transform.position, this.transform.position + moveDirection * moveAmount,Time.deltaTime * RushSpeed);
        }

        private void OnCollisionEnter(Collision other)
        {
            if ((other.gameObject.tag == "Statue" || other.gameObject.tag == "Beam" || other.gameObject.tag == "BOSS") && Attack)
            {
                isTarget_Statue = false;
                ismove_Statue = false;
                isTarget_Beam = false;
                ismove_Beam = false;
           
           
                isMoving = false;
              
                if (!mt.multiple)
                {
                    KnockBack(other);
                    jumpwait = true;
                }


                // プレイヤーの攻撃をした時の音をON（アニメーションが出来次第移動）--------村岡追加--------
                ps.isPlayAttackSound = true;
                // プレイヤーの攻撃が当たった時の音をON--------村岡追加--------
                //ps.isPlayAttackHitSound = true;

                StatuePos2 = Vector3.zero;
                //Physics.gravity = new Vector3(0, JumpGravityY, 0);
            }
            else if ((other.gameObject.tag == "Statue" || other.gameObject.tag == "Beam") && SpecialAttack)
            {
                st.SE_SuperAttackPlayer();
            }
        }

    
    /// <summary>
    /// 防御力が高かった場合の突進後の跳ね返り
    /// </summary>
        public void DenfensiveKnockBack()
        {
            Vector3 incidentVector = (FirstPosition - surfacePoint).normalized;
            float KnockBackDistance = 15f;
            Vector3 moveDirection = transform.position + incidentVector * KnockBackDistance;
            rb.velocity = Vector3.zero;
            //rb.AddForce(moveDirection, ForceMode.Impulse);
            this.transform.position = Vector3.Lerp(this.transform.position,moveDirection, Time.deltaTime * RushSpeed);
        }
        IEnumerator ResetCollisionFlag()
        {
            yield return new WaitForSeconds(1f);
            jumpwait = false;
        }


        public void AddData(Data data)
        {
            dataList.Add(data);
        }

    /// <summary>
    /// 必殺技のコルーチン
    /// </summary>
    /// <param name="delayInSeconds"></param>
    /// <returns></returns>
        public IEnumerator DelayedStart(float delayInSeconds)
        {
            SpecialAtStart = true;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            Vector3 FirstPoint = transform.position;
            //float originalTimeScale = Time.timeScale;
            Time.timeScale = slowMotionScale;
            //SpecialEffect.transform.position = this.transform.position;
            SpecialEffect.SetActive(true);
            transform.LookAt(SpecialPosition);

            yield return new WaitForSecondsRealtime(delayInSeconds);

            // プレイヤーの突進音をON--------村岡追加--------
            ps.isPlayRushSound = true;
            Time.timeScale = originalTimeScale;
            SpecialEffect.SetActive(false);
            SpecialAttack = true;
            rb.isKinematic = false;
            // ここに待った後に実行したい処理を書きます
            if (!isMoving)
            {
                int layerMask = 1 << LayerMask.NameToLayer("Default"); // "YourLayerName"を対象のLayer名に置き換える
                int enemyLayerMask = 1 << LayerMask.NameToLayer("Boss");

                Ray ray = new Ray(transform.position, SpecialPosition - transform.position);
                RaycastHit hit;
                //RaycastHit[] hits;


                if (Physics.Raycast(ray, out hit, Vector3.Distance(transform.position, SpecialPosition), (layerMask | enemyLayerMask)))
                {
                    if ((layerMask & (1 << hit.collider.gameObject.layer)) != 0)
                    {
                        Vector3 normal = hit.normal;


                        // 法線ベクトルの向きによって壁か床かを判別する
                        if (Mathf.Abs(normal.y) > Mathf.Abs(normal.x) && Mathf.Abs(normal.y) > Mathf.Abs(normal.z))
                        {
                            // 法線ベクトルが上向き（床の法線ベクトル）の場合は床と判定
                            // 床に対する処理を行う
                            // 衝突地点を取得
                            Vector3 collisionPoint = hit.point;
                            Debug.Log("床");
                            FinalSpecialPosition = collisionPoint;
                            // 新しい位置にTweenアニメーションを設定（床に移動）
                            transform.DOMove(collisionPoint, 0.2f)
                                .SetEase(Ease.Linear)
                                .OnStart(() =>
                                {
                                    isMoving = true;
                                })
                                .OnComplete(() =>
                                {
                                // 床への移動が完了したら、元々移動していた方向に床と水平な角度で移動
                                Vector3 incidentVector = (SpecialPosition - FirstPoint).normalized;
                                    Vector3 floorNormal = hit.normal;
                                    Vector3 reflectionVector = Vector3.Reflect(incidentVector, floorNormal);
                                    Vector3 moveDirection = Vector3.Cross(Vector3.Cross(incidentVector, floorNormal), floorNormal).normalized;

                                // 移動距離を計算
                                float remainingDistance = 15f;

                                // 新しい位置を計算
                                //Vector3 newPosition = collisionPoint + moveDirection * remainingDistance;
                                Vector3 newPosition = collisionPoint + reflectionVector * remainingDistance;
                                    isMoving = false;
                                    SpecialAttack = false;
                                    SpecialAtStart = false;
                                    rb.useGravity = true;
                                // 新しい位置にTweenアニメーションを設定（床と水平な角度で移動）
                                rb.velocity = Vector3.zero;
                                    this.transform.position = Vector3.Lerp(this.transform.position, newPosition, Time.deltaTime * RushSpeed);
                                });
                        }
                        else
                        {
                            // 法線ベクトルが水平方向（壁の法線ベクトル）の場合は壁と判定
                            // 壁に対する処理を行う
                            // 壁の法線ベクトルを取得
                            Vector3 wallNormal = hit.normal;
                            Debug.Log("壁");
                            FinalSpecialPosition = hit.point;
                            // 新しい位置にTweenアニメーションを設定（壁に移動）
                            transform.DOMove(hit.point, 0.2f)
                                .SetEase(Ease.Linear)
                                .OnStart(() =>
                                {
                                    isMoving = true;
                                })
                                .OnComplete(() =>
                                {
                                // 壁への移動が完了したら、反射を実行
                                Vector3 incidentVector = (SpecialPosition - FirstPoint).normalized;
                                    float remainingDistance = 15f;
                                    Vector3 reflectionVector = Vector3.Reflect(incidentVector, wallNormal);

                                    Vector3 newPosition = hit.point + reflectionVector * remainingDistance;
                                    isMoving = false;
                                    SpecialAttack = false;
                                    rb.isKinematic = false;
                                    SpecialAtStart = false;
                                    rb.useGravity = true;
                                // 新しい位置にTweenアニメーションを設定（反射）
                                rb.velocity = Vector3.zero;
                                    this.transform.position = Vector3.Lerp(this.transform.position, newPosition, Time.deltaTime * RushSpeed);
                                });
                        }
                    }
                    else if (hit.collider.CompareTag("BOSS") || hit.collider.gameObject.name.Contains("Variant"))
                    {
                        FinalSpecialPosition = hit.collider.gameObject.GetComponent<IsRendered>().StatueRenderer.bounds.center;
                        transform.DOMove(hit.collider.gameObject.GetComponent<IsRendered>().StatueRenderer.bounds.center, 0.3f)
                                      .SetEase(Ease.Linear)
                                      .OnComplete(() =>
                                      {
                                          isMoving = false;
                                          SpecialAttack = false;
                                          SpecialAtStart = false;
                                          rb.useGravity = true;
                                          Vector3 incidentVector = (SpecialPosition - FirstPoint).normalized;
                                          float remainingDistance = 15f;
                                          Vector3 newPosition = hit.point + incidentVector * remainingDistance;
                                          this.transform.position = Vector3.Lerp(this.transform.position, newPosition, Time.deltaTime * RushSpeed);
                                      });
                    }
                }
                else
                {
                    FinalSpecialPosition = SpecialPosition;
                    // 衝突しない場合は通常のTweenアニメーションを実行
                    transform.DOMove(SpecialPosition, 0.3f)
                        .SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            isMoving = false;
                            SpecialAttack = false;
                            SpecialAtStart = false;
                            rb.useGravity = true;
                        });
                }
            }
        }

    }

