using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move1 : MonoBehaviour
{
    private float inputHorizontal;
    private float inputVertical;
    private Rigidbody rb;

    public float moveSpeed = 3f;
    public float jump = 3f;
    public bool jumpnow=false;

    private target ta;
    public bool JumpTime;

    public float CountDown_01;
    private float CountDown_02;

    //重力の変更値
    public float gravity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ta = GetComponent<target>();
        JumpTime = false;
        CountDown_02 = CountDown_01;
    }

    void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        //スペースで移動できるようにする
        if (JumpTime == false)
        {
            Physics.gravity = new Vector3(0, -70, 0);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpnow = true;
                this.rb.constraints = RigidbodyConstraints.None;
                rb.freezeRotation = true;
            }
        }
        if(JumpTime == true)
        {
            CountDown_01 -= Time.deltaTime;
            Physics.gravity = new Vector3(0, gravity, 0);
        }
        if(CountDown_01 <= 0)
        {
            JumpTime = false;
            CountDown_01 = CountDown_02;
        }
    }

    void FixedUpdate()
    {
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;

        if (jumpnow == true)
        {
            rb.AddForce(transform.up * jump, ForceMode.Impulse);
           
        }
        jumpnow = false;
        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        rb.velocity = moveForward * moveSpeed + new Vector3(0, rb.velocity.y, 0);



        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Statue" || other.gameObject.tag == "Beam")
        {
            JumpTime = true;
        }
    }
}


