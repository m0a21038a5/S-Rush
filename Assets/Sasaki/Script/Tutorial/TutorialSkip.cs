using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSkip : MonoBehaviour
{
    // チュートリアルを△あるいはNキー長押しでスキップする　△って何番?
    public  int framenumber = 180;
    private int frameKey = 0;
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKey("joystick button 3") || Input.GetKey(KeyCode.N))
        {
            frameKey = frameKey + 1;
        }
        if (Input.GetKeyUp("joystick button 3") && Input.GetKeyUp(KeyCode.N))
        {
            frameKey = 0;
        }
        if(frameKey == framenumber)
        {
            SceneManager.LoadScene("Map 1");
        }
    }
}
