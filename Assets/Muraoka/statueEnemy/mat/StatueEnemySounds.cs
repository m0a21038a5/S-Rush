using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueEnemySounds : MonoBehaviour
{
    public bool isPlayWalkingSound = false;
    public bool isPlayAttackSound = false;
    public bool isPlayDeadSound = false;

    private float moveValue;
    private Vector3 beforeFramePos;
    private bool isStartASDP;


    private StatueEnemyMove SEM;

    // Start is called before the first frame update
    void Start()
    {
        moveValue = 0.0f;
        beforeFramePos = transform.position;
        SEM = GetComponent<StatueEnemyMove>();
    }

    // Update is called once per frame
    void Update()
    {
        moveValue += Vector3.Magnitude(transform.position - beforeFramePos);
        beforeFramePos = transform.position;

        if (moveValue >= 2.0f)
        {
            isPlayWalkingSound = true;
            moveValue = 0.0f;
        }

        if (SEM.state == "attack" && SEM.stateTransitionCount < 1.0f && isStartASDP == false)
        {
            isPlayAttackSound = true;
            isStartASDP = true;
        }
        else
        {
            isStartASDP = false;
        }

    }
}