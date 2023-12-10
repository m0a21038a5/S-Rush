using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAniParent : MonoBehaviour
{//アニメーション用
    public bool isGround;
    public bool isEnemy;
    private bool SpeAttack;
    Combo combo;
    void Start()
    {
        combo = GameObject.FindGameObjectWithTag("Player").GetComponent<Combo>();
    }
    void Update()
    {
       if( combo.Special && (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.LeftShift))){
            SpeAttack = true;
        }
        else
        {
            SpeAttack = false;
        }
        
    }
    private void OnCollisionEnter(Collision other)
    {
            if (other.gameObject.tag == "Floor")
            {
                isGround = true;
            }
            if (other.gameObject.tag == "Statue" || other.gameObject.tag == "Beam" || other.gameObject.tag == "Boss")
            {
                isEnemy = true;
            }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Floor")
        {
            isGround = false;
        }
        if (other.gameObject.tag == "Statue" || other.gameObject.tag == "Beam"|| other.gameObject.tag == "Boss")
        {
            isEnemy= false;
        }
    }
    private void OnCollisionStay(Collision other)
    {
            if ((other.gameObject.tag == "Floor"))
            {
                isGround = true;
            }
    }
    }
