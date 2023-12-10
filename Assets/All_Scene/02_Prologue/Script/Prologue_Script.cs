using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Prologue_Script : MonoBehaviour
{
    Soundtest st;
    // Start is called before the first frame update
    void Start()
    {
        st = GameObject.Find("SEPlayer").GetComponent<Soundtest>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.anyKey)
        {
            SceneManager.LoadScene("Map");
        }
        */

    }

    public void skip()
    {
        SceneManager.LoadScene("Map 1");
        st.SE_ButtonPlayer();
    }
}
