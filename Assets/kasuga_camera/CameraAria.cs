using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraAria : MonoBehaviour
{
    public GameObject vcam1;
    // Start is called before the first frame update
    void Start()
    {
        vcam1 = GameObject.Find("CM vcam1");
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            vcam1.GetComponent<CinemachineVirtualCamera>().Priority = 100;
        }
    }
}
