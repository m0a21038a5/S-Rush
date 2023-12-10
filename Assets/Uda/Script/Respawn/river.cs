using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class river : MonoBehaviour
{
    RespawnManager t;
    Fadeout FO;
    // Start is called before the first frame update
    void Start()
    {
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<RespawnManager>();
        FO = GameObject.FindGameObjectWithTag("FadeOut").GetComponent<Fadeout>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            FO.fadeout = true;
        }
    }
}
