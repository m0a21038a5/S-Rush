using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScore : MonoBehaviour
{
    public Text StageClearText;
    public GameObject AllRankingText;
    public GameObject AllResultButton;
    void Start()
    {
        //「SCORE」というキーで保存されているFloat値を読み込み
        float ClearScore = PlayerPrefs.GetFloat("SCORE");
        StageClearText.text = "あなたのスコアは・・・" + ClearScore + "!!";
    }

    void Update()
    {
        if (Input.anyKey)
        {//ボタンをクリックしたら、ランキングを表示させる
            StageClearText.enabled = false;
            AllRankingText.SetActive(true);
        }
    }
}
