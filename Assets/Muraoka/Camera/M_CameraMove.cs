using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_CameraMove : MonoBehaviour
{

    public GameObject Player;
    public Rigidbody PlayerRB;
    public target PlayerTargrtScr;
    public Vector3 offset;

    public float goForwardSeconds;
    public bool isStop;
    public bool isMove;

    void Start()
    {
        Player = GameObject.Find("Player");
        PlayerRB = Player.GetComponent<Rigidbody>();
        PlayerTargrtScr = Player.GetComponent<target>();
        offset = transform.position - Player.transform.position;

        goForwardSeconds = 0.0f;
        isStop = false;
        isMove = false;
    }

    
    void Update()
    {
        if (PlayerRB.velocity.z > 0.1f && goForwardSeconds < 0.25f)
        {
            goForwardSeconds += Time.deltaTime;
        }
        else if(goForwardSeconds > 0.0f)
        {
            goForwardSeconds -= Time.deltaTime;
        }



        

        if (PlayerTargrtScr.ismove_Beam == true || PlayerTargrtScr.ismove_Statue == true)
        {
            isStop = true;
        }

        if (isStop == false && isMove == false)
        {
            //�ʏ펞
            transform.position = Player.transform.position + (offset * (1.0f + goForwardSeconds));
        }
        if(isStop == true)
        {
            if ((Player.transform.position - PlayerTargrtScr.StatuePos2).magnitude < 0.01f)
            {
                isStop = false;
                isMove = true;
            }

            if ((Player.transform.position - PlayerTargrtScr.BeamPos).magnitude < 0.01f)
            {
                isStop = false;
                isMove = true;
            }
        }
        if (isMove == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position + offset, 50.0f * Time.deltaTime);
            if ((transform.position - (Player.transform.position + offset)).magnitude < 0.01f)
            {
                isMove = false;
            }
        }

        

        //transform.position = Player.transform.position + (offset * (1.0f + goForwardSeconds));

    }
}
