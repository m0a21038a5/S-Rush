using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class TargetController : MonoBehaviour
{
    public List<GameObject> targetCount;
    multipleTarget mt;
    target t;
    private RectTransform TargetImagePos;

    [SerializeField]
    GameObject SingletargetUI;
    RectTransform SingleTargetUIPos;

    [SerializeField]
    GameObject SpecialtargetUI;
    RectTransform SpecialTargetUIPos;

    GameObject Player;

    float width;
    float height;
    public float AimUI_Max_Size;
    public float AimUI_Min_Size;
    float Statue_tilt;
    float Beam_tilt;
    float Boss_tilt;
    public float Max_dis_Statue;
    public float Max_dis_Beam;
    public float Max_dis_Boss;

    //プレイヤーと敵の距離
    float dis_Statue;
    float dis_Beam;
    float dis_Boss;

    Combo c;

    public Vector2 targetPosition;

    [SerializeField]
    move1_ver2 m2;
    [SerializeField]
    CinemachineVirtualCamera TargetCamera;
    private CinemachinePOV TargetCameraPov;
    [SerializeField]TPCamera TPCamera;
    public Animator Playerani;
    Rigidbody rb;

    [SerializeField]
    float Rush;
    public int PushCount;

    public Text Time;

    public Vector3 WPosition;

    [SerializeField] GameObject P;
    //[SerializeField] GameObject PlayerHP;
    [SerializeField] Text ComboText;
    [SerializeField] Text ScoreText;
    [SerializeField] GameObject UIParent;
    // Start is called before the first frame update
    void Start()
    {
        mt = GameObject.FindGameObjectWithTag("Manager").GetComponent<multipleTarget>();
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();
        SingleTargetUIPos = SingletargetUI.GetComponent<RectTransform>();
        Player = GameObject.FindGameObjectWithTag("Player");
        SingletargetUI.GetComponent<Image>().enabled = false;
        SpecialtargetUI.GetComponent<Image>().enabled = false;
        //SpecialTargetUIPos = SpecialtargetUI.GetComponent<RectTransform>();
        c = GameObject.FindGameObjectWithTag("Player").GetComponent<Combo>();
        foreach (GameObject obj in targetCount)
        {
            obj.GetComponent<Image>().enabled = false;
        }
        TargetCamera.Priority = 1;
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        PushCount = 0;

        TPCamera = GameObject.FindGameObjectWithTag("TrackingCamera").GetComponent<TPCamera>();
        TargetCameraPov = TargetCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mt.target)
        {
            SingletargetUI.GetComponent<Image>().enabled = false;
            SpecialtargetUI.GetComponent<Image>().enabled = false;
            Vector3 pos;
            for (int i = 0; i < mt.multipleTargetObject.Count; i++)
            {
                if (mt.multipleTargetObject[i] == null)
                {
                    // mt.multipleTargetObject[i]がnullの場合、対応するImageを非表示にする
                    targetCount[i].GetComponent<Image>().enabled = false;
                    continue;
                }

                if (mt.multipleTargetObject[i].CompareTag("Statue") || mt.multipleTargetObject[i].CompareTag("BOSS"))
                {
                    pos = mt.multipleTargetObject[i].GetComponent<IsRendered>().StatueRenderer.bounds.center;
                }
                else
                {
                    pos = mt.multipleTargetObject[i].transform.position;
                }
                targetCount[i].GetComponent<Image>().enabled = true;
                Vector2 position = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
                TargetImagePos = targetCount[i].GetComponent<RectTransform>();
                TargetImagePos.transform.position = new Vector3(position.x, position.y, 0);
            }
        }
        else
        {
            foreach (GameObject obj in targetCount)
            {
                obj.GetComponent<Image>().enabled = false;
            }

        }

        if((t.isTarget_Beam || t.isTarget_Statue || t.isTarget_Boss) && !mt.multiple && !c.SpecialMode && !t.isMoving)
        {
            if (t.isTarget_Statue)
            {
                dis_Statue = Vector3.Distance(Player.transform.position, t.StatuePos2);
                //Debug.Log("距離：" + dis_Statue);

                SingletargetUI.GetComponent<Image>().enabled = true;
                //ターゲットのサイズ変更
                Statue_tilt = (AimUI_Max_Size - AimUI_Min_Size) / Max_dis_Statue;
                if (dis_Statue < Max_dis_Statue && dis_Statue > 0)
                {
                    width = AimUI_Max_Size - dis_Statue * Statue_tilt;
                    height = AimUI_Max_Size - dis_Statue * Statue_tilt;
                }
                SingleTargetUIPos.sizeDelta = new Vector2(width, height);

                //ターゲットの出現場所
                targetPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, t.StatuePos2);
                SingleTargetUIPos.transform.position = new Vector3(targetPosition.x, targetPosition.y, 0f);
            }
            if(t.isTarget_Beam)
            {
                dis_Beam = Vector3.Distance(Player.transform.position, t.BeamPos);
                //Debug.Log("距離：" + dis_Beam);

                //ターゲットのサイズの変更
                Beam_tilt = (AimUI_Max_Size - AimUI_Min_Size) / Max_dis_Beam;
                if (dis_Beam < Max_dis_Beam && dis_Beam > 0)
                {
                    width = AimUI_Max_Size - dis_Beam * Beam_tilt;
                    height = AimUI_Max_Size - dis_Beam * Beam_tilt;
                }

                //ターゲットの出現場所
                targetPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, t.BeamPos);
                SingleTargetUIPos.transform.position = new Vector3(targetPosition.x, targetPosition.y, 0f);

                //ターゲット出現
                SingletargetUI.GetComponent<Image>().enabled = true;
            }
            if (t.isTarget_Boss)
            {
                dis_Beam = Vector3.Distance(Player.transform.position, t.BossPos);
                //Debug.Log("距離：" + dis_Beam);

                //ターゲットのサイズの変更
                Boss_tilt = (AimUI_Max_Size - AimUI_Min_Size) / Max_dis_Beam;
                if (dis_Boss < Max_dis_Boss && dis_Boss > 0)
                {
                    width = AimUI_Max_Size - dis_Boss * Boss_tilt;
                    height = AimUI_Max_Size - dis_Boss * Boss_tilt;
                }

                //ターゲットの出現場所
                targetPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, t.BossPos);
                SingleTargetUIPos.transform.position = new Vector3(targetPosition.x, targetPosition.y, 0f);

                //ターゲット出現
                SingletargetUI.GetComponent<Image>().enabled = true;
            }
        }
        else
        {
            SingletargetUI.GetComponent<Image>().enabled = false;
        }

        if (!mt.multiple && c.Special && (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.LeftShift)))
        {
            PushCount++;
        }
        

        if(PushCount == 1)
        {
            SpecialtargetUI.GetComponent<Image>().enabled = true;
            TargetCamera.Priority = 20;
            m2.enabled = false;
            Playerani.enabled = false;
            //rb.isKinematic = true;
            c.SpecialMode = true;
            P.SetActive(false);
            Vector3 SPosition = new Vector3(0.5f, 0.5f, Rush);
            WPosition = Camera.main.ViewportToWorldPoint(SPosition);
            t.SpecialPosition = WPosition;
            Time.enabled = false;
            UIParent.SetActive(false);
            ComboText.enabled = false;
            ScoreText.enabled = false;
        }
        if (PushCount > 1 || (c.SpecialMode && Input.GetKeyDown("joystick button 0")))
        {
            c.Special = false;
            t.SpecialPosition = WPosition;
            StartCoroutine(t.DelayedStart(1.0f));
            c.SpecialMode = false;
            SpecialtargetUI.GetComponent<Image>().enabled = false;
            TargetCamera.Priority = 1;
            m2.enabled = true;
            Playerani.enabled = true;
            //rb.isKinematic = true;
            PushCount = 0;
            Time.enabled = true;
            P.SetActive(true);
            UIParent.SetActive(true);
            ComboText.enabled = true;
            ScoreText.enabled = true;
            t.SpecialAtStart = true;

            TPCamera.setAngle(-TargetCameraPov.m_HorizontalAxis.Value, TargetCameraPov.m_VerticalAxis.Value);
        }
        if (t.SpecialAtStart)
        {
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
        }
    }
}
