using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSize : MonoBehaviour
{
    Combo com;
    public int PSSize;
    public int defoSize;
    // Start is called before the first frame update
    void Start()
    {
        com = GameObject.FindGameObjectWithTag("Player").GetComponent<Combo>();


    }

    // Update is called once per frame
    void Update()
    {
        PSSize = defoSize;
        if (com.ComboCount <= 10)
        {
            PSSize += com.ComboCount ;
        }
        else 
        {
            PSSize =defoSize+10;
        }
        


        var ParticleSystem = GetComponent<ParticleSystem>();
        var main = ParticleSystem.main;
        main.startSize = PSSize;
    }
}
