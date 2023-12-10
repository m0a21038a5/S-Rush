using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;  //StreamWriterなどを使うために追加


public class TitleUIButton : MonoBehaviour
{
    //タイトルのUI用スクリプト
    //設定ボタン起動と言語変更
    public GameObject ConfigAll;//設定ボタンすべて
    public GameObject FirstTitleButton;//最初に表示されるボタン
    public GameObject StuffAll;//スタッフクレジット全体
    public GameObject LanguageAll;//言語設定全体
    public Button FirstSelectButton;//最初に選択されるボタン
    public Button ConfigSelectButton;//設定画面で選択されるボタン
    public Button LangageButton;//言語選択画面で選択されるボタン
    public Button StaffCloseButton;//スタッフクレジットで選択されるボタン
    public GameObject[] JapaneseObjects;//日本語テキスト
    public Text[] JapaneseTexts;//日本語テキスト
    public GameObject[] EnglishObjects;//英語版テキスト
    public Text[] EnglishTexts;//英語版テキスト
    private bool StuffPanelClose;//スタッフクレジットが表示中か判定
    private bool StuffPanel;//スタッフクレジットが表示中か判定
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
        //AcquisitionEnglish();
        //AcquistionJapanese();
        jsonType = loadJsonData();
        CurrentLanguage = jsonType.Language;
    }
    void Update()
    {
        if(CurrentLanguage== "Japanese")
        {
            for (int i = 0; i < JapaneseObjects.Length; i++)
            {
                JapaneseTexts[i].enabled = true;
                EnglishTexts[i].enabled = false;
            }
        }
        else if (CurrentLanguage == "English")
        {
            for (int i = 0; i < EnglishObjects.Length; i++)
            {
                JapaneseTexts[i].enabled = false;
                EnglishTexts[i].enabled = true;
            }
        }
        
    }
    //設定ボタンをクリックしたときの処理
    public void OnClickConfigButton() 
    {
        FirstTitleButton.SetActive(false);
        LanguageAll.SetActive(false);
        ConfigAll.SetActive(true);
        ConfigSelectButton.Select();
    }
    //言語設定ボタンをクリックしたときの設定
    public void OnClickLanguageButton()
    {
        ConfigAll.SetActive(false);
        LanguageAll.SetActive(true);
        LangageButton.Select();
    }
    //開発者ボタンをクリックしたときの処理
    public void OnClickStuffButton()
    {
        StuffPanel = true;
        ConfigAll.SetActive(false);
        StuffAll.SetActive(true);
        StaffCloseButton.Select();
    }
    //戻るボタンを押したときの処理
    public void OnClickReturnButton()
    {
        ConfigAll.SetActive(false);
        FirstTitleButton.SetActive(true);
        FirstSelectButton.Select();
    }
    //スタッフクレジットを閉じる処理
    public void CloseStuff()
    {
        StuffAll.SetActive(false);
        ConfigAll.SetActive(true);
        StuffPanel = false;
        StuffPanelClose = false;
        ConfigSelectButton.Select();
    }
    //日本語に変更したときの処理
    public void ChangeLanguageJapanese()
    {
        jsonType = loadJsonData();
        CurrentLanguage = "Japanese";
        jsonType.Language = CurrentLanguage;
        var jsonBody = JsonConvert.SerializeObject(jsonType);
        JsonType outPut = JsonConvert.DeserializeObject<JsonType>(jsonBody);
        saveJsonData(jsonBody);
        for (int i = 0; i < JapaneseObjects.Length; i++)
        {
            JapaneseTexts[i].enabled = true;
            EnglishTexts[i].enabled = false;
        }
    }
    //英語に変更したときの処理
    public void ChangeLanguageEnglish()
    {
        jsonType = loadJsonData();
        CurrentLanguage = "English";
        jsonType.Language = CurrentLanguage;
        var jsonBody = JsonConvert.SerializeObject(jsonType);
        JsonType outPut = JsonConvert.DeserializeObject<JsonType>(jsonBody);
        saveJsonData(jsonBody);
    }
    //TextJapaneseタグのついたテキストを取得 取得できない? 時間がないので手動で
    void AcquistionJapanese()
    {
        JapaneseObjects = GameObject.FindGameObjectsWithTag("TextJapanese");
        for (int i = 0; i < JapaneseObjects.Length; i++)
        {
            JapaneseTexts[i] = JapaneseObjects[i].GetComponent<Text>();
        }
        
    }
    //TextJapaneseタグのついたテキストを取得
    void AcquisitionEnglish()
    {
        EnglishObjects = GameObject.FindGameObjectsWithTag("TextEnglish");
        for (int i = 0; i < EnglishObjects.Length; i++)
        {
            EnglishTexts[i] = EnglishObjects[i].GetComponent<Text>();
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
