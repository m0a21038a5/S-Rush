using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  

    //メニューCanvas
    public GameObject Menu;
    //操作説明Canvas
    public GameObject Tutorial;
    //表示されている画面の状態
    public int MenuCount = 0;
    //player用
    [SerializeField]
    GameObject Player;
    //Retry確認画面
    public GameObject Retry;
    //ReturnToTitle確認画面
    public GameObject RTT;
    //音量調節画面
    public GameObject Sd;
    //カメラ調節画面
    public GameObject Cd;
    //BGM音量調節Slider
    public  GameObject BGMSlider;
    //SE音量調節Slider
    public GameObject SESlider;
    
    Soundtest SE;
    BGMPlayer BGM;
    //近接敵用の配列
    GameObject[] Statues;
  
    public MainCamera mc;
    GameObject[] BeamObjects;

    public GameObject Boss;
    Clear c;

    public GameObject StartCountDown;
    StartCountdown SCD;
    public bool isDisplay;

    public bool Boss1 = false;
    public bool Boss2 = false;
    public bool Boss3 = false;

    // Start is called before the first frame update
    void Start()
    {
        //メニュー非表示
        Menu.SetActive(false);
        //操作説明画面非表示
        Tutorial.SetActive(false);
        //Retry確認画面非表示
        Retry.SetActive(false);
        //ReturnToTitle確認画面非表示
        RTT.SetActive(false);
        //音量調節画面非表示
        Sd.SetActive(false);
        //音量調節画面非表示
        Cd.SetActive(false);

        SE = GameObject.Find("SEPlayer").GetComponent<Soundtest>();
        BGM = GameObject.Find("BGMPlayer").GetComponent<BGMPlayer>();
        SE.menuObj = Menu;
        SE.menuObj2 = Tutorial;
        SE.menuObj3 = RTT;
        SE.menuObj4 = Sd;// Cdもやらんとあかん
        SE.p = Player.GetComponent<PlayerSounds>();
        BGM.p = Player.GetComponent<PlayerSounds>();

        BGM.volumeSlider = BGMSlider.GetComponent<Slider>();
        SE.volumeSlider = SESlider.GetComponent<Slider>();

        BGM.volumeSlider.value = BGM.volumeSlider.maxValue = 1.0f;
        BGM.volumeSlider.minValue = 0.0f;

        SE.volumeSlider.value = SE.volumeSlider.maxValue = 1.0f;
        SE.volumeSlider.minValue = 0.0f;

        c = Boss.GetComponent<Clear>();
        mc.enabled = true;
        //マウスカーソル非表示＆中央に固定----------------村岡追加----------------
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SCD = StartCountDown.GetComponent<StartCountdown>();
        isDisplay = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Escapeキーを1度押した時
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 9")) && MenuCount == 0)
        {
            //メニュー表示
            Menu.SetActive(true);
            // 効果音
            SE.positive1Player();
        }
        //Escapeキーを2度押したとき
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 9")) && MenuCount == 1)
        {
            //メニュー非表示
            Menu.SetActive(false);
            Time.timeScale = 1;
            //マウスカーソル非表示＆中央に固定----------------村岡追加----------------
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            // 効果音
            SE.negative1Player();
        }
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 9")) && MenuCount == 2)
        {
            //メニュー表示
            Menu.SetActive(true);
            
            //操作説明非表示
            Tutorial.SetActive(false);
           
            //Retry確認画面非表示
            Retry.SetActive(false);
           
            //ReturnToTitle確認画面非表示
            RTT.SetActive(false);

            //音量調節画面非表示
            Sd.SetActive(false);

            //カメラ調節画面非表示
            Cd.SetActive(false);

            // 効果音
            SE.negative1Player();
        }
        //メニューが表示されている場合
        if (Menu.activeSelf == true)
        {
            MenuCount = 1;
            Time.timeScale = 0;
            BGM.aisac2 = 1;
         
            //操作説明非表示
            Tutorial.SetActive(false);
            //Retry確認画面非表示
            Retry.SetActive(false);
            //ReturnToTitle確認画面非表示
            RTT.SetActive(false);
            //ReturnToTitle確認画面非表示
            Sd.SetActive(false);
            //ReturnToTitle確認画面非表示
            Cd.SetActive(false);
            mc.enabled = false;
            //マウスカーソル表示＆自由に移動可能----------------村岡追加----------------
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        //メニュー画面から派生した画面が表示されている場合
        if (isDisplay == true || SCD.GameStart == false || Tutorial.activeSelf == true || Retry.activeSelf == true || RTT.activeSelf == true || Sd.activeSelf == true || Cd.activeSelf == true)
        {
            MenuCount = 2;
            BGM.aisac2 = 1;

            Time.timeScale = 0;
            mc.enabled = false;
            //マウスカーソル表示＆自由に移動可能----------------村岡追加----------------
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
      
        //メニューと操作説明画面どちらも表示されていない場合
        if (isDisplay == false &&SCD.GameStart == true && Menu.activeSelf == false && Tutorial.activeSelf == false && Retry.activeSelf == false && RTT.activeSelf == false && Sd.activeSelf == false && Cd.activeSelf == false)
        {
            MenuCount = 0;
            BGM.aisac2 = 0;

            mc.enabled = true;
            //マウスカーソル非表示＆中央に固定----------------村岡追加----------------
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            if (c.isDead == false)
            {
                Time.timeScale = 1;
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
    }
}
