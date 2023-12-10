using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    private target ta;

    void Start()
    {
        GameObject Playerobj = GameObject.Find("Player");
        ta = Playerobj.GetComponent<target>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    //ƒGƒŠƒA‚É“G‚ª‚¢‚é‚©‚Ç‚¤‚©
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Statue")
        {
            ta.isTarget_Statue = true;
            Debug.Log("a");
        }

        if (other.gameObject.tag == "Beam")
        {
            ta.isTarget_Beam = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Statue")
        {
            ta.isTarget_Statue = false;
        }

        if (other.gameObject.tag == "Beam")
        {
            ta.isTarget_Beam = false;
        }
    }
    
}
