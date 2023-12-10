using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSettingManager : MonoBehaviour
{
    [SerializeField] Slider CSS;
    [SerializeField] Toggle OYT;
    [SerializeField] Text OYTS;

    private void Awake()
    {
        CSS.value = TPCamera.Stick_sensi;
        OYT.isOn = TPCamera.isOperateY;
        if (OYT.isOn == true)
        {
            OYTS.text = "ON";
        }
        else
        {
            OYTS.text = "OFF";
        }
    }

    public void SetCameraSensi()
    {
        TPCamera.Stick_sensi = (int)CSS.value;
    }

    public void SetOperationY()
    {
        TPCamera.isOperateY = OYT.isOn;
        if (OYT.isOn == true)
        {
            OYTS.text = "ON";
        }
        else
        {
            OYTS.text = "OFF";
        }
    }

}
