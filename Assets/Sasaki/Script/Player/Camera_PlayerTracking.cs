using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_PlayerTracking : MonoBehaviour
{
    //CameraTarget カメラが追跡する対象
    public GameObject CameraTarget;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - CameraTarget.transform.position;
    }

    void Update()
    {
        //カメラが追跡する
        transform.position = CameraTarget.transform.position + offset;
    }
}
