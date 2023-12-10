using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackSound : MonoBehaviour
{

    public Soundtest st;
    public BGMPlayer bp;
    public target t;
    public Combo c;

    public bool isPlayedSPBP = false;
    public int a;

    // Start is called before the first frame update
    void Start()
    {
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();
        c = GameObject.FindGameObjectWithTag("Player").GetComponent<Combo>();

        st = GameObject.Find("SEPlayer").GetComponent<Soundtest>();
        bp = GameObject.Find("BGMPlayer").GetComponent<BGMPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (c.SpecialMode == true && isPlayedSPBP == false)
        {
            st.SE_SPButtonPlayer();
            bp.ChainNumber = 10;
            isPlayedSPBP = true;
        }
        if (c.SpecialMode == false && isPlayedSPBP == true)
        {
            bp.ChainNumber = 0;
            isPlayedSPBP = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Statue" || other.gameObject.tag == "Beam" || other.gameObject.tag == "BOSS")
        {
            st.SE_SuperAttackPlayer();
        }
    }
}
