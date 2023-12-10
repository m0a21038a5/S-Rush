using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickAnim : MonoBehaviour
{
    public float moveSpeed = 33f; // 上に移動する速度（ピクセル/秒）
    public float moveDistance = 13f; // 移動する距離（ピクセル）
    private Vector3 initialPosition;
    private bool movingUp = true;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        // 上に移動するか、下に戻るかを決定
        if (movingUp)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

            // 移動距離がmoveDistanceを超えたら下に戻る
            if (transform.localPosition.y - initialPosition.y >= moveDistance)
            {
                movingUp = false;
            }
        }
        else
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

            // 初期位置に戻ったら再び上に移動
            if (transform.localPosition.y <= initialPosition.y)
            {
                movingUp = true;
            }
        }
    }
}