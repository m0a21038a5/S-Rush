using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialAttackUIController : MonoBehaviour
{
    Combo c;
    private Animator SpecialAttackUIAnimation;
    private string Pushstr = "IsPushed";
    Image SpecialAttackUI;
    target t;

    //ètì˙í«ãL
    [SerializeField] GameObject aura;
    

    // Start is called before the first frame update
    void Start()
    {
        c = GameObject.FindGameObjectWithTag("Player").GetComponent<Combo>();
        SpecialAttackUI = this.gameObject.GetComponent<Image>();
        SpecialAttackUI.enabled = false;
        SpecialAttackUIAnimation = this.gameObject.GetComponent<Animator>();
        SpecialAttackUIAnimation.enabled = false;
        SpecialAttackUIAnimation.SetBool(Pushstr, false);
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();
    }

    // Update is called once per frame
    void Update()
    {
        if(c.Special)
        {
            //ètì˙í«ãL
            aura.SetActive(true);
        }
        else
        {
            //ètì˙í«ãL
            aura.SetActive(false);
        }

        if(!c.SpecialMode && c.Special)
        {
            SpecialAttackUIAnimation.enabled = true;
            SpecialAttackUIAnimation.SetBool(Pushstr, false);
            if ((Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.LeftShift)))
            {
                SpecialAttackUIAnimation.SetBool(Pushstr, true);
            }
        }
  

    }
}
