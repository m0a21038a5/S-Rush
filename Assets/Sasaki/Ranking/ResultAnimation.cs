using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;  //StreamWriterなどを使うために追加

public class ResultAnimation : MonoBehaviour
{
    //リザルト画面アニメーション
    private string TextRankingScoreStr = "isTextRankingScore";
    private string TextRankingTimeStr = "isTextRankingTime";
    private string TextStaffStr = "isTextStaff";
    private string TextButtonStr = "isButtonTItle";
    public GameObject RightBoxAll;
    public GameObject RightBoxRankingScore;
    public GameObject RightBoxRankingTime;
    public GameObject RightBoxStaff;
    public GameObject RightTitleButton;
    public Image[] CoinAllImage;
    public Image[] CoinDottLineAllImage;
    private Animator ResultAni;
    public int MouseClickNumber;//マウスクリックした回数
    public bool isMouseClick;
    public int[] MiniBossAllcoin;
    private Soundtest sd;//音のスクリプト取得
    private GameObject playerse;//SEPlayerの取得
    public bool isSoundResult01;//trueにすると、音を一回だけ鳴らす　true変更はアニメーション側で設定
    public bool isSoundResult02;//音を一回だけ鳴らす
    public bool isSoundResultCoin;//音を一回だけ鳴らす
    public bool isSoundResultRush;//音を一回だけ鳴らす
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
    {//マウスを一回にして、前半は自動で動かす、
        this.ResultAni = GetComponent<Animator>();
        playerse = GameObject.Find("SEPlayer");
        sd = playerse.GetComponent<Soundtest>();
        RightBoxRankingScore.SetActive(false);
        RightBoxRankingTime.SetActive(false);
        RightBoxStaff.SetActive(false);
        RightTitleButton.SetActive(false);
        isMouseClick = false;
        jsonType = loadJsonData();
        MiniBossAllcoin = jsonType.ClearCoin;
        //コインの枚数に応じて表示させる
        for (int i = 0; i < MiniBossAllcoin.Length; i++)
        {
            if (MiniBossAllcoin[i] == 1)
            {
                CoinAllImage[i].enabled = true;
                CoinDottLineAllImage[i].enabled = false;
            }
            else if (MiniBossAllcoin[i] == 0)
            {
                CoinAllImage[i].enabled = false;
                CoinDottLineAllImage[i].enabled = true;
            }
           
        }
    }

    void Update()
    {
        if (isMouseClick == true)
        {
            if (Input.GetKeyDown("joystick button 2") || Input.GetMouseButtonDown(0)|| Input.GetKeyDown("joystick button 1"))
            {
                MouseClickNumber++;
            }
        }
        if (MouseClickNumber == 1)
        {
            isMouseClick = true;
            this.ResultAni.SetBool(TextRankingScoreStr, true);
            RightBoxAll.SetActive(false);
            RightBoxRankingScore.SetActive(true);

        }
        if (MouseClickNumber == 2)
        {
            isMouseClick = true;
            this.ResultAni.SetBool(TextRankingTimeStr, true);
            RightBoxRankingScore.SetActive(false);
            RightBoxRankingTime.SetActive(true);
        }
        if (MouseClickNumber == 3)
        {
            isMouseClick = true;
            this.ResultAni.SetBool(TextStaffStr, true);
            RightBoxRankingTime.SetActive(false);
            RightBoxStaff.SetActive(true);
        }
        if (MouseClickNumber == 4)
        {
            isMouseClick = true;
            this.ResultAni.SetBool(TextButtonStr, true);
            RightBoxStaff.SetActive(false);
            RightTitleButton.SetActive(true);
            //コインの枚数をリセットする
            for (int i = 0; i < MiniBossAllcoin.Length; i++)
            {
                MiniBossAllcoin[i]=0;
            }
            jsonType.ClearCoin = MiniBossAllcoin;
            var jsonBody = JsonConvert.SerializeObject(jsonType);
            JsonType outPut = JsonConvert.DeserializeObject<JsonType>(jsonBody);
            saveJsonData(jsonBody);
        }
        if (isSoundResult01 == true) //result1Player()音を鳴らす
        {
            SoundResult01();
            isSoundResult01 = false;
        }
        if (isSoundResult02 == true)// result2Player()音を鳴らすための関数
        {
            SoundResult02();
            isSoundResult02 = false;
        }
        if (isSoundResultCoin == true)//SE_getCoinPlayer()音を鳴らす
        {
            SoundResultCoin();
            isSoundResultCoin = false;
        }
        if (isSoundResultRush == true)// kazePlayer()音を鳴らす
        {
            SoundResultRush();
            isSoundResultRush = false;
        }
    }
    //セーブするための関数
    public void saveJsonData(string text)
    {
        StreamWriter writer;

        //JSONファイルに書き込み
        writer = new StreamWriter(datapath, false);
        writer.Write(text);
        writer.Flush();
        writer.Close();
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
    public void SoundResult01()// result1Player()音を鳴らすための関数(アニメーション側で実行したほうが早いかも)
    {
        sd.result1Player();
    }
    public void SoundResult02()// result2Player()音を鳴らすための関数(アニメーション側で実行したほうが早いかも)
    {
        sd.result2Player();
    }
    public void SoundResultCoin()//SE_getCoinPlayer()音を鳴らすための関数(アニメーション側で実行したほうが早いかも)
    {
        sd.SE_getCoinPlayer();
    }
    public void SoundResultRush()// kazePlayer()音を鳴らすための関数(アニメーション側で実行したほうが早いかも)
    {
        sd.kazePlayer();
    }

}
