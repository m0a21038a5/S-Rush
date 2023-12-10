using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZoneMesh : MonoBehaviour
{
    public List<MeshRenderer> DangerMesh;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(DangerMesh != null)
        {
           foreach(MeshRenderer dm in DangerMesh)
            {
                dm.enabled = false;
            }
        }
    }
}
