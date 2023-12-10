using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearResultAni : MonoBehaviour
{
    public bool isClearAni;//リザルト画面とつなぐクリアアニメーション
    private Animator ClearAni;
    public Clear c;
    void Start()
    {
        this.ClearAni = GetComponent<Animator>();
    }
    void Update()
    {
        if (c.isDead==true)
        {
            isClearAni = true;
        }
        if (isClearAni == true)
        {
            ClearAni.enabled = true;
        }
    }
}
