using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationSR : MonoBehaviour
{//第一回OC用のアニメーション制御用スクリプトです
    private Animator SRPlayerAnimator;
    private string RunStr = "isRun";
    private string RushStr = "isRush";
    private string WaitStr = "isWait";
    private string AttackStart = "isAttackStart";
    private string AttackAir = "isAttackAir";
    private string AttackMain = "isAttackMain";
    private string AttackEnd = "isAttackEnd";

    private float StickValueL;
    private float StickValueW;
    private bool StickValueLB;
    private bool StickValueWB;

    //comboスクリプトから突進中を取得
    public int CountEnemyRush;
    Combo  combo;
    target t;
    //PlayerAniParentから地面判定を取得
    PlayerAniParent pap;
    private bool isAir;//飛んでるとき
    public bool isAttack;//攻撃中判定
    public GameObject AttackArea;
    PlayerAniAttack paa;
    void Start()
    {
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();
        combo = GameObject.FindGameObjectWithTag("Player").GetComponent<Combo>();
        pap = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAniParent>();
        paa = AttackArea.GetComponent<PlayerAniAttack>();
        this.SRPlayerAnimator = GetComponent<Animator>();
        isAttack = false;
        isAir = false;
    }

    void Update()
    {
        CountEnemyRush = combo.CountEnemyCombo;
        StickValueL = Input.GetAxisRaw("Horizontal");
        StickValueW = Input.GetAxisRaw("Vertical");
        if (StickValueL == 0)
        {
            StickValueLB = false;
        } else
        {
            StickValueLB = true;
        }
        if (StickValueW == 0)
        {
            StickValueWB = false;
        }else
        {
            StickValueWB = true;
        }
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) ||
              Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)
              || StickValueWB == true || StickValueLB ==true)&& pap.isGround == true)
        {
            this.SRPlayerAnimator.SetBool(RunStr, true);
        }  else { 
            this.SRPlayerAnimator.SetBool(RunStr, false); 
        }

        
        if (((Input.GetKey("joystick button 0") || Input.GetMouseButton(0)) && t.Attack==true)&&isAttack==false)
        {
            this.SRPlayerAnimator.SetBool(AttackStart, true);
            this.SRPlayerAnimator.SetBool(AttackEnd, false);
            isAttack = true;

        } else
        {
           // this.SRPlayerAnimator.SetBool(AttackStart, false);
        }
        if (isAttack == true)
        {
            if (((Input.GetKey("joystick button 0") || Input.GetMouseButton(0)) && t.Attack == true))
            {
                this.SRPlayerAnimator.SetBool(AttackAir, true);
                this.SRPlayerAnimator.SetBool(AttackMain, false);
            }
        }
        if (t.ismove_Beam == true || t.ismove_Boss == true || t.ismove_Statue == true)
        {
            this.SRPlayerAnimator.SetBool(AttackAir, true);
            this.SRPlayerAnimator.SetBool(AttackMain, false);
            paa.isAttackStart = false;
        }
        // if (paa.isAttackStart== true)
        // if(t.Attack==true)

         if (pap.isEnemy == true)
        //if (paa.isAttackStart == true)
        {
                this.SRPlayerAnimator.SetBool(AttackMain, true);
                this.SRPlayerAnimator.SetBool(AttackAir, false);
            //主人公のサイズを変更する
        }
        else
        {
          // this.SRPlayerAnimator.SetBool(AttackMain, false);
            //this.SRPlayerAnimator.SetBool(AttackAir, true);
        }/*
        if (combo.Special && (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.LeftShift)))
        {
            this.SRPlayerAnimator.SetBool(AttackStart, true);
            this.SRPlayerAnimator.SetBool(AttackAir, true);
            if (pap.isEnemy == true)
            {
                this.SRPlayerAnimator.SetBool(AttackMain, true);
                this.SRPlayerAnimator.SetBool(AttackAir, false);
            }else
            {

            }
        }*/
        if (pap.isGround == true)
        {
            this.SRPlayerAnimator.SetBool(WaitStr, true);
            this.SRPlayerAnimator.SetBool(AttackEnd, true);
            this.SRPlayerAnimator.SetBool(AttackStart, false);
            this.SRPlayerAnimator.SetBool(AttackAir, false);
            this.SRPlayerAnimator.SetBool(AttackMain, false);
            isAttack = false;
            if (combo.Special && (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.LeftShift)))
            {
                this.SRPlayerAnimator.SetBool(WaitStr, false);
                this.SRPlayerAnimator.SetBool(AttackAir, true);
                this.SRPlayerAnimator.SetBool(AttackMain, false);
            }
            }
        else {
            this.SRPlayerAnimator.SetBool(AttackEnd, false);
            this.SRPlayerAnimator.SetBool(AttackAir,true);
            this.SRPlayerAnimator.SetBool(WaitStr, false);
        }
        if (combo.Special && (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.LeftShift)))
        {
            this.SRPlayerAnimator.SetBool(AttackAir, true);
            this.SRPlayerAnimator.SetBool(AttackMain, false);
        }
            /*
            if(t.Attack == false)
            {
                this.SRPlayerAnimator.SetBool(RushStr, false);
            }*/
        }
}
