using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Infinityjump : MonoBehaviour
{
    int cmdSeq = 0;
    int[] keyCodes;
    int[] konamiCommand = new[] {
        (int)KeyCode.T,
        (int)KeyCode.U,
        (int)KeyCode.N,
        (int)KeyCode.E,
        (int)KeyCode.T,
        (int)KeyCode.E,
        (int)KeyCode.R,
        (int)KeyCode.U
    };
    int kcnt = 0;

    public bool Infinity;

    private void Start()
    {
        keyCodes = (int[])Enum.GetValues(typeof(KeyCode));
    }

    void Update()
    {
        var len = keyCodes.Length;
        for (var i = 0; i < len; i++)
        {
            if (Input.GetKeyUp((KeyCode)keyCodes[i]))
            {
                if (konamiCommand[cmdSeq] == keyCodes[i])
                {
                    cmdSeq++;
                    if (cmdSeq == konamiCommand.Length)
                    {
                        Infinity = true;
                    }
                }
                else
                {
                    cmdSeq = 0;
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            cmdSeq = 0;
            Infinity = false;
        }
    }
}
