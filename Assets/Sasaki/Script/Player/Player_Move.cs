using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    //Rigidbodyを使用して主人公を動かすスクリプトです
    //床にFloorタグを入れてください
    //moveSpeed 主人公の速さ
    public float moveSpeed;
    private Rigidbody rb;
    private float movementLRInputValue;
    private float movementFBInputValue;
   private float JumpTimeCountUp;
    // jumpupSpeed ジャンプして上がった時のスピード
    //jumpdownSpeed ジャンプ落下のスピード
    public float jumpupSpeed;
    public float jumpdownSpeed;
    private bool isJumping = false;
    public Vector3 colPosition;

    private Vector3 PlayerPos;
    private float xInput;
    private float zInput; 
    void Start()
    { 
      //  PlayerPos = GetComponent<Transform>().position;//最初の時点でのプレイヤーのポジションを取得
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
            //左右上下キーで移動
            movementLRInputValue = Input.GetAxis("Horizontal");
            movementFBInputValue = Input.GetAxis("Vertical");
        Vector3 movementLR = transform.right * movementLRInputValue * moveSpeed * Time.deltaTime;
        Vector3 movementFB = transform.forward * movementFBInputValue * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movementLR + movementFB);
       

        //スペースでジャンプ
        if (Input.GetKey(KeyCode.Space) && isJumping == false)
            {
                rb.velocity = Vector3.up * jumpupSpeed;
                JumpTimeCountUp += Time.deltaTime;
                isJumping = true;
                if (JumpTimeCountUp > 3.0f)
                {
                    rb.velocity = Vector3.up * -jumpdownSpeed;
                    JumpTimeCountUp = 0;
                }
            }
        /*
        //プレイヤーの移動方向に向き変更
        Vector3 diff = transform.position - PlayerPos;//プレイヤーがどの方向に進んでいるかがわかるように、初期位置と現在地の座標差分を取得
        //プレイヤーの前にcubeを置いて角度だけ追跡させるようにする？？？
        if (diff.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(diff); //ベクトルの情報をQuaternion.LookRotationに引き渡し回転量を取得しプレイヤーを回転させる
        }
        PlayerPos = transform.position;
        */
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJumping = false;
        }
    }
   
    
}
