using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class statueDead : MonoBehaviour
{
    private target ta;
    private single si;

    public bool isDead=false;
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
            //Destroy(this.gameObject);
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player"&&ta.ismove_Statue)
        {
            isDead = true;
            ta.isTarget_Statue = false;
            ta.ismove_Statue = false;
            ta.StatuePos2 = Vector3.zero;
            //si.ListClear();
        }
    }
}
