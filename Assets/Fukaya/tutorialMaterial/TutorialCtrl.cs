using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCtrl : MonoBehaviour
{
    public GameObject[] canvasArray;
    public int currentIndex = 0;
    public GameObject[] colArray;
    private int colIndex = 0;
    public GameObject Lstick;
    public GameObject sikaku;
    public GameObject R2;
    public bool displaying;
    public bool stopping;
    public GameObject start;
    public GameObject StartCountDown;
    StartCountdown SCD;
    public GameObject GO;
    ButtonManager BM;
    GameManager GM;

    // Start is called before the first frame update
    void Start()
    {
        SCD = StartCountDown.GetComponent<StartCountdown>();
        GO = GameObject.FindWithTag("Manager");
        BM = GO.GetComponent<ButtonManager>();
        GM = GO.GetComponent<GameManager>();

        sikaku.SetActive(false);
        R2.SetActive(false);
        Lstick.SetActive(false);
        foreach (GameObject obj in canvasArray)
        {
            obj.SetActive(false);
        }
        displaying = true;
        stopping = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<target>().SpecialAttack)
        {
            if (stopping == false)
            {
                Time.timeScale = 1;
            }
            if (stopping == true)
            {
                Time.timeScale = 0;
            }
        }


        if(start.activeSelf != false)
        {
            //Time.timeScale = 0;
            //stopping = true;
            
        }
        if (SCD.GameStart == true && currentIndex == 0)
        {
            Lstick.SetActive(true);
            stopping = false;
        }


        if (currentIndex == 1 && displaying ==true)
        {
            Lstick.SetActive(false);
            sikaku.SetActive(true);
            displaying = false;
        }
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown("joystick button 0")))
        {
            sikaku.SetActive(false);
        }
        if (currentIndex == 2 && displaying == true)
        {
            R2.SetActive(true);
            displaying = false;

        }
        if (currentIndex == 3 || Input.GetMouseButtonDown(1) || Input.GetKeyDown("joystick button 7"))
        {
            R2.SetActive(false);
        }
       

        if (SCD.GameStart == true && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 1")))
        {
            foreach (GameObject obj in canvasArray)
            {
                obj.SetActive(false);
                stopping = false;
                GM.isDisplay = false;

            }
        }

        if(SCD.TimeStopStart == true || BM.Menu.activeSelf == true || BM.Tutorial.activeSelf == true || BM.Retry.activeSelf == true || BM.RTT.activeSelf == true || BM.Sd.activeSelf == true || BM.Cd.activeSelf == true)
        {
            stopping = true;
        }
        else
        {
            stopping = false;
        }
    

        if (canvasArray[0].activeSelf == true || canvasArray[1].activeSelf == true || canvasArray[2].activeSelf == true || canvasArray[3].activeSelf == true)
        {
            stopping = true;
        }


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == colArray[colIndex])
        {
            if (currentIndex < canvasArray.Length)
            {                
                canvasArray[currentIndex].SetActive(true);
                displaying = true;
                stopping = true;
                currentIndex++;
                colIndex++;
                GM.isDisplay = true;
            
            }
            Destroy(other);
            
        }
    }

}
