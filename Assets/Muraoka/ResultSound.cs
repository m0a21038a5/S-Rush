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
        st = GameObject.Find("SEPlayer").GetComponent<Soundtest>();// ここは一番下の行にしてください
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
