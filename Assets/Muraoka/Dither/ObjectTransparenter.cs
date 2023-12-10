using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTransparenter : MonoBehaviour
{
    private MeshRenderer mesh;
    private GameObject camera;
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void FixedUpdate()
    {
        if (Vector3.Magnitude(camera.transform.position - transform.position) < 7.5f && mesh.material.color.a > 0.5f)
        {
            mesh.material.color = mesh.material.color - new Color32(0, 0, 0, 5);
        }
        else if (Vector3.Magnitude(camera.transform.position - transform.position) < 15.0f)
        {
            if (mesh.material.color.a > Vector3.Magnitude(camera.transform.position - transform.position) / 15.0f )
            {
                mesh.material.color = mesh.material.color - new Color32(0, 0, 0, 5);
            }
            else
            {
                mesh.material.color = mesh.material.color + new Color32(0, 0, 0, 5);
            }
        }
        else if (mesh.material.color.a < 1.0f)
        {
            mesh.material.color = mesh.material.color + new Color32(0, 0, 0, 5);
        }
    }

}
