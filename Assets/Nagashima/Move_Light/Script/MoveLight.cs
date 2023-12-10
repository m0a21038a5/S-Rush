using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLight : MonoBehaviour
{
    int count = 0;

    int angle = 600;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        count++;

        if (count < angle)
        {
            transform.Rotate(0, 0, 0.1f);
        }
        else
        {
            transform.Rotate(0, 0, -0.1f);
        }

        if (count > angle * 2)
        {
            count = 0;
        }
    }
}

