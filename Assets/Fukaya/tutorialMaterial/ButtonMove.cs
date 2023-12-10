using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMove : MonoBehaviour
{
    public GameObject STT;
    UIChangeLanguage UICL;
    public string CurL;
    public float XOffset;
    private Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {

        originalPosition = this.transform.localPosition;
        UICL = STT.GetComponent<UIChangeLanguage>();
        CurL = UICL.CurrentLanguage;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurL == "Japanese")
        {
            this.transform.localPosition = originalPosition;
        }

        else if(CurL == "English")
        {
            this.transform.localPosition = (originalPosition + new Vector3(XOffset, 0, 0));
        }

    }
}
