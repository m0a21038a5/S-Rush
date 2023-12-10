using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Rope_Action : MonoBehaviour
{
    //左シフトで親子関係の解消とキー入力で動けるようにする
    private GameObject Player;
    private GameObject catchRope;
    public Rigidbody rigidy;
    private GameObject Ropemove;
    private GameObject Playermove;
    // Start is called before the first frame update
    void Start()
    {
        Playermove = GameObject.Find("Player");
        Player = GameObject.Find("Player");
        catchRope = GameObject.Find("Ropebase");
        Ropemove = GameObject.Find("Ropebase");
        this.rigidy = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //親子関係の解消
            Player.gameObject.transform.parent = null;
            //FreezePositionのオンとオフを切り替える変数
            catchRope.GetComponent<catchRope>().freezePos = false;
            //ロープを動かすための変数
            Ropemove.GetComponent<Ropemove>().moveStart = false;
            Ropemove.GetComponent<Ropemove>().moveEnd = false;
            //重力が働くようにする
            this.rigidy.constraints = RigidbodyConstraints.None;
            //↑freezeRotationのチェックが外れるらしいからもう1度freezeRotationにチェックを入れる
            this.rigidy.freezeRotation = true;
            //キー入力でキャラを動かせるようにする
            Playermove.GetComponent<Player_Move>().enabled = true;
        }
    }


}
