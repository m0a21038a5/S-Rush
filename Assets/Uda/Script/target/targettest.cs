using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targettest : MonoBehaviour
{
    [SerializeField] TargetController tc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = tc.WPosition;
    }
}
