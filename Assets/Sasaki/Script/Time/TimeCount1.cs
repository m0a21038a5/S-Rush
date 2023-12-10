using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;  //StreamWriterなどを使うために追加
public class TimeCount1 : MonoBehaviour
{
    //経過時間を計測するスクリプト、計測した時間はRankingScoreSave.jsonにClearTimeとして保存
    //時間は文字列(string)で保存する
    public Text timeLabel;
    public float timeCount;//timeCount 経過時間
    public string timeCountstr;//timecountstr 経過時間文字列 
    public string timeCountstrrd;//切り捨て用
    public float timeCountflo;
    public GameObject Bossobj;
    public Clear ClearSclipt;
    private JsonType jsonType = new JsonType();
    public Animator ClearUIAni;
    //保存先
    string datapath;
    public bool TimeClearStop;//時間停止用bool,ゲーム開始時カウントダウンにも使用します
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
        timeLabel.text = "Time :" + timeCount;
        Bossobj = GameObject.Find("BOSS");
        ClearSclipt = Bossobj.GetComponent<Clear>();
        TimeClearStop = true;
        //JSONファイルがあればロード, なければ初期化関数へ
       
            jsonType = loadJsonData();
            Debug.Log(jsonType.Key1);
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeClearStop == false)
        {
            timeCount += Time.deltaTime;//経過時間を増やす
        }
        else if (TimeClearStop==true)//ボスを倒したらタイマーを止める
        {
            //経過時間を増やすのを止める
        }
        timeLabel.text = "Time :" + ((int)(timeCount / 60)).ToString("00") + ":" + ((int)(timeCount % 60)).ToString("00"); //timeCount.ToString("00.00"); //文字列として表示させる
        timeCountstr = ((int)(timeCount / 60)).ToString("00") + ":" + ((int)(timeCount % 60)).ToString("00"); //timeCount.ToString("00.00");//文字列として保存
        timeCountstrrd = timeCount.ToString("0.00");
        timeCountflo = float.Parse(timeCountstrrd);//floatに変換できてない
        if (ClearSclipt.isDead == true)//ボスが倒されたら
        {
            jsonType = loadJsonData();
            ClearUIAni.enabled=true;
            TimeClearStop = true;
            //RankingScoreSave.jsonを書き換える
            jsonType.ClearTime = timeCountflo;
            var jsonBody = JsonConvert.SerializeObject(jsonType);
            JsonType outPut = JsonConvert.DeserializeObject<JsonType>(jsonBody);
            Debug.Log(outPut.ClearTime + "保存その２!");
            saveJsonData(jsonBody);
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
    public void Initialize()
    {
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
