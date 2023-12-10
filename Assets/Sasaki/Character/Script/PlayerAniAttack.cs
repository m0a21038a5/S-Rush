using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAniAttack : MonoBehaviour
{//アニメーション用
    public bool isAttackStart;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Statue" || other.gameObject.tag == "Beam" || other.gameObject.tag == "Boss")
        {
            isAttackStart = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Statue" || other.gameObject.tag == "Beam" || other.gameObject.tag == "Boss")
        {
            isAttackStart = false;
        }
    }
}
