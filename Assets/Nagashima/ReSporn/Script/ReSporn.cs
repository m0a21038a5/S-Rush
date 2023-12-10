using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSporn : MonoBehaviour
{

    public GameObject DethEria;
    public GameObject DethEria01;
    public Vector3 ReSporn_Point;
    public Vector3 ReSporn_Point01;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter(Collision collision)
    {
        Transform myTransform = this.transform;

        if (collision.gameObject == DethEria)
        {
            myTransform.position = ReSporn_Point;   
        }
        if (collision.gameObject == DethEria01)
        {
            myTransform.position = ReSporn_Point01;
        }

    }
}