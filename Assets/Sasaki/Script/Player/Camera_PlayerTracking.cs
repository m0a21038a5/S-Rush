using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_PlayerTracking : MonoBehaviour
{
    //CameraTarget ƒJƒƒ‰‚ª’ÇÕ‚·‚é‘ÎÛ
    public GameObject CameraTarget;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - CameraTarget.transform.position;
    }

    void Update()
    {
        //ƒJƒƒ‰‚ª’ÇÕ‚·‚é
        transform.position = CameraTarget.transform.position + offset;
    }
}
