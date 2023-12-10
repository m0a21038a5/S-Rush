using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeCollider : MonoBehaviour
{
    public GameObject cone;
    public MeshCollider coneCollider;
    // Start is called before the first frame update
    void Start()
    {
        cone = GameObject.Find("Cone");
        coneCollider = cone.GetComponent<MeshCollider>();
        coneCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
