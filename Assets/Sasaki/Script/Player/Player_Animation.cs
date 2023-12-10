using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animation : MonoBehaviour
{
    private Animator PlayerAnimator;
    private string runStr = "isRun";
    private string JumpStr = "isJump";
    void Start()
    {
        this.PlayerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        PlayerAnimation();
    }

    void PlayerAnimation()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            this.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            this.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if(Input.GetKey(KeyCode.RightArrow)|| Input.GetKey(KeyCode.LeftArrow)|| Input.GetKey(KeyCode.UpArrow)|| Input.GetKey(KeyCode.DownArrow)
            || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D))
        {
            this.PlayerAnimator.SetBool(runStr, true);
        }
        else
        {
            this.PlayerAnimator.SetBool(runStr, false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.PlayerAnimator.SetBool(JumpStr, true);
        }
        else
        {
            this.PlayerAnimator.SetBool(JumpStr, false);
        }
    }
}
