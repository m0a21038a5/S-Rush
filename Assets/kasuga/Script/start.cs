using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start : MonoBehaviour
{
    // プレイヤーがどこでロープに触れたかを調べるスクリプト
    public bool startarea = false;

    private GameObject Ropemove;
    // Start is called before the first frame update
    void Start()
    {
        Ropemove = GameObject.Find("Ropebase");
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //プレイヤーが触れた位置を知る
            startarea = true;
            //ロープを動かすための変数
            Ropemove.GetComponent<Ropemove>().moveStart = true;
        }
    }
}
