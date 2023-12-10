using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PrologueButtonController : MonoBehaviour
{
    public GameObject SkipButton;
    EventSystem eventSystem;
    Soundtest st;
    // Start is called before the first frame update
    void Start()
    {
        eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(SkipButton);
        SkipButton.SetActive(false);
        st = GameObject.Find("SEPlayer").GetComponent<Soundtest>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            eventSystem.SetSelectedGameObject(SkipButton);
            SkipButton.SetActive(true);
            st.SE_ButtonPlayer();
        }
    }
}
