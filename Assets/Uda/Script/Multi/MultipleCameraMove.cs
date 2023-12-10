using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MultipleCameraMove : MonoBehaviour
{
    
    public CinemachineVirtualCamera virtualCamera;
    public CinemachinePOV pov;
    public Vector3 newInitialOrientation;
    public GameObject MultiObject;
    multipleTarget mt;
    int ChangeCount;
    Combo c;
    target t;
    [SerializeField] GameObject main;
    [SerializeField] float Stoptime;

    private void Start()
    {        
        // CinemachineVirtualCameraÇ©ÇÁCinemachineComposerÇéÊìæ
        pov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        mt = GameObject.FindGameObjectWithTag("Manager").GetComponent<multipleTarget>();
        c = GameObject.FindGameObjectWithTag("Player").GetComponent<Combo>();
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();
    }

    private void Update()
    {
        if ((mt.ChangeCamera || c.SpecialMode)&& ChangeCount == 0)
        {
            StartCoroutine(CameraMoveStop(Stoptime));
            //ChangeCount++;
        }
        if(!mt.ChangeCamera && !c.SpecialMode)
        {
            ChangeCount = 0;
        }
        

        if((mt.ChangeCamera || c.SpecialMode) && ChangeCount > 0)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                pov.m_HorizontalAxis.m_InputAxisName = "Horizontal";
                pov.m_VerticalAxis.m_InputAxisName = "Vertical";
            }
            else
            {
                // ÇÊÇËí·Ç¢óDêÊìxÇÃInputÇê›íË
                pov.m_HorizontalAxis.m_InputAxisName = "Mouse X";
                pov.m_VerticalAxis.m_InputAxisName = "Mouse Y";
            }
        }

        if(c.SpecialMode)
        {
            if(MultiObject.GetComponent<MultpleFirstLookAt>().LookAtBoss)
            {
                float verticalRotation = MultiObject.transform.rotation.eulerAngles.x;
                float horizontalRotation = MultiObject.transform.rotation.eulerAngles.y;
                pov.m_VerticalAxis.Value = verticalRotation;
                pov.m_HorizontalAxis.Value = horizontalRotation;
            }
        }
    }


    private IEnumerator CameraMoveStop(float Stop)
    {
        float verticalRotation = main.transform.rotation.eulerAngles.x;
        float horizontalRotation = main.transform.rotation.eulerAngles.y;
        pov.m_VerticalAxis.Value = verticalRotation;
        pov.m_HorizontalAxis.Value = horizontalRotation;
        pov.m_HorizontalAxis.m_InputAxisName = "";
        pov.m_VerticalAxis.m_InputAxisName = "";

        yield return new WaitForSecondsRealtime(Stop);

        ChangeCount++;
    }
   
}
