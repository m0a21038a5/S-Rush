using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class AfterimageEffect : MonoBehaviour
{
    //残像のポストプロセスを制御するスクリプトです
    //AfterimageEffectTimeが残像を表示させる時間です
    public PostProcessVolume AfterimageProcessVolume;
    private MotionBlur AfterimagePostProcessMotionBlur;
    public float AfterimageEffectTime = 1.0f;
    public GameObject player3;
    public target Target3;
    //targetから敵のオブジェクトを取得
    target t;
    void Start()
    {
        player3 = GameObject.Find("Player");
        Target3 = player3.GetComponent<target>();
    }

    void Update()
    {
        foreach (PostProcessEffectSettings item in AfterimageProcessVolume.profile.settings)
        {
            if (item as MotionBlur)
            {
                AfterimagePostProcessMotionBlur = item as MotionBlur;
            };
        }
        if (Target3.ismove_Statue || Target3.ismove_Beam)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(SpaceDistortEffectCoroutine());
            }
        }
        else
        {
            AfterimagePostProcessMotionBlur.shutterAngle.value = 0;
            AfterimagePostProcessMotionBlur.sampleCount.value = 0;
        }
    }

    IEnumerator SpaceDistortEffectCoroutine()
    {
        AfterimagePostProcessMotionBlur.shutterAngle.value = 270;
        AfterimagePostProcessMotionBlur.sampleCount.value = 10;
        yield return new WaitForSecondsRealtime(AfterimageEffectTime);
        AfterimagePostProcessMotionBlur.shutterAngle.value = 0;
        AfterimagePostProcessMotionBlur.sampleCount.value = 0;

    }
}
