using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffectController : MonoBehaviour
{
    Combo c;
    target t;
    private Animator SpecialUIAnimation;
    private string Finishstr = "isSpecial";
  
    // Start is called before the first frame update
    void Start()
    {
        c = GameObject.FindGameObjectWithTag("Player").GetComponent<Combo>();
        SpecialUIAnimation = this.gameObject.GetComponent<Animator>();
        SpecialUIAnimation.SetBool(Finishstr, true);
    }

    // Update is called once per frame
    void Update()
    {
        if(c.SpecialMode)
        {
            SpecialUIAnimation.SetBool(Finishstr, true);
        }
        if(!c.SpecialMode)
        {
            SpecialUIAnimation.SetBool(Finishstr, false);
        }
    }
}
