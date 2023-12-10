using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;  //StreamWriterなどを使うために追加

public class RankingScore : MonoBehaviour
{//ランキングのスクリプト　演出とかはまだ　表示だけ　
    public float[] ClearScoreList;
    public float[] ClearScoreRanking;
    public GameObject AllResultButton;
    List<float> RankingList = new List<float>();//リスト
    List<float> RankingListTime = new List<float>();//タイムリスト
    public float[] RankingListSave;//ランキング保存
    public float[] RankingTimeSave;//タイム保存
    private int PlayerRank;//プレイヤーの順位
    public Text RankingText;
    public Text RankingTimeText;
    public Text RankingTimeTextPlayer;
    public Text PlayerText;
    private JsonType jsonType = new JsonType();
    public GameObject RankingAll;
    public GameObject Stafroll;
    public bool StafrollOn;
    private bool AllKeyUp;
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
        Debug.Log(jsonType.Key1);
        RankingListSave = jsonType.Key1;
        RankingTimeSave = jsonType.ClearScoreRanking;
        //JSONファイルがあればロード, なければ初期化関数へ
        if (FindJsonfile())
        {
            jsonType= loadJsonData();
            Debug.Log(jsonType.Key1);
        }
        else
        {
            Initialize();
        }
        //「SCORE」というキーで保存されているFloat値を読み込み
        float ClearScoreNow = PlayerPrefs.GetFloat("SCORE");
        //「ClearTime」で保存されているFloat値を読み込み
        float ClearTimeNow = jsonType.ClearTime;
        // ClearScoreListに保存されているランキングを読み取って
        //ClearScoreRanking配列にClearScoreNowとClearScoreListを入れて
        RankingList.AddRange(RankingListSave);
        RankingList.Add(ClearScoreNow);
        RankingListTime.AddRange(RankingTimeSave);
        RankingListTime.Add(ClearTimeNow);

        //リストを降順に並べ替える
        RankingList.Sort();
        RankingList.Reverse();
        RankingListTime.Sort();

        //スクロール無しバージョン、5位まで表示させるために6以下を切り捨て
        RankingList.RemoveAt(5);
        RankingListTime.RemoveAt(5);
        //並べ替えた配列を表示させる
        //リスト
        for (int i = 0; i < RankingList.Count; i++)
        {
            //RankingText.text =RankingText.text +"\n"+ (i+1) + "位　Score：" + RankingList[i] + "!!"; //テキストの上書き
            RankingText.text = RankingText.text + "\n" + (i + 1) + "　" + RankingList[i]; //テキストの上書き
        }
        for (int i = 0; i < RankingListTime.Count; i++)
        {
            //RankingTimeText.text = RankingTimeText.text + "\n" + (i + 1) + "位　Score：" + RankingListTime[i] + "!!"; //テキストの上書き
            RankingTimeText.text = RankingTimeText.text + "\n" + (i + 1) + "　"+((int)(RankingListTime[i] / 60)).ToString("00") + ":" + ((int)(RankingListTime[i] % 60)).ToString("00"); //テキストの上書き
         
        }

        //配列を保存する
        //listRankingListを配列に変換しRankingListSaveに保存
        //RankingListを保存
        RankingListSave = RankingList.ToArray();
        jsonType.Key1 = RankingListSave;
        Debug.Log(jsonType.Key1 + "保存その0!");
        /*var jsonBody = JsonConvert.SerializeObject(jsonType);
        Debug.Log(jsonBody+"保存その１!");
        JsonType outPut = JsonConvert.DeserializeObject<JsonType>(jsonBody);
        Debug.Log(outPut.Key1+"保存その２!");
        saveJsonData(jsonBody);*/
        //配列を保存する
        //listRankingListを配列に変換しRankingListSaveに保存
        //RankingListを保存
        RankingTimeSave = RankingListTime.ToArray();
        jsonType.ClearScoreRanking = RankingTimeSave;
        var jsonBody = JsonConvert.SerializeObject(jsonType);
        JsonType outPut = JsonConvert.DeserializeObject<JsonType>(jsonBody);
        saveJsonData(jsonBody);
        PlayerText.text = ClearScoreNow.ToString();
        //RankingTimeTextPlayer.text = ClearTimeNow.ToString();
        RankingTimeTextPlayer.text = ((int)(ClearTimeNow / 60)).ToString("00") + ":" + ((int)(ClearTimeNow % 60)).ToString("00");
        /*
       //「SCORE」というキーで保存されているFloat値のRankingListと同じスコアを探して
       //順番を取得,IndexOfは存在しないと-1を返す
       PlayerRank = RankingList.IndexOf(ClearScoreNow) + 1;//リストの書き方
       //あなたの順位は○○位!と表示させる、ランキング圏外の場合はスコアを表示する
       if (PlayerRank >= 1)
       {
           PlayerText.text = "あなたの順位は" + PlayerRank + "位!!";
       }
       else if (PlayerRank == 0){
           PlayerText.text = "あなたのスコアは" + ClearScoreNow + "!!";
       }*/
    }

    void Update()
    {/*
        if (StafrollOn == false)
        {//ボタンをクリックしたら、スタッフロールを表示させる
            if (Input.anyKey)
            {
                RankingAll.SetActive(false);
                Stafroll.SetActive(true);
                AllKeyUp = true;
            }
            if (!Input.anyKey&& AllKeyUp==true)
            {
                StafrollOn = true;
            }
        }
        if (StafrollOn == true)
        {//ボタンをクリックしたら、リザルトボタンを表示させる
            if (Input.anyKey)
            {
                AllResultButton.SetActive(true);
                RankingAll.SetActive(false);
                Stafroll.SetActive(false);
            }
        }*/
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
    //JSONファイルがない場合に呼び出す初期化関数
    //初期値をセーブし, JSONファイルを生成する
    public void Initialize()
    {
        jsonType.Key1 = RankingListSave;
        jsonType.ClearScoreRanking = RankingTimeSave;
        var jsonBody = JsonConvert.SerializeObject(jsonType);

        saveJsonData(jsonBody);
    }
    //JSONファイルの有無を判定するための関数
    public bool FindJsonfile()
    {//StreamingAssetsフォルダを指定. /以降にファイル名RankingScoreSave.json
        string filePath = Application.streamingAssetsPath + "/RankingScoreSave.json";
        return File.Exists(filePath);
    }
}


public class JsonType
{
    [JsonProperty("Key1")]
    public float[] Key1 { get; set; }
    [JsonProperty("ClearTime")]//クリアタイム保存用
    public float ClearTime { get; set; }
    [JsonProperty("ClearScore")]//クリアスコア保存用
    public float ClearScore { get; set; }
    [JsonProperty("ClearScoreRanking")]//クリアタイムランキング保存用
    public float[] ClearScoreRanking { get; set; }
    [JsonProperty("ClearCoin")]
    public int[] ClearCoin { get; set; }
    [JsonProperty("Language")]
    public string Language { get; set; }
}
