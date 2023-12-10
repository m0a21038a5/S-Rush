using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultpleFirstLookAt : MonoBehaviour
{
    target t;
    public GameObject MultipleCamera;
    Combo c;
    TargetController tc;
    public bool LookAtBoss;
    // Start is called before the first frame update
    void Start()
    {
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();
        c = GameObject.FindGameObjectWithTag("Player").GetComponent<Combo>();
        tc = GameObject.FindGameObjectWithTag("Target").GetComponent<TargetController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = MultipleCamera.transform.position;
        /*
        if (t.isTarget_Statue)
        {
            // カメラが上方向をVector3.upに向けるようにLookAtを呼び出す
            transform.LookAt(t.StatuePos2, Vector3.up);
        }
        else if (t.isTarget_Beam)
        {
            // カメラが上方向をVector3.upに向けるようにLookAtを呼び出す
            transform.LookAt(t.BeamPos, Vector3.up);
        }
        else if (t.isTarget_Boss)
        {
            // カメラが上方向をVector3.upに向けるようにLookAtを呼び出す
            transform.LookAt(t.BossPos, Vector3.up);
        }
        */

        if(c.SpecialMode)
        {
            int enemyLayerMask = 1 << LayerMask.NameToLayer("Boss");
            Ray ray = new Ray(MultipleCamera.transform.position, tc.WPosition - MultipleCamera.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Vector3.Distance(MultipleCamera.transform.position, tc.WPosition), enemyLayerMask))
            {
                if(hit.collider.CompareTag("BOSS") || hit.collider.gameObject.name.Contains("Variant"))
                {
                    LookAtBoss = true;
                    transform.LookAt(hit.collider.gameObject.GetComponent<IsRendered>().StatueRenderer.bounds.center, Vector3.up);
                }
            }
        }
        
        if(LookAtBoss && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0))
        {
            LookAtBoss = false;
        }
        

    }
}
