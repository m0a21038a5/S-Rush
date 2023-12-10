using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationTarget : MonoBehaviour
{
    Combo c;
    target t;
    private Animator SpecialTargetUIAnimation;
    private string Finishstr = "IsFinished";
    Image SpecialTargetUI;

    // Start is called before the first frame update
    void Start()
    {
        c = GameObject.FindGameObjectWithTag("Player").GetComponent<Combo>();
        SpecialTargetUI = this.gameObject.GetComponent<Image>();
        //SpecialTargetUI.enabled = false;
        SpecialTargetUIAnimation = this.gameObject.GetComponent<Animator>();
        //SpecialTargetUIAnimation.enabled = false;
        SpecialTargetUIAnimation.SetBool(Finishstr, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (c.Special && (t.isTarget_Beam || t.isTarget_Statue || t.isTarget_Boss))
        {
            SpecialTargetUI.enabled = true;
            SpecialTargetUIAnimation.enabled = true;
        }
        else
        {
            SpecialTargetUI.enabled = false;
            //SpecialTargetUIAnimation.enabled = false;
            SpecialTargetUIAnimation.SetBool(Finishstr, true);
        }
    }
}
