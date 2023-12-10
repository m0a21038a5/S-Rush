using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combo : MonoBehaviour
{/*コンボのスクリプトです
　コンボの仕様は
    敵に連続して攻撃すると発生する
    効果はコンボ数に応じて攻撃力強化、効果音の変化、スコア倍率の上昇、エフェクトの変化(仮)、コンボ表記の変化
    コンボが解除される条件は地面に着地したとき
    攻撃力上昇の倍率(スコア?)
    コンボ倍率
１コンボ：1.0倍
２コンボ：1.2倍
３コンボ：1.4倍
４コンボ：1.6倍
5コンボ：1.8倍
６コンボ：2.0倍
７コンボ：2.3倍
８コンボ：2.6倍
９コンボ：2.9倍
10コンボ：3.0倍
  */
    public float ComboResetTime = 5.0f;//コンボがリセットされる時間
    [SerializeField]
    [HeaderAttribute("現在のコンボ")]
    public int ComboCount = 0;//現在のコンボ
    [SerializeField]
    [HeaderAttribute("現在のコンボ倍率")]
    public float ComboAttackCurrentMagnification;//現在のコンボ倍率
    [SerializeField]
    [HeaderAttribute("最大のコンボ数")]
    public int ComboAttackMaxMagnification = 10;//最大のコンボ倍率
    [SerializeField]
    [HeaderAttribute("入力をしないでコンボが途切れる秒数")]
    public float ChainUnlocksec = 5.0f;//プレイヤーが入力をしないで途切れる秒数
    [SerializeField]
    [HeaderAttribute("コンボの攻撃力上昇倍率")]
    public float[] ComboAttackMagnification ;//コンボの攻撃力上昇倍率
    [SerializeField]
    [HeaderAttribute("必殺技の攻撃力上昇倍率")]
    public float SpecialAttackMagnification;//必殺技の攻撃力上昇倍率

    [SerializeField]
    [HeaderAttribute("コンボゲージ")]
    private Slider Gage_ChainGage;
    [SerializeField]
    private GameObject Obj_ChainGage;


    //Comboを敵に当たった時の一回のみ追加されるようにするための変数
    public int CountEnemyCombo;

    //targetから敵のオブジェクトを取得
    target t;

    //コンボのテキスト
    public Text ComboNumberText;
    public Text ComboText;

    //必殺技用の変数
    public bool Special;
    public bool isPushed;

    //コンボ解除
    public bool ChainUnlock;
    //コンボ解除の時間
    public float ChainUnlocktime;

    //複数ターゲット
    multiplePlayer mp;

    public bool a;

    public bool isDeathblow = false;

    public bool isFloor;
    public bool isCombo;
    public bool SpecialMode;

    int EnemyCount;

    [SerializeField] GameObject UIParent;
    [SerializeField] Image SpecialGauge;

    void Start()
    {
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();
        ComboText.enabled = false;
        ComboNumberText.enabled = false;
        ChainUnlock = false;
        ChainUnlocktime = 3.0f;
        //ChainUnlocksec秒ごとにChainを0にする処理を一定間隔で呼び出す
        // InvokeRepeating(nameof(ChainUnlock0), ChainUnlocksec, ChainUnlocksec);

        mp = GameObject.FindGameObjectWithTag("Player").GetComponent<multiplePlayer>();
        Special = false;
        isPushed = false;
        a = false;
        EnemyCount = 0;

        Obj_ChainGage.SetActive(true);
        SpecialGauge.fillAmount = 0.1f;
    }

    void LateUpdate()
    {
        ChainUnlocktime += Time.deltaTime;

        Gage_ChainGage.value = (ChainUnlocksec - ChainUnlocktime) / ChainUnlocksec;
        //SpecialGauge.fillAmount = (ChainUnlocksec - ChainUnlocktime) / ChainUnlocksec;

        if (ComboCount == 0 || SpecialMode)
        {
            ComboAttackCurrentMagnification = ComboAttackMagnification[0];
            ComboText.enabled = false;
            ComboNumberText.enabled = false;
            isCombo = false;
        }
      
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown("joystick button 7") || Input.GetKeyDown("joystick button 0") || mp.At || Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.LeftShift)) && (t.ismove_Statue || t.ismove_Beam || t.ismove_Boss))
        {
            //ChainUnlock = false;       敵に当たった場所に移行（OnCollisionEnter）
            //ChainUnlocktime = 0.0f;
            //CountEnemyCombo = 0;
            // StartCoroutine(ChainUnlock1());
        }
        if (ChainUnlock == true)
        {
            ComboCount = 0;
            ComboNumberText.GetComponent<Text>().text = ComboCount.ToString();
        }
        if (ChainUnlock == false)
        {
            //Debug.Log("Chain継続中!");
            isCombo = true;
        }
        if(ChainUnlocktime >= ChainUnlocksec || a)
        {
            ChainUnlock = true;
            //ChainUnlocktime = 0.0f;
            a = false;
            //EnemyCount = 0;
            //SpecialGauge.fillAmount = 0;
            //Special = false;
        }

        if (t.SpecialAttack)
        {
            isDeathblow = true;
            //ComboAttackCurrentMagnification = SpecialAttackMagnification;
            SpecialGauge.fillAmount = 0;
        }
        else
        {
            isDeathblow = false;
            //ComboAttackCurrentMagnification = ComboAttackMagnification[ComboCount - 1];
        }

        if(t.SpecialAtStart)
        {
            EnemyCount = 0;
        }

        /*
        if(isPushed)
        {
            ComboAttackCurrentMagnification = SpecialAttackMagnification;
            isDeathblow = true;
        }
        */
        if(EnemyCount >= 10)
        {
            Special = true;
        }
        else
        {
            Special = false;
        }

        if (SpecialMode || GameObject.FindGameObjectWithTag("Manager").GetComponent<multipleTarget>().ChangeCamera)
        {
            UIParent.SetActive(false);
        }
        else
        {
            UIParent.SetActive(true);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {//タグBeamかタグStatueの付いた敵に当たった時、ComboProcessを実行
        //if ((collision.gameObject == t.TargetBeam || collision.gameObject == t.TargetStatue) && Count == 0)
        if ((collision.gameObject.CompareTag("Beam") || collision.gameObject.CompareTag("Statue") || collision.gameObject.CompareTag("BOSS")) && (t.Attack || mp.At))
        {
            // 通常攻撃でチェインがつながるかどうか
            bool isChain = true;
            if (collision.gameObject.CompareTag("Beam"))
            {
                if (collision.gameObject.GetComponent<BeamHPManager>().DamagedValue <= 0.0f)
                {
                    isChain = false;
                }
            }

            // 攻撃が効く場合のみ処理
            if (isChain || t.SpecialAttack)
            {
                ComboProcess();
                ComboText.enabled = true;
                ComboNumberText.enabled = true;
                //CountEnemyCombo++;
                ChainUnlock = false;
                ChainUnlocktime = 0.0f;
                //CountEnemyCombo = 0;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isFloor = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Beam") || collision.gameObject.CompareTag("Statue") || collision.gameObject.CompareTag("BOSS"))
        {
            //CountEnemyCombo = 0;
            //Debug.Log("Out");
        }

        if (collision.gameObject.tag == "Floor")
        {
            isFloor = false;
        }
    }

    
    public void ComboProcess()
    {//コンボの処理
        //ComboCountを1増やす
      
        ComboCount++;
        Gage_ChainGage.value = 1;
        EnemyCount++;
        if (EnemyCount <= 9)
        {
            SpecialGauge.fillAmount += 0.06f;
        }
        else
        {
            SpecialGauge.fillAmount += 1 - SpecialGauge.fillAmount;
        }


        //テキストを現在のコンボにする(要変更)
        ComboNumberText.GetComponent<Text>().text = ComboCount.ToString();
        ComboText.GetComponent<Text>().text = "Chain";
        //最大コンボより小さい時、現在のコンボに応じた倍率を現在のコンボ倍率であるComboAttackCurrentMagnificationに入れる
        //最大コンボ以上になったら倍率を最大の倍率に固定
        if (ComboCount <= ComboAttackMaxMagnification)
        {
            ComboAttackCurrentMagnification = ComboAttackMagnification[ComboCount-1];
        }else if(ComboCount > ComboAttackMaxMagnification && !t.SpecialAttack)
        {
            ComboAttackCurrentMagnification = ComboAttackMagnification[ComboAttackMaxMagnification-1];
        }
    }
    void ChainUnlock0()
    {
        ChainUnlock = true;
        Debug.Log("Chainを0にする!");
    }

   

    /*IEnumerator ChainUnlock1()
    {
        yield return new WaitForSecondsRealtime(ChainUnlocksec);
        ChainUnlock = true;
        Debug.Log("Chainを0にする!");

    }
    private void OnDestroy()
    {
        // Destroy時に登録したInvokeをすべてキャンセル
        CancelInvoke();
    }*/
}
