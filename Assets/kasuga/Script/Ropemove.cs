using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ropemove : MonoBehaviour
{
    public float speed = 0.05f;
    public Transform targetStart;
    public Transform targetEnd;
    public bool moveStart = false;
    public bool moveEnd = false;

    private GameObject endarea;
    private GameObject startarea;
    private GameObject Playermove;
    void Start()
    {
        
        startarea = GameObject.Find("start");
        endarea = GameObject.Find("end");
        Playermove = GameObject.Find("Player");

    }

    void Update()
    {
        if (moveStart == true&& startarea.GetComponent<start>().startarea == true && moveEnd == false)
        {
            //目的地まで移動させる
            transform.position = Vector3.MoveTowards(transform.position, targetEnd.position, speed);
            //ロープにつかまっている時にキー入力を無効にする(左シフト以外)
            Playermove.GetComponent<Player_Move>().enabled = false;

        }

        if (moveEnd == true && endarea.GetComponent<end>().endarea == true && moveStart == false)
        {
            //目的地まで移動させる
            transform.position = Vector3.MoveTowards(transform.position, targetStart.position, speed);
            //ロープにつかまっている時にキー入力を無効にする(左シフト以外)
            Playermove.GetComponent<Player_Move>().enabled = false;

        }
    }
}
