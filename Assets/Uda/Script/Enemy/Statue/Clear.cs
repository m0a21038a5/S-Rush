using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Clear : MonoBehaviour
{
    public GameObject BossSlider;
    public float DamageValue;
    [SerializeField]
    public float MaxHP;
    public float height;
    private float HP;
    private Slider slider;
    private GameObject hp;
    private Canvas canvas;
    public bool isDead;

    // BOSSを倒した瞬間にシーン移動しないようにします。新しい変数を追加
    private bool isDefeated = false;

    // UIテキストを表示するための変数
    public Text messageText;
    public GameObject Cm;

    GameObject player;
    [SerializeField]target t;
    [SerializeField]move1_ver2 m;
    TPCamera tc;
    Combo c;
    private bool Damage;
    Rigidbody rb;
    Vector3 Move;
    single s;
    multiplePlayer mp;

    //防御力
    public float DefencePower;

    // 村岡追加
    private PlayerSounds ps;
    private Soundtest st;
    private BGMPlayer bp;

    float DamagedValue;

    bool CanCollide;

    float ComboAttackMagnification;

 
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("HP").GetComponent<Canvas>();
        hp = Instantiate(BossSlider, canvas.transform);
        slider = hp.GetComponent<Slider>();
        slider.maxValue = MaxHP;
        HP = MaxHP;
        slider.value = slider.maxValue;
        hp.SetActive(false);
        Damage = false;

        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponent<Rigidbody>();
       
        tc = Cm.GetComponent<TPCamera>();
        c = player.GetComponent<Combo>();
        isDead = false;
        Time.timeScale = 1;
        s = GameObject.FindGameObjectWithTag("Manager").GetComponent<single>();
        mp = GameObject.FindGameObjectWithTag("Player").GetComponent<multiplePlayer>();


        // 村岡追加 効果音関係
        ps = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSounds>();
        st = GameObject.Find("SEPlayer").GetComponent<Soundtest>();// ここは一番下の行にしてください
        bp = GameObject.Find("BGMPlayer").GetComponent<BGMPlayer>();// ここは一番下の行にしてください
    }

    // Update is called once per frame
    void Update()
    {
        if (t.SpecialAttack)
        {
            ComboAttackMagnification = c.SpecialAttackMagnification;
        }
        else
        {
            ComboAttackMagnification = c.ComboAttackCurrentMagnification;
        }
        if (DamageValue * ComboAttackMagnification - DefencePower < 0)
        {
            DamagedValue = 0;
        }
        else
        {
            DamagedValue = DamageValue * ComboAttackMagnification - DefencePower;
        }

        if (Damage == true)
        {
            HP = HP - DamagedValue;
            slider.value = HP;
            Damage = false;
            t.Attack = false;

            // 村岡追加
            if (HP > 0)
            {
                st.SE_CantAttackPlayer();
                ps.isPlayAttackHitSound = true;
            }
            else
            {
                bp.JINGLE_ClearPlayer();
            }
        }

   

        if (HP <= 0 && !isDefeated)
        {
            isDefeated = true;
            isDead = true;
            StartCoroutine(SlowMotionAndLoadScene());
        }

        if(t.isTarget_Boss == true)
        {
            hp.SetActive(true);
        }
        else
        {
            hp.SetActive(false);
        }

        if(CanCollide)
        {
            s.targetList.Remove(this.gameObject);
            s.SortVectorList();
        }

    }

   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && (t.Attack || mp.At || t.SpecialAttack))
        {
            if (CanCollide)
                return;
            t.ismove_Boss = false;

            Damage = true;

            //Move = rb.velocity;
            if (DamagedValue <= 0 && !mp.At)
            {
                t.DenfensiveKnockBack();
            }

            if(t.SpecialAttack)
            {
                st.SE_SuperAttackPlayer();
            }
            CanCollide = true;
            if (DamagedValue < HP)
            {
                StartCoroutine(EnableCollisionAfterDelay(0.5f));
            }
            else
            {
                StartCoroutine(EnableCollisionAfterDelay(5f));
            }
        }
    }

   


    // スローモーションとシーン遷移のコルーチンを追加
    private IEnumerator SlowMotionAndLoadScene()
    {
        t.enabled = false;
        m.enabled = false;
        tc.enabled = false;
        messageText.text = "クリア！"; // クリアメッセージを表示
        Time.timeScale = 0.01f;

        yield return new WaitForSecondsRealtime(5f); // スローモーションの時間を調節
        Time.timeScale = 1f; // ゲームの時間を通常に戻す
        SceneManager.LoadScene("Result");
    }

    private IEnumerator EnableCollisionAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        CanCollide = false;
    }
}
