using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatueHPManager : MonoBehaviour
{
    //HPのprefab
    public GameObject StatueHP;
    //ダメージの数値
    public float DamageValue;
    //最大HP
    [SerializeField]
    public float MaxHP;
    //HP表示座標関連
    public float height;
    //現在のHP
    public float HP;
    private Slider slider;
    public GameObject hp;
    //HP表示用Canvas
    private Canvas canvas;

    GameObject player;
    target t;
    Combo c;

    multiplePlayer mp;

    private bool Damage;

    //春日追記
    [SerializeField]
    public GameObject hit;
    [SerializeField]
    public GameObject Deathblow;
    Combo Combo;
    public float EffectHeight = 0;

    //防御力
    public float DefencePower;

    // 村岡追加
    private PlayerSounds ps;
    private Soundtest st;

    float DamagedValue;

    public bool Boss;
    bool isDead; 
    public bool isDefeat;

    GameObject Zone;

    public GameObject cai;//コアのUIの表示 佐々木追加

    float ComboAttackMagnification;

    bool CanCollide;

    single s;

    //中ボスもしくは、中ボス周辺に存在している場合は0以外
    public int BossCount = 0;

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        //春日追記
        Combo = GameObject.FindGameObjectWithTag("Player").GetComponent<Combo>();

        //HP表示
        canvas = GameObject.FindGameObjectWithTag("HP").GetComponent<Canvas>();
        while (hp == null)
        {
            hp = Instantiate(StatueHP, canvas.transform);
        }
        slider = hp.GetComponent<Slider>();
        slider.maxValue = MaxHP;
        slider.value = slider.maxValue;
        HP = MaxHP;
        hp.SetActive(false);
        Damage = false;

        player = GameObject.FindGameObjectWithTag("Player");
        t = player.GetComponent<target>();
        c = player.GetComponent<Combo>();

        mp = GameObject.FindGameObjectWithTag("Player").GetComponent<multiplePlayer>();

        s = GameObject.FindGameObjectWithTag("Manager").GetComponent<single>();

        //佐々木追加　コアのUI
        cai = GameObject.Find("CoreAllUI");

        gm = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();

        // 村岡追加 効果音関係
        ps = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSounds>();
        st = GameObject.Find("SEPlayer").GetComponent<Soundtest>();// ここは一番下の行にしてください
    }

    // Update is called once per frame
    void Update()
    {
        //ダメージ量
        if (t.SpecialAttack)
        {
            ComboAttackMagnification = c.SpecialAttackMagnification;
        }
        else
        {
            ComboAttackMagnification = c.ComboAttackCurrentMagnification;
        }

        if (DamageValue * ComboAttackMagnification - DefencePower <= 0)
        {
            DamagedValue = 0;
        }
        else
        {
            DamagedValue = DamageValue * ComboAttackMagnification - DefencePower;
        }
        //ダメージを受けた時
        if (Damage == true)
        {
            HP = HP - DamagedValue;
            slider.value = HP;
            Damage = false;
            t.Attack = false;
            t.ismove_Statue = false;
            // 村岡追加
            ps.isPlayAttackHitSound = true;
            if (HP > 0)
            {
                st.SE_CantAttackPlayer();
            }
            /*
            else
            {
                ps.isPlayAttackHitSound = true;

                if (GetComponent<MiniBossCoinStatueEnemy>() != null)
                {
                    GetComponent<MiniBossCoinStatueEnemy>().getCoin();
                    st.SE_getCoinPlayer();
                }
            }
            */
        }
       
        if(HP <= 0)
        {
            StartCoroutine(DeadCoroutine());

            if (GetComponent<MiniBossCoinStatueEnemy>() != null)
            {
                GetComponent<MiniBossCoinStatueEnemy>().getCoin();
                cai.GetComponent<CoreAllImage>().coreUIsave();
                st.SE_getCoinPlayer();
            }
        }

        Vector3 pos = new Vector3(t.StatuePos2.x, t.StatuePos2.y + height, t.StatuePos2.z);
        Vector2 position = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
        hp.transform.position = new Vector3(position.x, position.y, 0);

        if(t.isTarget_Statue == true && t.TargetStatue == this.gameObject)
        {
            hp.SetActive(true);
        }
        else
        {
            hp.SetActive(false);
        }

        if(DamagedValue >= HP)
        {
            isDead = true;
        }
        else
        {
            isDead = false;
        }


     
    }

 

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && (t.Attack || mp.At || t.SpecialAttack))
        {
            if (CanCollide)
                return;
            //StartCoroutine(DamageCoroutine());
            Damage = true;
            //春日追記
            if (Combo.isDeathblow == true)
            {
                var obj = Instantiate(Deathblow);
                obj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + EffectHeight, this.transform.position.z);
            }
            else
            {
                var obj = Instantiate(hit);
                obj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + EffectHeight, this.transform.position.z);
            }

            if (DamagedValue <= 0 && !mp.At)
            {
                t.DenfensiveKnockBack();
                CanCollide = true;
                StartCoroutine(EnableCollisionAfterDelay(0.5f));
                Boss = true;
                s.targetList.Remove(this.gameObject);
                s.SortVectorList();
            }

            if(isDead)
            {
                HP = 0;
                isDefeat = true;
            }

            if (c.ComboAttackCurrentMagnification >= 10.0f)
            {
                st.SE_CantAttackPlayer();
            }
        }
        if (collision.gameObject.tag == "DangerZone")
        {
            Zone = collision.gameObject;
        }
    }

    private IEnumerator DeadCoroutine()
    {
        Destroy(hp);
        GetComponent<CapsuleCollider>().enabled = false;
        t.isTarget_Statue = false;
        t.TargetStatue = null;
        t.Attack = false;

        yield return null;

        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        //中ボス以外
        if (!this.gameObject.name.Contains("Variant") && !Boss)
        {
            EnemyManager em = GameObject.FindGameObjectWithTag("Manager").GetComponent<EnemyManager>();
            em.AddStatueData(this.gameObject);
            em.RequestsRespawn(gameObject);
        }
        if (Zone != null)
        {
            if (this.gameObject.name.Contains("Variant"))
            {
                Zone.GetComponent<DangerArea>().Boss = null;
                if (BossCount == 1) gm.Boss1 = true;
                else if (BossCount == 2) gm.Boss2 = true;
                else if (BossCount == 3) gm.Boss3 = true;
            }
            else
            {
                Zone.GetComponent<DangerArea>().DangerList.Remove(this.gameObject);
            }
        }
        targetChange tc = GameObject.FindGameObjectWithTag("Manager").GetComponent<targetChange>();
        tc.right.Clear();
        tc.left.Clear();
        single s = GameObject.FindGameObjectWithTag("Manager").GetComponent<single>();
        s.targetList.Clear();
    }

    private IEnumerator EnableCollisionAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        CanCollide = false;
        Boss = false;
    }
}
