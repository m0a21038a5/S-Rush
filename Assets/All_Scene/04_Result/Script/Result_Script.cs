using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Result_Script : MonoBehaviour
{

  
    Soundtest st;

    [SerializeField] 
    public GameObject Staff;
    [SerializeField]
    public GameObject AllResultButton;
    public bool isstaff = false;


    // Start is called before the first frame update
    void Start()
    {
        st = GameObject.Find("SEPlayer").GetComponent<Soundtest>();
        isstaff = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isstaff == true) 
        {
            Staff.SetActive(true);
            AllResultButton.SetActive(false);
            if (Input.anyKey) 
            {
                Staff.SetActive(false);
                AllResultButton.SetActive(true);
                isstaff = false;
            }
        }
    }

    public void ButtonYes()
    {
        SceneManager.LoadScene("Map 1");
        st.SE_ButtonPlayer();
    }

    public void ButtonNo()
    {
        SceneManager.LoadScene("Title 1");
        st.negative1Player();
        /*
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
        */
    }

    public void ButtonStaff() 
    {
        //Staff.SetActive(true);
        //AllResultButton.SetActive(false);
        isstaff = true;
    }

}
