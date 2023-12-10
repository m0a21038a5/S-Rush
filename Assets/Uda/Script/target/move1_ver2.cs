using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move1_ver2 : MonoBehaviour
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

    // 音関係スクリプト--------村岡追加--------
    private PlayerSounds ps;
    private BGMPlayer bp;
    private bool isInDangerArea;

    [SerializeField] GameObject DangerArea1;
    private DangerArea da1;
    [SerializeField] GameObject DangerArea2;
    private DangerArea da2;
    [SerializeField] GameObject DangerArea3;
    private DangerArea da3;

    public bool isFloor = false;
    Infinityjump I;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ta = GetComponent<target>();
        JumpTime = false;
        CountDown_02 = CountDown_01;

        I = GameObject.FindGameObjectWithTag("Manager").GetComponent<Infinityjump>();

        // PlayerSoundsスクリプト取得--------村岡追加--------
        ps = GetComponent<PlayerSounds>();
        bp = GameObject.Find("BGMPlayer").GetComponent<BGMPlayer>();
        DangerArea1 = GameObject.FindGameObjectsWithTag("DangerZone")[0];
        DangerArea2 = GameObject.FindGameObjectsWithTag("DangerZone")[1];
        DangerArea3 = GameObject.FindGameObjectsWithTag("DangerZone")[2];
        da1 = DangerArea1.GetComponent<DangerArea>();
        da2 = DangerArea2.GetComponent<DangerArea>();
        da3 = DangerArea3.GetComponent<DangerArea>();
    }

    void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        //inputHorizontal = Input.GetAxisRaw("Horizontal_L");
        inputVertical = Input.GetAxisRaw("Vertical");
        //inputVertical = Input.GetAxisRaw("Vertical_L");
        //スペースで移動できるようにする
        if (JumpTime == false)
        {
            Physics.gravity = new Vector3(0, -70, 0);
            if (Input.GetKeyDown(KeyCode.Space)&&isFloor==true || Input.GetKeyDown("joystick button 1") && isFloor == true)
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
            if (CountDown_01 <= 0)
            {
                JumpTime = false;
                CountDown_01 = CountDown_02;
            }
        }
    }

    void FixedUpdate()
    {
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward;
       
        // 方向キーの入力値とカメラの向きから、移動方向を決定
        moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;
        
        

        if (jumpnow == true)
        {
            rb.AddForce(transform.up * jump, ForceMode.Impulse);
            
            // PlayerSoundsスクリプト取得--------村岡追加--------
            ps.isPlayJumpSound = true;
            if (!I.Infinity)
            {
                isFloor = false;
            }
        }
        jumpnow = false;

        if (!ta.isMoving || !ta.SpecialAtStart)
        {
            // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
            rb.velocity = moveForward * moveSpeed + new Vector3(0, rb.velocity.y, 0);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }



        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }


        // DangerAreaサウンド処理（物理関係との相性でFixedにしました）
        if (isInDangerArea == true && bp.aisac4 < 1.0f)
        {
            bp.aisac4 = Mathf.Clamp(bp.aisac4 + Time.deltaTime, 0.0f, 1.0f);
        }
        if (isInDangerArea == false && bp.aisac4 > 0.0f)
        {
            bp.aisac4 = Mathf.Clamp(bp.aisac4 - Time.deltaTime, 0.0f, 1.0f);
        }
        isInDangerArea = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Statue" || other.gameObject.tag == "Beam")
        {
            JumpTime = true;
        }
        if(other.gameObject.tag== "Floor")
        {
            isFloor = true;
        }
        
    }

    // DangerAreaサウンド処理
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == DangerArea1 && da1.isDestroy == false)
        {
            isInDangerArea = true;
        }
        if (other.gameObject == DangerArea2 && da2.isDestroy == false)
        {
            isInDangerArea = true;
        }
        if (other.gameObject == DangerArea3 && da3.isDestroy == false)
        {
            isInDangerArea = true;
        }
    }
}


