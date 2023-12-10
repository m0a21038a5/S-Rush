using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    // HPを減らすためのスクリプトです
    //攻撃が当たる場所につけてください(ビーム敵ならビームにつけて、近接敵なら腕の先とか)
    //SliderのMaxValueを最大HPにしてください
    // DamageValueはダメージの大きさです

    public PlayerHP pHP;
    public StatueEnemyMove SEM;
    public bool isDamaged = false;
    
    //private GameObject PlayerHPSlider;
    void Start()
    {
        pHP = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHP>();
        SEM = GetComponentInParent<StatueEnemyMove>();
    }

    private void Update()
    {
        if (SEM.stateTransitionCount < 0.1f)
        {
            isDamaged = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && isDamaged == false && SEM.stateTransitionCount > 0.1f)
        {//プレイヤーに当たったらDamageValueの分だけ減らす

            pHP.Damage = true;
            isDamaged = true;
            pHP.DamageCount = 0;
        }
    }


    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && pHP.DamageCount == 0)
        {//プレイヤーに当たったらDamageValueの分だけ減らす
           
            pHP.Damage = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
           pHP.DamageCount = 0;
        }
    }*/
}
