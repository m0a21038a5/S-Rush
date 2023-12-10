using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputDebug : MonoBehaviour
{
    void Update()
    {
        // 全てのキーをチェック
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            // キーが押されているかどうかをチェック
            if (Input.GetKey(keyCode))
            {
                // キーの名前をデバッグログに表示
                Debug.Log("キーが押されています: " + keyCode.ToString());
            }
        }
    }
}
