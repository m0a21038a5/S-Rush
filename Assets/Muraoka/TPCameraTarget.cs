using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPCameraTarget : MonoBehaviour
{
    // Mainカメラ
    private MainCamera MC;

    public bool isTarget = false;

    private GameObject Player;
    private target target;

    public GameObject targetObj;

    private Vector3 velocity = Vector3.zero;

    public Vector3 offset;


    public float player_y_offset;

    // Start is called before the first frame update
    void Start()
    {
        MC = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCamera>();

        Player = GameObject.FindGameObjectWithTag("Player");
        target = Player.GetComponent<target>();
    }

    // Update is called once per frame
    void Update()
    {
        // ターゲットのGameObject取得
        if (target.TargetStatue != null && MC.isCameraFree == false)
        {
            targetObj = target.TargetStatue;
            isTarget = true;
            offset = new Vector3(0.0f, 5.0f, 0.0f);
        }
        else if (target.TargetBeam != null && MC.isCameraFree == false)
        {
            targetObj = target.TargetBeam;
            isTarget = true;
            offset = new Vector3(0.0f, 0.0f, 0.0f);
        }
        else if (target.TargetBoss != null && MC.isCameraFree == false)
        {
            targetObj = target.TargetBoss;
            isTarget = true;
            offset = new Vector3(0.0f, 15.0f, 0.0f);
        }
        else
        {
            targetObj = Player;
            isTarget = false;
            offset = new Vector3(0.0f, player_y_offset, 0.0f);
        }


        // 
        if ((targetObj.transform.position - transform.position).magnitude > 0.01f)
        {
            if (targetObj == Player)
            {
                transform.position = Vector3.SmoothDamp(transform.position, targetObj.transform.position + offset, ref velocity, 0.1f);
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, targetObj.transform.position + offset, ref velocity, 0.25f);
            }
        }
    }
}
