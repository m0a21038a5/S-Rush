using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    RespawnManager R;
    // Start is called before the first frame update
    void Start()
    {
        R = GameObject.FindGameObjectWithTag("Player").GetComponent<RespawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            R.CP = this.gameObject.transform;
            R.CPobj = this.gameObject;
            R.position = this.transform.position;
            R.lookup = true;
        }
    }

  
}
