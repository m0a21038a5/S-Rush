using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChainGage_Script : MonoBehaviour
{
    // スライダーの取得
    public Slider ChainGage;

    [SerializeField] private float ChainLimit;

    [SerializeField] GameObject ChainUI;
    
    private float d_ChainGage;

    // Start is called before the first frame update
    void Start()
    {
        d_ChainGage = ChainGage.maxValue / ChainLimit;
    }

    // Update is called once per frame
    void Update()
    {
        ChainGage.value -= d_ChainGage;

        if(ChainGage.value <= 0)
        {

        }
    }
}
