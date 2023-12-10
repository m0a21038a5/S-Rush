using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainCamera : MonoBehaviour
{

    public string CameraType = "";

    [SerializeField] public GameObject TPCamera;
    private TPCamera TPC;

    [SerializeField] public GameObject TPCameraTarget;
    private TPCameraTarget TPCT;

    private GameObject Player;

    public bool isCameraResetNow;

    public bool isCameraFree = true;
    public Vector2 cameraMoveV = Vector2.zero;
    public GameObject pastEnemy;

    public float returnVecRange;

    private Combo combo;

    private target target;

    multipleTarget mt;

    public float reset_angle;

    void Start()
    {
        TPC = TPCamera.GetComponent<TPCamera>();
        TPCT = TPCameraTarget.GetComponent<TPCameraTarget>();

        Player = GameObject.FindGameObjectWithTag("Player");
        target = Player.GetComponent<target>();

        combo = Player.GetComponent<Combo>();

        mt = GameObject.FindGameObjectWithTag("Manager").GetComponent<multipleTarget>();
    }


    void LateUpdate()
    {
        // ターゲットが変わった時
        if (target.TargetStatue != null && pastEnemy != target.TargetStatue)
        {
            pastEnemy = target.TargetStatue;
            isCameraFree = true;
        }
        else if (target.TargetBeam != null && pastEnemy != target.TargetBeam)
        {
            pastEnemy = target.TargetBeam;
            isCameraFree = true;
        }
        else if (target.TargetBoss != null && pastEnemy != target.TargetBoss)
        {
            pastEnemy = target.TargetBoss;
            isCameraFree = true;
        }
        else if ((target.TargetStatue == null && target.TargetBeam == null && target.TargetBoss == null) && pastEnemy != null)
        {
            pastEnemy = null;
            isCameraFree = true;
        }







        if (Input.GetMouseButtonDown(2) || Input.GetKeyDown("joystick button 11"))
        {
            isCameraResetNow = true;
            reset_angle = Mathf.Atan2(-Player.transform.forward.x, Player.transform.forward.z) * Mathf.Rad2Deg;
        }

        cameraMoveV.x += Input.GetAxis("Mouse X");
        cameraMoveV.y += Input.GetAxis("Mouse Y");

        // ターゲット注目カメラからプレイヤー注目カメラへ遷移
        if (isCameraFree == false && (cameraMoveV.magnitude > returnVecRange) )
        {
            //pastObj = TPCT.targetObj;
            isCameraFree = true;
        }

        // プレイヤー注目カメラからターゲット注目カメラへ遷移（像）
        if (isCameraFree == true && target.TargetStatue != null && (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown("joystick button 6")))
        {
            cameraMoveV = Vector2.zero;
            isCameraFree = false;
        }

        // プレイヤー注目カメラからターゲット注目カメラへ遷移（ビーム）
        if (isCameraFree == true && target.TargetBeam != null && (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown("joystick button 6")))
        {
            cameraMoveV = Vector2.zero;
            isCameraFree = false;
        }

        // プレイヤー注目カメラからターゲット注目カメラへ遷移（ボス）
        if (isCameraFree == true && target.TargetBoss != null && (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown("joystick button 6")))
        {
            cameraMoveV = Vector2.zero;
            isCameraFree = false;
        }


        if (!mt.ChangeCamera)
        {

            // カメラリセット中
            if (isCameraResetNow == true)
            {
                TPC.resetCamera(reset_angle);
            }
            // カメラリセットをしていないとき
            else
            {
                // 何かをターゲットしていて、ターゲット注目カメラにしている時
                if (TPCT.isTarget == true && isCameraFree == false)
                {
                    TPC.targetEnemy();
                }
                else
                {
                    // W操作時（コンボ中＆空中）
                    if (combo.isCombo == true && combo.isFloor == false)
                    {
                        TPC.targetPlayer(true);
                    }
                    // 通常時
                    else
                    {
                        TPC.targetPlayer(false);
                    }

                }
            }
        }


    }

}
