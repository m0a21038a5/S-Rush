using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriWare;
using CriWare.Assets;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BGMPlayer : MonoBehaviour
{
    public CriAtomAcfAsset acf;//acfアセット
    public CriAtomAcbAsset acb;//acbアセット
    public CriAtomExPlayer BGM_Player;//BGM再生プレイヤー
    private CriAtomExPlayback playback;//プレイバック
    private CriAtomExPlayback playback2;//プレイバック2

    public Slider volumeSlider;
    public float BGM_PlayerVol;

    public PlayerSounds p;

    public bool BGM_Initialization;

    [SerializeField]
    public int ChainNumber;

    [SerializeField]
    public float aisac4;

    bool PlayerDead = false;

    public float aisac1 = 0.0f;
    public float aisac2;
    public float aisac3;

    [SerializeField]
    private GameObject PlayerObj;
    private float PlayerPositionZ;
    public bool isLoaded
    {
        get
        {
            /* 各データがロード済みかどうかをチェック | 論理和*/
            bool value = acfIsRegistered;

            value |= acb.Loaded;

            return value;
        }
    }

    private bool acfIsRegistered = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        /* ライブラリの初期化済みチェック !は∼でなかったならという意味*/
        while (!CriWareInitializer.IsInitialized())
        {
            yield return null;
            //こうやって書いておけば、ほかの処理も走る(IEnumerator)←こいつも必要,書かなかったら、ずっとここが繰り返される

        }

        BGM_Initialization = false;
        DontDestroyOnLoad(this.gameObject);
        acf.Register();
        CriAtomEx.AttachDspBusSetting("DspBusSetting_0");
        acb.LoadImmediate();
        BGM_Player = new CriAtomExPlayer();
        volumeSlider.value = volumeSlider.maxValue = 1.0f;
        volumeSlider.minValue = 0.0f;

        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (!BGM_Initialization)
        {
            BGM_Player.SetCue(acb.Handle, "BGM_Dungeon");
            playback = BGM_Player.Start();
            BGM_Player.SetCue(acb.Handle, "BGM_Atmos");
            playback2 = BGM_Player.Start();
            aisac3 = 1.0f;
            BGM_Initialization = true;
        }

        if(PlayerObj == null)  //階段のインタラクティブミュージック
        {
            PlayerObj = GameObject.Find("Player");
        }
        PlayerPositionZ = PlayerObj.transform.position.z;
        if (-400f < PlayerPositionZ && PlayerPositionZ < -200f)
        {
            aisac3 = (PlayerPositionZ / 200f) + 2f;
        }
        else if(PlayerPositionZ > -100)
        {
            aisac3 = 1.0f;
        }

        SetAisacControl3(aisac3);

        if (p.isPlayDeadSound == true) //プレイヤーが死んだらBGMを止めてジングルを流す処理。
        {
            BGM_Player.SetCue(acb.Handle, "JINGLE_PlayerDead");
            BGM_Player.Start();
            p.isPlayDeadSound = false;
        }
        if (ChainNumber > 9)   //10チェーン以上でBGMを小さくする処理
        {           
            if(aisac1 < 1.0f)
            {
                aisac1 += Time.deltaTime / 2.0f;
            }
        }
        else
        {
            aisac1 = 0.0f;
        }
        SetAisacControl(aisac1);
        SetAisacControl2(aisac2);
        SetAisacControl4(aisac4);

        BGM_PlayerVol = volumeSlider.value;
        SetVolume(BGM_PlayerVol);
    }
    void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
    {
        Debug.Log(prevScene.name);
    }
    public void SetVolume(float vol)
    {
        /* (19) ボリュームの設定 */
        BGM_Player.SetVolume(vol);

        /* (20) パラメーターの更新 */
        BGM_Player.UpdateAll();
    }

    public void JINGLE_ClearPlayer()
    {
        BGM_Player.SetCue(acb.Handle, "JINGLE_Clear");
        BGM_Player.Start();
    }
    public void SetAisacControl(float value)
    {
        BGM_Player.SetAisacControl("AisacControl_00", value);
        BGM_Player.UpdateAll();
    }
    public void SetAisacControl2(float value)
    {
        BGM_Player.SetAisacControl("AisacControl_01", value);
        BGM_Player.UpdateAll();
    }
    public void SetAisacControl3(float value)
    {
        BGM_Player.SetAisacControl("AisacControl_02", value);
        BGM_Player.UpdateAll();
    }
    public void SetAisacControl4(float value)
    {
        BGM_Player.SetAisacControl("AisacControl_03", value);
        BGM_Player.UpdateAll();
    }
}

