using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SpaceDistortVer2 : MonoBehaviour
{//ゆがみをつけるエフェクトを制御するスクリプトです
    //SpaceDistortProcessVolume Intensityの最小値　
    //SpaceDistortTime　変化にかかる時間　　SpaceDistortChangeValue　ゆがみの変化量
    public PostProcessVolume SpaceDistortProcessVolume;
    private LensDistortion SpaceDistortPostProcessLensDistortion;
    public float SpaceDistortValue = -100.0f;
    public float SpaceDistortTime = 0.2f;
    public float SpaceDistortChangeValue = 20.0f;
    public bool SpaceDistortStart;
    public bool SpaceDistortFinal;
    public GameObject player1;
    public target Target1;
    //targetから敵のオブジェクトを取得
    target t;
    void Start()
    {
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();
        SpaceDistortStart = false;
        SpaceDistortFinal = false;
        player1 = GameObject.Find("Player");
        Target1 = player1.GetComponent<target>();
    }
    void Update()
    {
        foreach (PostProcessEffectSettings item in SpaceDistortProcessVolume.profile.settings)
        {
            if (item as LensDistortion)
            {
                SpaceDistortPostProcessLensDistortion = item as LensDistortion;
            };
        }
        //左クリックしたら空間をゆがませる
        if (t.ismove_Statue == true || t.ismove_Beam == true)
        {
            if (SpaceDistortPostProcessLensDistortion)
            {
                //SpaceDistortStart = true;
                StopCoroutine(SpaceDistortEffectCoroutine());
                StartCoroutine(SpaceDistortEffectCoroutine());
                //SpaceDistortEffect();
            }
            else
            {
                if (SpaceDistortPostProcessLensDistortion)
                {
                    // TimeStopPostProcessColorGrading.Intensity.value = 50.0f;
                }
            }
        }


        if (SpaceDistortStart == true)
        {
            SpaceDistortPostProcessLensDistortion.intensity.value -= SpaceDistortChangeValue;
            if (SpaceDistortPostProcessLensDistortion.intensity.value < SpaceDistortValue)
            {
                SpaceDistortStart = false;
            }
        }
        if (SpaceDistortFinal == true)
        {
            SpaceDistortPostProcessLensDistortion.intensity.value += SpaceDistortChangeValue;
            if (SpaceDistortPostProcessLensDistortion.intensity.value > 0)
            {
                SpaceDistortPostProcessLensDistortion.intensity.value = 0.0f;
                SpaceDistortFinal = false;
            }
        }
    }

    IEnumerator SpaceDistortEffectCoroutine()
    {
        SpaceDistortValue = 0;
        SpaceDistortStart = true;
        SpaceDistortFinal = false;
        yield return new WaitForSecondsRealtime(SpaceDistortTime);
        SpaceDistortStart = false;
        SpaceDistortFinal = true;
    }
}