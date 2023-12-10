using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeamEnemy_ver2 : MonoBehaviour
{
    public BeamBody_ver2 beamBodyEnemy;
    public float BeamSpeed;
    [SerializeField]
    public GameObject BeamPrefab;
    private GameObject Player;

    //targetスクリプト取得
    private target ta;

    //重力を弱めるタイミング
    public bool gravity_B;

    //プレイヤーのrigidbody取得
    private Rigidbody rb;

    //突進後にプレイヤーを少し上に
    public float Fly;

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
        Player = GameObject.FindGameObjectWithTag("Player");
        ta = Player.GetComponent<target>();
        rb = Player.GetComponent<Rigidbody>();
        gravity_B = false;

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
        rb.constraints = RigidbodyConstraints.None;
        Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + Fly, Player.transform.position.z);
        gravity_B = true;
        //ta.ismove_Beam = false; 
        //rb.isKinematic = false;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {//プレイヤーに当たったらKnockDownにする
            //beamEnemyStatus = BeamEnemyStatus.KnockDown;
            beamEnemyStatus = BeamEnemyStatus.DamegePlayerAttack;
            //ta.ismove_Beam = false;
            //rb.isKinematic = false;
            Debug.Log("Hit");
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
