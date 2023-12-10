using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_Rigidboody : MonoBehaviour
{
    //ロープにつかまっている時に自然落下しないようにするためのスクリプト
    private GameObject catchRope;

    public Rigidbody rigidy;

    private void Start()
    {
        catchRope = GameObject.Find("Ropebase");
        this.rigidy = this.GetComponent<Rigidbody>();
    }
    void Update()
    {


    }

    private void OnCollisionEnter(Collision other)
    {
        if (catchRope.GetComponent<catchRope>().freezePos == true)
        {
            //ロープにつかまっている時に自然落下しないようにする
            this.rigidy.constraints = RigidbodyConstraints.FreezePosition;
            //↑freezeRotationのチェックが外れるらしいからもう1度freezeRotationにチェックを入れる
            this.rigidy.freezeRotation = true;
        }
    }
}
