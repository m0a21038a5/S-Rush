using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamDead : MonoBehaviour
{
    private target ta;

    private single si;

    public bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        GameObject Playerobj = GameObject.Find("Player");
        GameObject ConeObj = GameObject.Find("Cone");
        ta = Playerobj.GetComponent<target>();
        si = ConeObj.GetComponent<single>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            Destroy(this.gameObject);
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && ta.ismove_Beam == true)
        {
            isDead = true;
            ta.isTarget_Beam = false;
            ta.ismove_Beam = false;

            ta.BeamPos = Vector3.zero;
            si.ListClear();
        }
    }
}
