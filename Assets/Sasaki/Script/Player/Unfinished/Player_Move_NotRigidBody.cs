using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move_NotRigidBody : MonoBehaviour
{
    //Rigidbodyを使用しないで主人公を動かすスクリプトです。ジャンプが実装できてません
    private Vector3 moveX;
    private Vector3 moveZ;
    private Vector3 moveY;
    // MoveSpeed プレイヤーの速さ
    public float MoveSpeed;
    Vector3 target;
    Rigidbody rd;
    private Vector3 PlayerPos;
    private float JumpTimeCountUp;
    // jumpupSpeed ジャンプして上がった時のスピード
    //jumpdownSpeed ジャンプ落下のスピード
    public float jumpupSpeed;
    public float jumpdownSpeed;
    private bool isJumping = false;
    void Start()
    {
        target = transform.position;
        rd = GetComponent<Rigidbody>();
        rd.useGravity = false;
        rd.isKinematic = true;
        moveX = new Vector3(1.0f, 0.0f, 0.0f);
        moveZ  = new Vector3(0.0f, 0.0f, 1.0f);
        moveY = new Vector3(0.0f, 1.0f, 0.0f);
    }

    void Update()
    {
        TargetPosition();
        transform.position = Vector3.Lerp(transform.position, target, MoveSpeed * Time.deltaTime);
        //スペースキーを押して秒間はジャンプする
        JumpUp();
        JumpDown();
        /*
        //プレイヤーの移動方向に向き変更
        Vector3 diff = transform.position - PlayerPos;//プレイヤーがどの方向に進んでいるかがわかるように、初期位置と現在地の座標差分を取得
        PlayerPos = transform.position;
        if (diff.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(diff); //ベクトルの情報をQuaternion.LookRotationに引き渡し回転量を取得しプレイヤーを回転させる
        }*/
    }
    void TargetPosition()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            target = transform.position + moveX;
            return;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            target = transform.position - moveX;
            return;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            target = transform.position + moveZ;
            return;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            target = transform.position - moveZ;
            return;
        }
        if (Input.GetKey(KeyCode.Space) && isJumping == false)
        {
            target = transform.position + moveY;
            JumpTimeCountUp += Time.deltaTime;
            isJumping = true;
            if (JumpTimeCountUp > 3.0f)
            {
                target = transform.position - moveY;
                JumpTimeCountUp = 0;
            
                return;
            }
            return;
        }
    }
    void JumpUp()
    {
        transform.position = Vector3.Lerp(transform.position, target, jumpupSpeed * Time.deltaTime);
    }
    void JumpDown()
    {
        transform.position = Vector3.Lerp(transform.position, target, jumpdownSpeed * Time.deltaTime);
    }

}
