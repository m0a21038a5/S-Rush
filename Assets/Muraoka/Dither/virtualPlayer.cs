using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class virtualPlayer : MonoBehaviour
{
    void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")) * 5.0f * Time.deltaTime;
    }
}
