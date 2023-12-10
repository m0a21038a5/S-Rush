using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    //操作説明Canvas
    public GameObject Tutorial;
    //メニューCanvas
    public GameObject Menu;
    //Retry確認画面
    public GameObject Retry;
    //ReturnToTitle確認画面
    public GameObject RTT;
    //音量調節画面
    public GameObject Sd;
    //カメラ調節画面
    public GameObject Cd;

    Soundtest st;

    // Start is called before the first frame update

    void Start()
    {
        st = GameObject.Find("SEPlayer").GetComponent<Soundtest>();
    }
    //Retryボタンを押したとき
    public void ToRePlay()
    {
        //Retry確認画面表示
        Retry.SetActive(true);
        //Menu画面非表示
        Menu.SetActive(false);
        st.positive1Player();
    }

    //ReturnToTitleボタンを押した時
    public void ToTitle()
    {
        //ReturnToTitle確認画面表示
        RTT.SetActive(true);
        //Menu画面非表示
        Menu.SetActive(false);
        st.positive1Player();
    }

    //Tutorialボタンを押したとき
    public void ToTutorial()
    {
        //操作説明画面を表示
        Tutorial.SetActive(true);
        //メニュー画面を非表示
        Menu.SetActive(false);
        st.positive1Player();
    }

    //操作説明画面で戻るボタンを押したとき
    public void ReturnToMenu()
    {
        //操作説明画面を非表示
        Tutorial.SetActive(false);
        //メニュー画面を表示
        Menu.SetActive(true);
        st.negative1Player();
    }

    //メニュー画面で×印を押したとき
    public void closeMenu()
    {
        //メニュー画面を非表示
        Menu.SetActive(false);
        Time.timeScale = 1;
        st.negative1Player();
    }

    //Retryします
    public void Retry_Y()
    {
        //現在のシーンを再読み込み
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        st.SE_ButtonPlayer();
    }

    //Retryしません
    public void Retry_N()
    {
        //Retry確認画面非表示
        Retry.SetActive(false);
        //Menu画面表示
        Menu.SetActive(true);
        st.negative1Player();
    }

    //タイトルに戻る
    public void RTT_Y()
    {
        //Titleシーンへ遷移
        SceneManager.LoadScene("Title 1");
        Time.timeScale = 1;
        st.SE_ButtonPlayer();
        
    }

    //タイトルに戻らない
    public void RTT_N()
    {
        //ReturnToTitle確認画面非表示
        RTT.SetActive(false);
        //Menu画面表示
        Menu.SetActive(true);
        st.negative1Player();
    }

    public void ToSd()
    {
        //メニュー画面非表示
        Menu.SetActive(false);
        //音量調節画面表示
        Sd.SetActive(true);
        st.positive1Player();
    }

    public void closeSd()
    {
        //メニュー画面表示
        Menu.SetActive(true);
        //音量調節画面非表示
        Sd.SetActive(false);
        st.negative1Player();
    }

    public void ToCd()
    {
        //メニュー画面非表示
        Menu.SetActive(false);
        //音量調節画面表示
        Cd.SetActive(true);
        st.positive1Player();
    }

    public void closeCd()
    {
        //メニュー画面表示
        Menu.SetActive(true);
        //音量調節画面非表示
        Cd.SetActive(false);
        st.negative1Player();
    }

    public void playChoicesSE()
    {
        st.SE_TargetLockedPlayer();
    }
}
