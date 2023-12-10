using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CriWare;
using CriWare.Assets;

public class Soundtest : MonoBehaviour
{
    public GameObject BGMPlayer;
    public CriAtomAcfAsset acf;
    public CriAtomAcbAsset acb;
    public CriAtomExPlayer player;
    private CriAtomExPlayback playback;//プレイバック
    // Start is called before the first frame update
    [SerializeField]
    public PlayerSounds p;
    GameManager gm;
    public GameObject menuObj;
    public GameObject menuObj2;
    public GameObject menuObj3;
    public GameObject menuObj4;
    public Slider volumeSlider;
    public float playerVol;
    public GameObject BGMObj;
    BGMPlayer Code_bgmplayer;


    void Start()
    { 
        DontDestroyOnLoad(this.gameObject);
        acf.Register();
        CriAtomEx.AttachDspBusSetting("DspBusSetting_0");
        acb.LoadImmediate();
        player = new CriAtomExPlayer();
        volumeSlider.value = volumeSlider.maxValue = 1.0f;
        volumeSlider.minValue = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (p.isPlayWalkLSound == true)//左足音
        {
            player.SetCue(acb.Handle, "SE_FootSteps_Left");
            player.Start();
            p.isPlayWalkLSound = false;
        }
        if (p.isPlayWalkRSound == true)//右足音
        {
            player.SetCue(acb.Handle, "SE_FootSteps_Right");
            player.Start();
            p.isPlayWalkRSound = false;
        }
        if (p.isPlayRushSound == true)//突進風切り音
        {
            player.SetCue(acb.Handle, "SE_PlayerRush");
            player.Start();
            p.isPlayRushSound = false;
        }
        if (p.isPlayAttackSound == true)//攻撃音
        {
            player.SetCue(acb.Handle, "");
            player.Start();
            p.isPlayAttackSound = false;
        }
        if (p.isPlayAttackHitSound == true)//攻撃当たった音
        {
            player.SetCue(acb.Handle, "SE_PlayerAttack");
            player.Start();
            p.isPlayAttackHitSound = false;
        }
        if (p.isPlayDamageSound)//ダメージ
        {
            player.SetCue(acb.Handle, "SE_Damaged");
            player.Start();
            p.isPlayDamageSound = false;
        }
        if (p.isPlayJumpSound == true)//ジャンプ
        {
            player.SetCue(acb.Handle, "SE_Jump");
            player.Start();
            p.isPlayJumpSound = false;
        }
        if (p.isPlayFallSound == true)//穴に落ちる
        {
            player.SetCue(acb.Handle, "SE_FallToHall");
            player.Start();
            p.isPlayFallSound = false;
        }
        if (p.isPlayFallWaterSound == true)//水に落ちる
        {
            player.SetCue(acb.Handle, "SE_FallToWater");
            player.Start();
            p.isPlayFallWaterSound = false;
        }

        /*
        if(menuObj.activeSelf == false)//メニュー画面開いていない時 [ GameManagerに移動しました by村岡 ]
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                positive1Player();              
            }
            BGMObj.GetComponent<BGMPlayer>().aisac2 = 0.0f;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                negative1Player();               
            }
            BGMObj.GetComponent<BGMPlayer>().aisac2 = 1.0f;
        }*/

        playerVol = volumeSlider.value;
        SetVolume(playerVol);
        
    }
    public void positive1Player()
    {
        player.SetCue(acb.Handle, "positive1");
        player.Start();
    }
    public void positive2Player()
    {
        player.SetCue(acb.Handle, "positive2");
        player.Start();
    }
    public void negative1Player()
    {
        player.SetCue(acb.Handle, "negative1");
        player.Start();
    }
    public void negative2Player()
    {
        player.SetCue(acb.Handle, "negative2");
        player.Start();
    }
    public void SE_ButtonPlayer()
    {
        player.SetCue(acb.Handle, "SE_Button");
        player.Start();
    }
    public void SE_MenuOpenPlayer()
    {
        player.SetCue(acb.Handle, "SE_MenuOpen");
        player.Start();
    }
    public void SE_MenuClosePlayer()
    {
        player.SetCue(acb.Handle, "SE_MenuClose");
        player.Start();
    }
    public void SE_PsoundPlayer()
    {
        player.SetCue(acb.Handle, "SE_Psound");
        player.Start();
    }

    public void SE_CantAttackPlayer() //攻撃が弾かれる音
    {
        player.SetCue(acb.Handle, "SE_CantAttack");
        player.Start();
    }

    public void SE_SPButtonPlayer()//必殺技ボタンの音
    {
        player.SetCue(acb.Handle, "SE_SPButton");
        player.Start();
    }

    public void SE_SuperAttackPlayer()//必殺技が当たる音
    {
        player.SetCue(acb.Handle, "SE_SuperAttack");
        player.Start();
    }

    public void SE_getCoinPlayer()//コイン取得の音
    {
        player.SetCue(acb.Handle, "SE_GetCoin");
        player.Start();
    }

    public void SE_TargetLockedPlayer()//ターゲットの音
    {
        player.SetCue(acb.Handle, "SE_TargetLocked");
        player.Start();
    }
    public void SE_EnemyDead1Player()//電撃敵が死んだときの音
    {
        player.SetCue(acb.Handle, "SE_EnemyDead1");
        player.Start();
    }

    public void SE_InSPPlayer()
    {
        player.SetCue(acb.Handle, "SE_InSP");
        player.Start();
    }

    public void SE_PlayerAttackPlayer()//武田用テスト攻撃音
    {
        player.SetCue(acb.Handle, "SE_PlayerAttack");
        player.Start();
    }

    public void SE_PlayerAttack2Player()//前の攻撃音
    {
        player.SetCue(acb.Handle, "SE_PlayerAttack2");
        player.Start();
    }

    public void result1Player()//リザルト画面の効果音①
    {
        player.SetCue(acb.Handle, "result1");
        player.Start();
    }
    public void result2Player()//リザルト画面の効果音②
    {
        player.SetCue(acb.Handle, "result2");
        player.Start();
    }
    public void kazePlayer()
    {
        player.SetCue(acb.Handle, "SE_PlayerRush");
        player.Start();
    }
    public void SE_SPChargePlayer()
    {
        player.SetCue(acb.Handle, "SE_SPCharge");
        player.Start();
    }
    public void SE_321Player()
    {
        player.SetCue(acb.Handle, "SE_321");
        player.Start();
    }

    public void SE_HeartbeatPlayer()
    {
        player.SetCue(acb.Handle, "SE_Heartbeat");
        playback = player.Start();
    }

    public void SE_HeartbeatStopper()
    {
        playback.Stop();
    }
    public void SetVolume(float vol)
    {
        /* (19) ボリュームの設定 */
        player.SetVolume(vol);

        /* (20) パラメーターの更新 */
        player.UpdateAll();
    }
}
