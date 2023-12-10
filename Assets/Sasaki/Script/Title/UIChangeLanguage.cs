using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;  //StreamWriterなどを使うために追加

public class UIChangeLanguage : MonoBehaviour
{
    //UIの言語を変更させる
    public Text[] JapaneseTexts;//日本語テキスト
    public Text[] EnglishTexts;//英語版テキスト
    public string CurrentLanguage;//現在選択している言語
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
    }

    void Update()
    {
        if (CurrentLanguage == "Japanese")
        {
            for (int i = 0; i < JapaneseTexts.Length; i++)
            {
                JapaneseTexts[i].enabled = true;
                EnglishTexts[i].enabled = false;
            }
        }
        else if (CurrentLanguage == "English")
        {
            for (int i = 0; i < EnglishTexts.Length; i++)
            {
                JapaneseTexts[i].enabled = false;
                EnglishTexts[i].enabled = true;
            }
        }
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
