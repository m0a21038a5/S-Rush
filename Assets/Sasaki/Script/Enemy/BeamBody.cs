using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamBody : MonoBehaviour
{

    public float BeamSpeed = 10.0f;
    public int BemaInterval = 60;
    public float BeamDestroy;
    [SerializeField]
    private GameObject BeamPrefab;
    private int interval;


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


    void Start()
    {
        Count = Move_Dist / (Move_Speed * Time.deltaTime * 2);
    }

    // プレイヤーが索敵範囲内のときisTriggerをtrueにする
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTrigger_Player = true;
        }
    }

    // プレイヤーが索敵範囲内のときisTriggerをfalseにする
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTrigger_Player = false;
        }
    }


    void FixedUpdate()
    {

        if (isTrigger_Player == true)
        {
            if (First == true)
            {
                if (Count * Move_Speed * Time.deltaTime < Move_Dist)
                {
                    if (Patrol_UPDOWN == true)
                    {
                        transform.position += transform.up * Move_Speed * Time.deltaTime;
                    }

                    if (Patrol_FRONTBACK == true)
                    {
                        transform.position += transform.forward * Move_Speed * Time.deltaTime;
                    }

                    if (Patrol_LEFTRIGHT == true)
                    {
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
                        transform.position -= transform.up * Move_Speed * Time.deltaTime;
                    }

                    if (Patrol_FRONTBACK == true)
                    {
                        transform.position -= transform.forward * Move_Speed * Time.deltaTime;
                    }

                    if (Patrol_LEFTRIGHT == true)
                    {
                        transform.position -= transform.right * Move_Speed * Time.deltaTime;
                    }
                    Count--;
                }
                else First = true;
            }
        }



        interval += 1;//* Time.deltaTime

        if (interval % BemaInterval * Time.deltaTime == 0)
        {
            GameObject shell = Instantiate(BeamPrefab, transform.position, Quaternion.identity);
            Rigidbody shellRb = shell.GetComponent<Rigidbody>();
            shellRb.AddForce(transform.forward * BeamSpeed);
            Destroy(shell, BeamDestroy);
        }
    }

    /*
    IEnumerator BeamCoroutine()
    {
       
        GameObject shell = Instantiate(BeamPrefab, transform.position, Quaternion.identity);
        Rigidbody shellRb = shell.GetComponent<Rigidbody>();
        shellRb.AddForce(transform.forward * BeamSpeed);
        Destroy(shell, BemaInterval);
        yield return new WaitForSecondsRealtime(BemaInterval);

    }*/
}
