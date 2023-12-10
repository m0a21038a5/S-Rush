using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButtonAnim : MonoBehaviour
{
    public float pulseSpeed = 2.0f; // パルスの速度
    public float minScale = 0.9f;   // 最小スケール
    public float maxScale = 1.5f;   // 最大スケール

    private Vector3 initialScale;
    private bool scalingUp = true;


    // Start is called before the first frame update
    void Start()
    {
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

        // 現在のスケール
        Vector3 currentScale = transform.localScale;

        // パルスの動き
        float scaleFactor = Mathf.PingPong(Time.time * pulseSpeed, 1);
        float newScale = Mathf.Lerp(minScale, maxScale, scaleFactor);

        // スケールを更新
        transform.localScale = initialScale * newScale;
    }
}
