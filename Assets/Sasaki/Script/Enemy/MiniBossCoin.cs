using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;  //StreamWriterなどを使うために追加

public class MiniBossCoin : MonoBehaviour
{//中ボスを倒すとコインが手に入る
    [HeaderAttribute("中ボスの順番")]
    public int MiniBossNumber;
    public int[] MiniBossAllcoin;
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
        MiniBossAllcoin = jsonType.ClearCoin;
    }

    public void getCoin()
    {
        jsonType = loadJsonData();
        MiniBossAllcoin = jsonType.ClearCoin;
        MiniBossAllcoin[MiniBossNumber] = 1;
        jsonType.ClearCoin = MiniBossAllcoin;//jsonのほうに反映　これ無しでいままでどうやって保存してたんだ?
        var jsonBody = JsonConvert.SerializeObject(jsonType);
        JsonType outPut = JsonConvert.DeserializeObject<JsonType>(jsonBody);
        saveJsonData(jsonBody);
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
}
