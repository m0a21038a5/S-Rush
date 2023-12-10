using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPause : MonoBehaviour
{
    // Updateメソッドは毎フレーム呼び出されます
    void Update()
    {
        // 一時停止キー（例えばP）が押されたか確認します
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1)
            {
                // ゲームが一時停止していなければ、一時停止します
                Time.timeScale = 0;
            }
            else
            {
                // ゲームが一時停止していれば、再開します
                Time.timeScale = 1;
            }
        }
    }
}
