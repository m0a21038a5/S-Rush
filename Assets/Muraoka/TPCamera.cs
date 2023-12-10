using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TPCamera : MonoBehaviour
{
    // Mainカメラ
    private MainCamera MC;
    // プレイヤーのゲームオブジェクト
    private GameObject Player;

    // このVirtualCameraのCVCコンポーネント
    private CinemachineVirtualCamera CVC;
    // このVirtualCameraのCVCコンポーネントのBodyの部分
    private CinemachineTransposer CT;
    // このVirtualCameraのCVCコンポーネントのAimの部分
    private CinemachineComposer CC;

    public Vector3 toEnemyVec;
    public float toEnemyAngle;

    private float velocity_v = 0;
    private float velocity_h = 0;

    // 通常時のプレイヤーとカメラの距離
    [SerializeField] float radius;
    // カメラの左右方向の回転角
    [SerializeField] float rotate_h;
    // カメラの上下方向の回転角
    [SerializeField] float rotate_v;
    // カメラの上下方向の回転の下限
    [SerializeField] float rotate_v_min;
    // カメラの上下方向の回転の上限
    [SerializeField] float rotate_v_max;

    private float[] HsensiTable = { 1.00f, 1.10f, 1.30f, 1.60f, 1.80f, 2.00f, 2.30f, 2.60f, 3.00f, 3.50f, 4.00f };
    private float[] VsensiTable = { 0.50f, 0.60f, 0.70f, 0.80f, 0.90f, 1.00f, 1.15f, 1.30f, 1.50f, 1.75f, 2.00f };
    // カメラの回転の感度
    static public int Stick_sensi = 5;

    public float resetToAngle;
    public float reset_v = 0;

    public float h_offset;
    static public bool isOperateY = false;

    void Start()
    {
        MC = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCamera>();
        // プレイヤー取得
        Player = GameObject.FindGameObjectWithTag("Player");

        // このVirtualCameraのCVCコンポーネントとBodyとAim部分取得
        CVC = GetComponent<CinemachineVirtualCamera>();
        CT = CVC.GetCinemachineComponent<CinemachineTransposer>();
        CC = CVC.GetCinemachineComponent<CinemachineComposer>();
        rotate_h = -91f;
        rotate_v = 2f;
    }

    public void targetEnemy()
    {
        toEnemyVec = (CVC.LookAt.position - Player.transform.position).normalized;
        toEnemyAngle = Mathf.Atan2(-toEnemyVec.x, toEnemyVec.z) * Mathf.Rad2Deg;

        rotate_h = Mathf.SmoothDampAngle(rotate_h, toEnemyAngle, ref velocity_h, 0.5f);
        rotate_v = Mathf.SmoothDampAngle(rotate_v, h_offset, ref velocity_v, 0.5f);

        CT.m_FollowOffset = calcOffset(radius, rotate_h, rotate_v);
    }

    public void targetPlayer(bool w_operation)
    {
        // W操作時
        if (w_operation == true)
        {
            // 少しでもキー入力があった場合そっちを優先
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f && isOperateY))
            {
                if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f)
                {
                    rotate_h += -Input.GetAxis("Horizontal") * HsensiTable[Stick_sensi];// 左スティック
                }
                if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f && isOperateY)
                {
                    rotate_v += -Input.GetAxis("Vertical") * VsensiTable[Stick_sensi];
                }
            }
            // キー入力が全くなかったらスティック入力
            else
            {
                rotate_h += -Input.GetAxis("Mouse X") * HsensiTable[Stick_sensi];
                rotate_v += -Input.GetAxis("Mouse Y") * VsensiTable[Stick_sensi];
            }
        }
        // 通常時
        else
        {
            rotate_h += -Input.GetAxis("Mouse X") * HsensiTable[Stick_sensi];
            rotate_v += -Input.GetAxis("Mouse Y") * VsensiTable[Stick_sensi];
        }

        // rotate_vを-180~180で正規化してからClamp
        rotate_v = Mathf.Clamp(Mathf.Repeat(rotate_v+180.0f,360.0f)-180.0f, rotate_v_min, rotate_v_max);
        CT.m_FollowOffset = calcOffset(radius, rotate_h, rotate_v);
    }


    public void resetCamera(float reset_angle)
    {
        rotate_h = Mathf.SmoothDampAngle(rotate_h, reset_angle, ref reset_v, 0.1f);
        CT.m_FollowOffset = calcOffset(radius, rotate_h, rotate_v);
        if (Mathf.Abs(reset_v) < 5.0f)
        {
            MC.isCameraResetNow = false;
        }
    }


    public Vector3 calcOffset(float radius, float rotate_h, float rotate_v)
    {
        Vector3 offset;

        offset.x = radius * Mathf.Sin(rotate_h / 180.0f * Mathf.PI) * Mathf.Cos(rotate_v / 180.0f * Mathf.PI);
        offset.y = radius * Mathf.Sin(rotate_v / 180.0f * Mathf.PI);
        offset.z = radius * -Mathf.Cos(rotate_h / 180.0f * Mathf.PI) * Mathf.Cos(rotate_v / 180.0f * Mathf.PI);

        return offset;
    }

    public void setAngle(float h, float v)
    {
        rotate_h = h;
        rotate_v = v;
    }
}
