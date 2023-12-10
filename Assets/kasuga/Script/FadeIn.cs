using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    Fadeout FO;
    // Start is called before the first frame update
    void Start()
    {
        FO = GameObject.FindGameObjectWithTag("FadeOut").GetComponent<Fadeout>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (FO.fadeinstart == true)
        {
            FO.fadein = true;

        }

    }
}
