using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfetImagePlayer : MonoBehaviour
{
    // プレイヤーにのみ残像を表示させます
    public bool OnlyPlayerAfterImageEffect;//これにチェックを入れるとプレイヤーにのみ残像エフェクトが付きます
    
    public GameObject AfterImageEffectObject;
    public float PlayerAfterImageEffectTime;
    public GameObject player3;
    public target Target3;
    void Start()
    {
        AfterImageEffectObject = GameObject.Find("AfetImagePlayer");
        player3 = GameObject.Find("Player");
        Target3 = player3.GetComponent<target>();
        AfterImageEffectObject.SetActive(false);
    }
    void Update()
    {
        if(OnlyPlayerAfterImageEffect == true)
        {
            if (Target3.ismove_Statue || Target3.ismove_Beam)
            {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(AfterImageEffectCoroutine());
            } 
            }
        }
        else
        {
            AfterImageEffectObject.SetActive(false);
        }
    }

    IEnumerator AfterImageEffectCoroutine()
    {
        AfterImageEffectObject.SetActive(true);
        yield return new WaitForSecondsRealtime(PlayerAfterImageEffectTime);
        AfterImageEffectObject.SetActive(false);

    }
}
