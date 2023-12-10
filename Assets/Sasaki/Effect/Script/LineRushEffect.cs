using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRushEffect : MonoBehaviour
{
    public GameObject LineRushEffectObject;
    public float LineRushEffectTime=1.0f;

    //targetから敵のオブジェクトを取得
    target t;
    //comboからComboCountの取得
    Combo combo;
    //Comboを敵に当たった時の一回のみ追加されるようにするための変数
    public int CountEnemy;
    void Start()
    {
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();
        combo = GameObject.FindGameObjectWithTag("Player").GetComponent<Combo>();
        LineRushEffectObject.SetActive(false);
    }

    void Update()
    {
        CountEnemy = combo.CountEnemyCombo;
      //  if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("joystick button 7") || Input.GetKeyDown("joystick button 0")) { 
            if (t.ismove_Statue == true|| t.ismove_Beam == true || t.isTarget_Boss == true)
            {
            //LineRushEffectObject.SetActive(true);
             StopCoroutine(SpaceDistortEffectCoroutine());
            StartCoroutine(SpaceDistortEffectCoroutine());
        }
        //}
    }

    IEnumerator SpaceDistortEffectCoroutine()
    {
        //Debug.Log("集中線");
        LineRushEffectObject.SetActive(true);
        yield return new WaitForSecondsRealtime(LineRushEffectTime);
        LineRushEffectObject.SetActive(false);

    }
}
