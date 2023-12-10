using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSize2 : MonoBehaviour
{
    Combo com;
    public float PSSize;
    public float defoSize;
    // Start is called before the first frame update
    void Start()
    {
        com = GameObject.FindGameObjectWithTag("Player").GetComponent<Combo>();


    }

    // Update is called once per frame
    void Update()
    {
        if (com.ComboCount <= 10)
        {
            PSSize = defoSize;
            PSSize += com.ComboCount * 0.1f;
        }
        else 
        {
            PSSize = 1f+defoSize;
        }



        var ParticleSystem = GetComponent<ParticleSystem>();
        var main = ParticleSystem.main;
        main.startSize = PSSize;
    }
}
