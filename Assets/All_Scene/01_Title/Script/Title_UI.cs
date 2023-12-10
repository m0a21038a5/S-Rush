using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Title_UI : MonoBehaviour
{
    Soundtest st;
    private EventSystem eventSystem;
    private GameObject pastBoj;

    private void Start()
    {
        st = GameObject.Find("SEPlayer").GetComponent<Soundtest>();
        eventSystem = EventSystem.current;
    }

    private void Update()
    {
        if (eventSystem.currentSelectedGameObject != pastBoj && pastBoj != null && eventSystem.currentSelectedGameObject != null)
        {
            st.SE_TargetLockedPlayer();
        }
        pastBoj = eventSystem.currentSelectedGameObject;
    }

    public void ButtonGameStart()
    {
        SceneManager.LoadScene("TutorialStage");
        st.SE_PlayerAttack2Player();
    }

    public void ButtonSetting()
    {

    }

    public void ButtonExit()
    {
        st.negative1Player();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }


}
