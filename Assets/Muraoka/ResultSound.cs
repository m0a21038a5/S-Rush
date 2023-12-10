using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResultSound : MonoBehaviour
{
    Soundtest st;
    private EventSystem eventSystem;
    private GameObject pastBoj;

    private void Start()
    {
        eventSystem = EventSystem.current;
        st = GameObject.Find("SEPlayer").GetComponent<Soundtest>();// Ç±Ç±ÇÕàÍî‘â∫ÇÃçsÇ…ÇµÇƒÇ≠ÇæÇ≥Ç¢
    }

    private void Update()
    {
        if (eventSystem.currentSelectedGameObject != pastBoj && pastBoj != null && eventSystem.currentSelectedGameObject != null)
        {
            st.SE_TargetLockedPlayer();
        }
        pastBoj = eventSystem.currentSelectedGameObject;
    }

    public void playChoisesSE()
    {
        st.SE_TargetLockedPlayer();
    }
}
