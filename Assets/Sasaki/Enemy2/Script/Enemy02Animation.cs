using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy02Animation : MonoBehaviour
{
    // 地上敵アニメーション再生
    private string BrokenStr = "isBroken";
    private string AttackStr = "isAttack";
    private string WalkStr = "isWalk";
    public bool BreakNow;
    private Animator Enemy02Ani;
    public StatueEnemyMove sem;
    public StatueHPManager shpm;
    void Start()
    {
        this.Enemy02Ani = GetComponent<Animator>();
        sem = transform.root.gameObject.GetComponent<StatueEnemyMove>();
        shpm = transform.root.gameObject.GetComponent<StatueHPManager>();
        BreakNow = false;
    }

    void Update()
    {
        if (BreakNow == false)
        {
            if (sem.state == "stop")
            {
                this.Enemy02Ani.SetBool(BrokenStr, false);
                this.Enemy02Ani.SetBool(AttackStr, false);
                this.Enemy02Ani.SetBool(WalkStr, false);
            }
            if (sem.state == "patrol" || sem.state == "chase")
            {
                this.Enemy02Ani.SetBool(BrokenStr, false);
                this.Enemy02Ani.SetBool(AttackStr, false);
                this.Enemy02Ani.SetBool(WalkStr, true);
            }
            if (sem.state == "attack")
            {
                this.Enemy02Ani.SetBool(BrokenStr, false);
                this.Enemy02Ani.SetBool(AttackStr, true);
                this.Enemy02Ani.SetBool(WalkStr, false);
            }
        }
        else
        {
            this.Enemy02Ani.SetBool(BrokenStr, true);
            this.Enemy02Ani.SetBool(AttackStr, false);
            this.Enemy02Ani.SetBool(WalkStr, false);
        }
    }
}
