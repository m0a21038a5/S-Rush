using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Cinemachine;

public class Fadeout : MonoBehaviour
{
    public GameObject FadePanel;
    Image fadealpha;
    private float alpha = 0;
    public bool fadeout = false;
    public bool fadein = false;
    public bool fadeinstart = false;
    public int ScwneNo = 0;
    public GameObject cameraobj;
    public GameObject cameratargetobj;

    [SerializeField]
    UnityEngine.UI.Image image;


    RespawnManager R;

    private TrackingCamera TC;

    private CinemachineVirtualCamera CVC;
    // このVirtualCameraのCVCコンポーネントのBodyの部分
    private CinemachineTransposer CT;
    // このVirtualCameraのCVCコンポーネントのAimの部分
    private CinemachineComposer CC;

    // Start is called before the first frame update
    void Start()
    {
        fadealpha = FadePanel.GetComponent<Image>();
        alpha = fadealpha.color.a;
        R = GameObject.FindGameObjectWithTag("Player").GetComponent<RespawnManager>();
        cameraobj = GameObject.FindGameObjectWithTag("Camera");
        cameratargetobj = GameObject.FindGameObjectWithTag("CameraTarget");
        TC = GameObject.FindGameObjectWithTag("TrackingCamera").GetComponent<TrackingCamera>();
        CVC = GameObject.FindGameObjectWithTag("TrackingCamera").GetComponent<CinemachineVirtualCamera>();
        CT = CVC.GetCinemachineComponent<CinemachineTransposer>();
        CC = CVC.GetCinemachineComponent<CinemachineComposer>();


    }

    // Update is called once per frame
    void Update()
    {

        if (fadeout == true)
        {
            StartCoroutine("FadeOut");

        }


    }

    public void FadeOutON()
    {

        this.image.DOFade(endValue: 1f, duration: 1f).OnComplete(Respawn);

    }
    public void FadeInON()
    {
        this.image.DOFade(endValue: 0f, duration: 2f);


    }

    public void Respawn()
    {
        R.respawn = true;
        fadeout = false;
    }

    IEnumerator FadeOut()
    {
        FadeOutON();

        //2秒停止
        yield return new WaitForSeconds(2f);

        FadeInON();

        yield return new WaitForSeconds(1f);

        CVC.m_LookAt = cameratargetobj.transform;

    }
}
