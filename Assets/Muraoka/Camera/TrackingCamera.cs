using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// プレイヤーを追従するカメラのスクリプト
public class TrackingCamera : MonoBehaviour
{





















    /*
    // Mainカメラ
    private MainCamera MC;

    // このVirtualCameraのCVCコンポーネント
    private CinemachineVirtualCamera CVC;
    // このVirtualCameraのCVCコンポーネントのBodyの部分
    private CinemachineTransposer CT;
    // このVirtualCameraのCVCコンポーネントのAimの部分
    private CinemachineComposer CC;


    public Vector3 cameraStopPos;

    // 最初に１度だけ実行----------------------------------------------------------------
    void Start()
    {
        MC = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCamera>();

        // このVirtualCameraのCVCコンポーネントとBodyとAim部分取得
        CVC = GetComponent<CinemachineVirtualCamera>();
        CT = CVC.GetCinemachineComponent<CinemachineTransposer>();
        CC = CVC.GetCinemachineComponent<CinemachineComposer>();
    }

    // 毎フレーム実行----------------------------------------------------------------
    void Update()
    {

        if (MC.CameraType == "TR")
        {
            // マウスの移動距離分だけ回転角を動かす（縦方向には制限付きで）
            MC.rotate_h += -Input.GetAxis("Mouse X") * MC.rotate_h_sensi;
            MC.rotate_v += -Input.GetAxis("Mouse Y") * MC.rotate_v_sensi;
            MC.rotate_v = Mathf.Clamp(MC.rotate_v, MC.rotate_v_min, MC.rotate_v_max);

            CT.m_FollowOffset = MC.followOffset;
        }
        else
        {
            CT.m_FollowOffset = cameraStopPos;
        }
        
    }


    */


















    /*
    // プレイヤーのゲームオブジェクト
    private GameObject Player;

    // このVirtualCameraのCVCコンポーネント
    private CinemachineVirtualCamera CVC;
    // このVirtualCameraのCVCコンポーネントのBodyの部分
    private CinemachineTransposer CT;
    // このVirtualCameraのCVCコンポーネントのAimの部分
    private CinemachineComposer CC;

    // 通常時のプレイヤーとカメラの距離
    [SerializeField] float radius;
    // カメラの左右方向の回転角
    [SerializeField] float rotate_h;
    // カメラの上下方向の回転角
    [SerializeField] float rotate_v;

    // カメラの左右方向の回転の感度
    [SerializeField] float rotate_h_sensi;
    // カメラの上下方向の回転の感度
    [SerializeField] float rotate_v_sensi;
    // カメラの上下方向の回転の下限
    [SerializeField] float rotate_v_min;
    // カメラの上下方向の回転の上限
    [SerializeField] float rotate_v_max;

    // プレイヤーとカメラの最終的な距離ベクトル
    private Vector3 offset;
    // カメラの目標地点の高さオフセット
    private float aimOffsetY;

    // カメラリセット中フラグ
    public bool isCameraResetNow = false;
    // カメラを合わせる角度
    public float resetToAngle;

    [SerializeField] private float distance_ins;
    public static float distance;

    void Awake()
    {
        distance = distance_ins;
    }

    // 最初に１度だけ実行----------------------------------------------------------------
    void Start()
    {
        // プレイヤー取得
        Player = GameObject.FindGameObjectWithTag("Player");

        // このVirtualCameraのCVCコンポーネントとBodyとAim部分取得
        CVC = GetComponent<CinemachineVirtualCamera>();
        CT = CVC.GetCinemachineComponent<CinemachineTransposer>();
        CC = CVC.GetCinemachineComponent<CinemachineComposer>();

        radius = distance;
        rotate_h = transform.rotation.y;
        rotate_v = transform.rotation.x;
    }

    // 毎フレーム実行----------------------------------------------------------------
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // 前方向の rotate_h 取得
            resetToAngle = Mathf.Atan2(Player.transform.forward.z, Player.transform.forward.x) * Mathf.Rad2Deg;
            isCameraResetNow = true;
        }

        if (isCameraResetNow == true)
        {
            rotate_h = Mathf.MoveTowardsAngle(rotate_h, resetToAngle, 360.0f * Time.deltaTime * 2.0f);

            if (Mathf.Abs(rotate_h - resetToAngle) < 3.0f)
            {
                isCameraResetNow = false;
            }
        }
        else
        {
            // マウスの移動距離分だけ回転角を動かす（縦方向には制限付きで）
            rotate_h += -Input.GetAxis("Mouse X") * rotate_h_sensi;
            rotate_v += -Input.GetAxis("Mouse Y") * rotate_v_sensi;
            rotate_v = Mathf.Clamp(rotate_v, rotate_v_min, rotate_v_max);
        }


        if (rotate_h > 180.0f)
        {
            rotate_h -= 360.0f;
        }else if (rotate_h < -180.0f)
        {
            rotate_h += 360.0f;
        }

        // プレイヤーとカメラの距離ベクトルを計算
        offset.x = radius * Mathf.Sin(rotate_h / 180.0f * Mathf.PI) * Mathf.Cos(rotate_v / 180.0f * Mathf.PI);
        offset.y = radius * Mathf.Sin(rotate_v / 180.0f * Mathf.PI);
        offset.z = radius * -Mathf.Cos(rotate_h / 180.0f * Mathf.PI) * Mathf.Cos(rotate_v / 180.0f * Mathf.PI);

        // CVCコンポーネントのBodyのFollowOffsetにoffsetの値を入れる
        CT.m_FollowOffset = offset;

        if(0.0f >= rotate_v)
        {
            aimOffsetY = -rotate_v/16.0f;
        }
        else
        {
            aimOffsetY = 0.0f;
        }

        // CCコンポーネントのAimのTrackedObjectOffsetにaimOffsetYの値を入れる
        CC.m_TrackedObjectOffset.y = aimOffsetY;
    }*/
}