using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class RespawnManager : MonoBehaviour
{
    

    public Transform CP;
    public Vector3 position;

    public Vector3 lookpos;
    public GameObject lookatobj;
    public Vector3 playerlookpos;
    public GameObject cameraobj;
    public GameObject CPobj;
    public bool lookup = false;
    private TrackingCamera TC;
    private CinemachineVirtualCamera CVC;
    // このVirtualCameraのCVCコンポーネントのBodyの部分
    private CinemachineTransposer CT;
    // このVirtualCameraのCVCコンポーネントのAimの部分
    private CinemachineComposer CC;

    public bool respawn;
    // Start is called before the first frame update
    void Start()
    {
        cameraobj = GameObject.FindGameObjectWithTag("Camera");
        TC = GameObject.FindGameObjectWithTag("TrackingCamera").GetComponent<TrackingCamera>();
        CVC = GameObject.FindGameObjectWithTag("TrackingCamera").GetComponent<CinemachineVirtualCamera>();
        CT = CVC.GetCinemachineComponent<CinemachineTransposer>();
        CC = CVC.GetCinemachineComponent<CinemachineComposer>();
    }

    
    // Update is called once per frame
    void Update()
    {        
        if(respawn == true)
        {
            playerlookpos = lookpos;
            playerlookpos.y = this.transform.position.y;
            this.transform.LookAt(playerlookpos);
            CVC.m_LookAt = lookatobj.transform;

            this.transform.position = position;
            respawn = false;
        }
      
    }
}
