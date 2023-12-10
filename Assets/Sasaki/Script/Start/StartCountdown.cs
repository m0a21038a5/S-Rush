using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;  //StreamWriterなどを使うために追加

public class StartCountdown : MonoBehaviour
{//最初のステージ開始カウントダウン
    public float StarTextChangeTime;//テキストが出現する時間
    public Text FirstStartText;
    public Text SecondStartText;
    public Text FirstStartTextEn;//英語版テキスト
    public Text SecondStartTextEn;//英語版テキスト
    public Text CountDownText;
    public int MouseClickNumber;//マウスクリックした回数
    private bool NextText;
    private bool CountdownStart;
    private GameObject time;
    private TimeCount1 ts1;
    public bool GameStart;//ゲーム開始判定(どこにも使ってないけとりあえずカウントダウンが終わったらtrueにしてる)
    public GameObject StartBalliaobj;
    public GameObject CloseTextButton;
    public GameObject CloseTextButtonEn;
    private Soundtest sd;//音のスクリプト取得
    private GameObject playerse;//SEPlayerの取得
    public bool isSoundResult01;//trueにすると、音を一回だけ鳴らす　true変更はアニメーション側で設定
    public bool TimeStopStart;//カウントダウン中の時間停止用

    //UIの言語を変更させる
    private string CurrentLanguage;//現在選択している言語
    private JsonType jsonType = new JsonType();
    //保存先
    string datapath;
    void Awake()
    { //保存先の計算をする
        //datapath = Application.dataPath + "/RankingScoreSave.json";
        //StreamingAssetsフォルダを指定. /以降にファイル名RankingScoreSave.json
        //StreamingAssetsフォルダに指定することでビルドしても使用可能
        datapath = Application.streamingAssetsPath + "/RankingScoreSave.json";
    }
    void Start()
    {
        jsonType = loadJsonData();
        CurrentLanguage = jsonType.Language;
        playerse = GameObject.Find("SEPlayer");
        sd = playerse.GetComponent<Soundtest>();
        NextText = false;
        GameStart = false;
        CountdownStart = false;
        TimeStopStart = true;
        ts1 = GameObject.Find("Time").GetComponent<TimeCount1>();
        if (CurrentLanguage == "Japanese")
        {
            FirstStartText.enabled = true;
            FirstStartTextEn.enabled = false;
            CloseTextButton.SetActive(true);
            CloseTextButtonEn.SetActive(false);
        }
        else if (CurrentLanguage == "English")
        {
            FirstStartText.enabled = false;
            FirstStartTextEn.enabled = true;
            CloseTextButton.SetActive(false);
            CloseTextButtonEn.SetActive(true);
        }
    }

    void Update()
    { 
        if(TimeStopStart == true)
        {
            Time.timeScale = 0;
        }else if(GameStart == true || GameObject.FindGameObjectWithTag("Player").GetComponent<target>().SpecialAttack)
        {
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Space))
        {
            MouseClickNumber++;
        }
        if (MouseClickNumber == 1)
        {
            
            if (NextText == false)
            {
                SoundStartClose();
                StartTextAnimation();
                if (CurrentLanguage == "Japanese")
                {
                    FirstStartText.enabled = false;
                    FirstStartTextEn.enabled = false;
                    SecondStartText.enabled = true;
                    SecondStartTextEn.enabled = false;
                }
                else if (CurrentLanguage == "English")
                {
                    FirstStartText.enabled = false;
                    FirstStartTextEn.enabled = false;
                    SecondStartText.enabled = false;
                    SecondStartTextEn.enabled = true;
                }
            }

        }
        if (MouseClickNumber == 2)
        {
            EndTextAnimation();
            FirstStartText.enabled = false;
            SecondStartText.enabled = false;
            SecondStartTextEn.enabled = false;
            CloseTextButton.SetActive(false);
            CloseTextButtonEn.SetActive(false);
            if (NextText == true)
            {
                CountdownStart = true;
            }
            if (CountdownStart == true)
            {
                NextText = false;
                StartCoroutine(ReadygoCoroutine());//一回だけカウントダウンの処理をする
            }
        }
        if (MouseClickNumber == 3)
        {
          
        }
       
    }
    void StartTextAnimation()//テキストを伸び縮みさせる
    {
        var sequence = DOTween.Sequence().SetUpdate(true);
        sequence.Append(transform.DOScale(new Vector3(0, 1, 1), StarTextChangeTime))
            .Append(transform.DOScale(new Vector3(1, 1, 1), StarTextChangeTime));
        NextText = true;
    }
   
    void EndTextAnimation()
    {
        transform.DOScale(new Vector3(1, 1, 1), StarTextChangeTime).SetUpdate(true);
    }
    void StartGame()//ゲーム開始
    {
        StartCoroutine(ReadygoCoroutine());
    }
    IEnumerator ReadygoCoroutine()//ゲーム開始カウントダウン
    {
        SoundStartCountdown();
        CountdownStart = false;
        CountDownText.text = "3";
       Debug.Log("3");
        yield return new WaitForSecondsRealtime(1.0f);
        CountDownText.text = "2";
        Debug.Log("2");
        yield return new WaitForSecondsRealtime(1.0f);
        CountDownText.text = "1";
        Debug.Log("1");
        yield return new WaitForSecondsRealtime(1.0f);
        CountDownText.text = "Start!";
        Debug.Log("0");
        yield return new WaitForSecondsRealtime(0.5f);
        ts1.TimeClearStop = false;
        TimeStopStart = false;
        GameStart = true;
        StartBalliaobj.SetActive(false) ;
        this.gameObject.SetActive(false);
        MouseClickNumber++;
        
    }
    public void SoundStartCountdown()// SE_321()音を鳴らすための関数
    {
        sd.SE_321Player();
    }
    public void SoundStartClose()// SE_321()音を鳴らすための関数
    {
        sd.SE_PlayerAttack2Player();
    }

    //JSONファイルを読み込み, ロードするための関数
    public JsonType loadJsonData()
    {
        string datastr = "";
        StreamReader reader;
        reader = new StreamReader(datapath);
        datastr = reader.ReadToEnd();
        reader.Close();

        return JsonConvert.DeserializeObject<JsonType>(datastr);
    }
}

