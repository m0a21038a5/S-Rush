using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamEnemy : MonoBehaviour
{
    public BeamBody beamBodyEnemy;
    public float BeamSpeed;
    [SerializeField]
    private GameObject BeamPrefab;

    //春日追加
    private target ta;


    #region// 永嶋追加
    // 動く距離
    public float Move_Dist;
    // 上下、前後、左右どの方向に動くか
    public bool Patrol_UPDOWN;
    public bool Patrol_FRONTBACK;
    public bool Patrol_LEFTRIGHT;
    // 最初の動き
    public bool First;

    public bool isTrigger_Player = false;

    float Count;
    // 動く速度
    public float Move_Speed;
    #endregion




    public enum BeamEnemyStatus
    {
        WaitStop,//止まっている待機
        WaitWalk,//巡回している待機
        ChasePlayerWalk,//プレイヤーを追いかける
        Beam,//ビーム発射
        DamegePlayerAttack,//プレイヤーの攻撃を受ける
        KnockDown//ビームの敵が倒される
    }
    public BeamEnemyStatus beamEnemyStatus;
    void Start()
    {
        //春日追加
        GameObject Playerobj = GameObject.Find("Player");
        ta = Playerobj.GetComponent<target>();

        #region // 永嶋追加 
        Count = Move_Dist / (Move_Speed * Time.deltaTime * 2);
        #endregion

    }
    void Update()
    {
        switch (beamEnemyStatus)
        {
            case BeamEnemyStatus.WaitStop:
                //Debug.Log("待機中");
                EnemyWait();
                break;
            case BeamEnemyStatus.WaitWalk:
                //Debug.Log("巡回中");
                EnemyWalk();
                break;
            case BeamEnemyStatus.ChasePlayerWalk:
                //Debug.Log("プレイヤーを追いかける!");
                EnemyChase();
                break;
            case BeamEnemyStatus.Beam:
               // Debug.Log("ビーム発射!");
                EnemyBeam();
                break;
            case BeamEnemyStatus.DamegePlayerAttack:
                //Debug.Log("プレイヤーの攻撃を受けた!");
                EnemyDamage();
                break;
            case BeamEnemyStatus.KnockDown:
                //Debug.Log("ビームの敵が倒された!");
                EnemyKnockDown();
                break;
        }
    }
    void EnemyWait()
    {
        beamEnemyStatus = BeamEnemyStatus.ChasePlayerWalk;
    }
    void EnemyWalk()
    {
        //BeamBodyスクリプトを無効にする
        beamBodyEnemy.enabled = false;

        if (First == true)
        {
            if (Count * Move_Speed * Time.deltaTime < Move_Dist)
            {
                if (Patrol_UPDOWN == true)
                {
                    // 上昇する
                    transform.position += transform.up * Move_Speed * Time.deltaTime;
                }

                if (Patrol_FRONTBACK == true)
                {
                    // 前に進む
                    transform.position += transform.forward * Move_Speed * Time.deltaTime;
                }

                if (Patrol_LEFTRIGHT == true)
                {
                    // 右に進む
                    transform.position += transform.right * Move_Speed * Time.deltaTime;
                }
                Count++;
            }
            else First = false;
        }
        else
        {
            if (0 < Count * Move_Speed * Time.deltaTime)
            {
                if (Patrol_UPDOWN == true)
                {
                    // 下降する
                    transform.position -= transform.up * Move_Speed * Time.deltaTime;
                }

                if (Patrol_FRONTBACK == true)
                {
                    // 後ろに進む
                    transform.position -= transform.forward * Move_Speed * Time.deltaTime;
                }

                if (Patrol_LEFTRIGHT == true)
                {
                    // 左に進む
                    transform.position -= transform.right * Move_Speed * Time.deltaTime;
                }
                Count--;
            }
            else First = true;
        }
    }
    void EnemyChase()
    {
        //BeamBodyスクリプトを無効にする
        beamBodyEnemy.enabled = false;
        //とりあえずプレイヤーの方向を向く
        GameObject PlayerObject = GameObject.FindGameObjectWithTag("Player");
        transform.LookAt(PlayerObject.transform);
        
        // StartCoroutine(BeamCoroutine());
    }
    void EnemyBeam()
    {
        //とりあえずプレイヤーの方向を向く
        GameObject PlayerObject = GameObject.FindGameObjectWithTag("Player");
        transform.LookAt(PlayerObject.transform);
        //BeamBodyスクリプトを有効にする
        beamBodyEnemy.enabled=true;
    }
    void EnemyDamage()
    {

    }

    void EnemyKnockDown()
    {
        //プレイヤーに当たったら破壊する
        Destroy(this.gameObject);

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {//プレイヤーに当たったらKnockDownにする
            beamEnemyStatus = BeamEnemyStatus.KnockDown;

            //春日追加
            ta.isTarget_Beam = false;
            ta.ismove_Beam = false;
        }

    }
    IEnumerator BeamCoroutine()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        GameObject shell = Instantiate(BeamPrefab, transform.position, Quaternion.identity);
        Rigidbody shellRb = shell.GetComponent<Rigidbody>();
        shellRb.AddForce(transform.forward * BeamSpeed);
        Destroy(shell, 3.0f);
    }
}
