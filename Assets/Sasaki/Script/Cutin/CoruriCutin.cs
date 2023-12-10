using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoruriCutin : MonoBehaviour
{//必殺技のカットイン制御スクリプト
    public target t;
    public Image Cutin;
    private Animator CutinAnimator;
    private bool isSECutin;//trueにすると、音を一回だけ鳴らす
    private GameObject playerse;//SEPlayerの取得
    private Soundtest sd;//音のスクリプト取得

    private string CutinAni = "isCutin";
    void Start()
    {
        this.CutinAnimator = GetComponent<Animator>();
        playerse = GameObject.Find("SEPlayer");
        sd = playerse.GetComponent<Soundtest>();
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();
        isSECutin = true;
    }

    void Update()
    {
        if (t.SpecialAtStart == true)
        {
            Cutin.enabled = true;
            this.CutinAnimator.SetBool(CutinAni, true);
            if (isSECutin == true)
            {
                SECutin();
                isSECutin = false;
            }
        }
        else
        {
            isSECutin = true;
            Cutin.enabled = false;
            this.CutinAnimator.SetBool(CutinAni, false);
        }
    }
    public void SECutin()//カットインの音を再生する
    {
        sd.SE_PlayerAttack2Player();
    }
}
