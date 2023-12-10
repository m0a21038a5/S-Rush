using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleEventController : MonoBehaviour
{
    public GameObject StartButton;
    EventSystem eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(StartButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
