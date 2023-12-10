using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HighlightChoices : MonoBehaviour
{
    private EventSystem eventSystem;
    [SerializeField] GameObject Choices;
    [SerializeField] GameObject BackHighlight;

    void Start()
    {
        eventSystem = EventSystem.current;
    }

    void Update()
    {
        if (eventSystem.currentSelectedGameObject == Choices)
        {
            BackHighlight.SetActive(true);
        }
        else
        {
            BackHighlight.SetActive(false);
        }
    }
}
