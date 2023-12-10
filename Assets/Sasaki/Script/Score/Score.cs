
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [HeaderAttribute("現在のスコア")]
    public float ScoreCount = 0;//現在のスコア
    [SerializeField]
    [HeaderAttribute("地上敵を倒した時に貰えるスコア")]
    public float StatueEnemyScore; //地上敵を倒した時、1コンボの状態で貰えるスコア
    [SerializeField]
    [HeaderAttribute("ビーム敵を倒した時に貰えるスコア")]
    public float BeamEnemyScore; //ビーム敵を倒した時、1コンボの状態で貰えるスコア
    [SerializeField]
    [HeaderAttribute("ボスを倒した時に貰えるスコア")]
    public float BossEnemyScore; //ボスを倒した時、1コンボの状態で貰えるスコア
    [SerializeField]
    [HeaderAttribute("コンボのスコア上昇倍率")]
    public float[] ComboScoreMultiplier; //コンボのスコア上昇倍率
    //スコアのテキスト
    public Text ScoreText;
    //コンボの取得
    [HeaderAttribute("現在のコンボ")]
    public int ComboScoreCount = 0;//現在のコンボ
                                   //プレイヤー取得
    private GameObject PlayerCombo;
    //スクリプト取得
    Combo comboScript;
    //targetスクリプト取得
    target t;

    //最大スコア判定
    public bool MaxScoreMultiplier;
    //最大スコア
    public int CAMM;




    void Start()
    {
        PlayerCombo = GameObject.Find("Player");
        comboScript = PlayerCombo.GetComponent<Combo>();
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();
        CAMM = comboScript.ComboAttackMaxMagnification;
       

    }

    void Update()
    {
        ScoreText.GetComponent<Text>().text = "Score:" + ScoreCount;
       


        if (comboScript.SpecialMode)
        {
            ScoreText.GetComponent<Text>().enabled = false;
        }
        else
        {
            ScoreText.GetComponent<Text>().enabled = true;
        }
    }



    public void OnCollisionEnter(Collision collision)
    {
        //タグStatueの付いた敵に当たった時、スコアを増やすを実行
        if (collision.gameObject.tag == "Statue" && t.Attack)
        {
            if (comboScript.ComboCount < (CAMM - 1))
            {
                ScoreCount = ScoreCount + StatueEnemyScore * ComboScoreMultiplier[comboScript.ComboCount];
                //「SCORE」というキーで、Int値の「 ScoreCount 」を保存
                PlayerPrefs.SetFloat("SCORE", ScoreCount);
                PlayerPrefs.Save();
            }
            else if (comboScript.ComboCount >= (CAMM - 1))
            {
                ScoreCount = ScoreCount + StatueEnemyScore * ComboScoreMultiplier[CAMM - 1];
                //「SCORE」というキーで、Int値の「 ScoreCount 」を保存
                PlayerPrefs.SetFloat("SCORE", ScoreCount);
                PlayerPrefs.Save();
            }
           
        }
        //タグBeamの付いた敵に当たった時、スコアを増やすを実行
        if (collision.gameObject.tag == "Beam" && t.Attack)
        {
            if (comboScript.ComboCount < (CAMM - 1))
            {
                ScoreCount = ScoreCount + StatueEnemyScore * ComboScoreMultiplier[comboScript.ComboCount];
                //「SCORE」というキーで、Int値の「 ScoreCount 」を保存
                PlayerPrefs.SetFloat("SCORE", ScoreCount);
                PlayerPrefs.Save();
            }
            else if (comboScript.ComboCount >= (CAMM - 1))
            {
                ScoreCount = ScoreCount + StatueEnemyScore * ComboScoreMultiplier[(CAMM - 1)];
                //「SCORE」というキーで、Int値の「 ScoreCount 」を保存
                PlayerPrefs.SetFloat("SCORE", ScoreCount);
                PlayerPrefs.Save();
            }
           
        }

        if (collision.gameObject.tag == "BOSS" && t.Attack)
        {
            if (comboScript.ComboCount < (CAMM - 1))
            {
                ScoreCount = ScoreCount + StatueEnemyScore * ComboScoreMultiplier[comboScript.ComboCount];
                //「SCORE」というキーで、Int値の「 ScoreCount 」を保存
                PlayerPrefs.SetFloat("SCORE", ScoreCount);
                PlayerPrefs.Save();
            }
            else if (comboScript.ComboCount >= (CAMM - 1))
            {
                ScoreCount = ScoreCount + StatueEnemyScore * ComboScoreMultiplier[CAMM - 1];
                //「SCORE」というキーで、Int値の「 ScoreCount 」を保存
                PlayerPrefs.SetFloat("SCORE", ScoreCount);
                PlayerPrefs.Save();
            }
           
        }
    }
}
