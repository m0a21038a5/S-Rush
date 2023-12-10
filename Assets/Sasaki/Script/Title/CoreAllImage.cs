using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;  //StreamWriterなどを使うために追加

public class CoreAllImage : MonoBehaviour
{
    public Image[] CoinAllImage;
    public Image[] CoinDottLineAllImage;
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

    void Update()
    {

    }
    //コアのUI表示
    public void coreUIsave()
    {
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
