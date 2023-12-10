using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour
{
    EventSystem eventSystem;

    //Canvas
    public GameObject Menu;
    public GameObject Retry;
    public GameObject RTT;
    public GameObject Tutorial;
    public GameObject Sd;
    public GameObject Cd;

    //最初にカーソルを置いておくボタン
    public GameObject MenuButton;
    public GameObject RetryButton;
    public GameObject RTTButton;
    public GameObject TutorialButton;
    public GameObject SdButton;
    public GameObject CdButton;

    //カーソルを最初の一度だけ止める変数
    private int MenuCount = 0;
    private int RetryCount = 0;
    private int RTTCount = 0;
    private int TutorialCount = 0;
    private int SdCount = 0;
    private int CdCount = 0;

    Soundtest st;
    private GameObject pastBoj;

    // Start is called before the first frame update
    void Start()
    {
        eventSystem = EventSystem.current;

        st = GameObject.Find("SEPlayer").GetComponent<Soundtest>();
        eventSystem = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {
        if(Menu.activeSelf == true && MenuCount == 0)
        {
            eventSystem.SetSelectedGameObject(MenuButton);
            MenuCount++;
            RetryCount = 0; 
            RTTCount = 0;
            SdCount = 0;
            CdCount = 0;
            TutorialCount = 0; 
        }
        if(Retry.activeSelf == true && RetryCount == 0)
        {
            eventSystem.SetSelectedGameObject(RetryButton);
            MenuCount = 0;
            RetryCount++;
            RTTCount = 0;
            SdCount = 0;
            CdCount = 0;
            TutorialCount = 0;
        }
        if(RTT.activeSelf == true && RTTCount == 0)
        {
            eventSystem.SetSelectedGameObject(RTTButton);
            MenuCount = 0;
            RetryCount = 0;
            RTTCount++;
            SdCount = 0;
            CdCount = 0;
            TutorialCount = 0;
        }
        if(Tutorial.activeSelf == true && TutorialCount == 0)
        {
            eventSystem.SetSelectedGameObject(TutorialButton);
            MenuCount = 0;
            RetryCount = 0;
            RTTCount = 0;
            SdCount = 0;
            CdCount = 0;
            TutorialCount++;
        }
        if(Sd.activeSelf == true && SdCount == 0)
        {
            eventSystem.SetSelectedGameObject(SdButton);
            MenuCount = 0;
            RetryCount = 0;
            RTTCount = 0;
            SdCount++;
            CdCount = 0;
            TutorialCount = 0;
        }
        if (Cd.activeSelf == true && CdCount == 0)
        {
            eventSystem.SetSelectedGameObject(CdButton);
            MenuCount = 0;
            RetryCount = 0;
            RTTCount = 0;
            SdCount = 0;
            CdCount++;
            TutorialCount = 0;
        }

        if (eventSystem.currentSelectedGameObject != pastBoj && pastBoj != null && eventSystem.currentSelectedGameObject != null)
        {
            st.SE_TargetLockedPlayer();
        }
        pastBoj = eventSystem.currentSelectedGameObject;
    }
}
