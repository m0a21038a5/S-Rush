using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public float DamageValue = 0.1f;
    private float CurrentHP;
    private float FirstHP;
    private Slider PlayerHPSlider;

    RespawnManager R;
    public bool Damage;
    public int DamageCount = 0;

    Fadeout FO;

    // PlayerSoundsスクリプト--------村岡追加--------
    private PlayerSounds ps;
    // Start is called before the first frame update
    void Start()
    {
        PlayerHPSlider = GameObject.FindWithTag("PlayerHP").GetComponent<Slider>();
        FirstHP = PlayerHPSlider.maxValue;
        R = GameObject.FindGameObjectWithTag("Player").GetComponent<RespawnManager>();
        CurrentHP = FirstHP;
        Damage = false;

        FO = GameObject.FindGameObjectWithTag("FadeOut").GetComponent<Fadeout>();
        // PlayerSoundsスクリプト取得--------村岡追加--------
        ps = GameObject.FindWithTag("Player").GetComponent<PlayerSounds>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerHPSlider.value = CurrentHP;
        if (CurrentHP <= 0)
        {
            //R.respawn = true;
            FO.fadeout = true;
            CurrentHP += FirstHP;
            Damage = false;
            DamageCount = 0;
            Debug.Log("Dead");
            // プレイヤー死亡音フラグON--------村岡追加--------
            ps.isPlayDeadSound = true;
        }

        if(Damage == true && DamageCount == 0)
        {
            CurrentHP -= DamageValue;
            DamageCount++;
            // プレイヤーダメージ音フラグON--------村岡追加--------
            ps.isPlayDamageSound = true;
        }

       
    }

   
}
