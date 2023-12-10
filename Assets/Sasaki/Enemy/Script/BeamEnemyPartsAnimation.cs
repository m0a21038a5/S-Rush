using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamEnemyPartsAnimation : MonoBehaviour
{
    public bool brokenDropParts;
    public MeshCollider BeamMesh;
    public Rigidbody rb;
    public BeamEnemyAnimation bea;
    void Start()
    {
        brokenDropParts = false;
        BeamMesh = GetComponent<MeshCollider>();
        rb = GetComponent<Rigidbody>();
        bea = GetComponentInParent<BeamEnemyAnimation>();
    }

    void Update()
    {
        if (bea.brokenDrop == true)
        {
            brokenDropParts = true;
        }
        if (brokenDropParts == true)
        {
            rb.useGravity = true;
            BeamMesh.enabled = true;
        }
        else if(brokenDropParts == false)
        {
            rb.useGravity = false;
            BeamMesh.enabled = false;
        }
    }
}
