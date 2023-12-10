using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetChange : MonoBehaviour
{
    //右側のオブジェクト
    [SerializeField]
    public List<GameObject> right;
    //左側のオブジェクト
    [SerializeField]
    public List<GameObject> left;

    [SerializeField]
    //target取得
    target t;
    //ターゲットオブジェクトが変更された処理のフラグ
    public bool Change;

    //single取得
    single s;
    //singleBeam取得
    //singleBeam sb;

    TargetController tc;

    // プレイヤーとカメラ ----[ 村岡追加 ]----
    private GameObject Player;
    private MainCamera MC;

    // Start is called before the first frame update
    void Start()
    {
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();
        s = GameObject.FindGameObjectWithTag("Manager").GetComponent<single>();
        tc = GameObject.FindGameObjectWithTag("Target").GetComponent<TargetController>();
        Change = true;

        // プレイヤーとカメラ取得 ----[ 村岡追加 ]----
        MC = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCamera>();
        Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        SortLR();
        if (Change == true && s.targetList.Count > 1)
        {
            foreach (GameObject obj in s.targetList)
            {
                Vector2 targetPos = Vector2.zero;
                if (obj.CompareTag("Statue") || obj.CompareTag("BOSS"))
                {
                    targetPos = RectTransformUtility.WorldToScreenPoint(Camera.main, obj.GetComponent<IsRendered>().StatueRenderer.bounds.center);
                    
                }
                else if(obj.CompareTag("Beam"))
                {
                    targetPos = RectTransformUtility.WorldToScreenPoint(Camera.main, obj.transform.position);
                }

                if (tc.targetPosition.x < targetPos.x && (obj != t.TargetStatue && obj != t.TargetBeam && obj != t.TargetBoss))
                {
                    if (!right.Contains(obj))
                    {
                        right.Add(obj);
                    }
                }
                if (tc.targetPosition.x > targetPos.x && (obj != t.TargetStatue && obj != t.TargetBeam && obj != t.TargetBoss))
                {
                    if (!left.Contains(obj))
                    {
                        left.Add(obj);
                    }
                }
            }
            Change = false;
        }

        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown("joystick button 4"))
        {
            if (t.isTarget_Statue == true || t.isTarget_Beam == true || t.isTarget_Boss == true)
            {
                // 敵方向へのベクトル ----[ 村岡追加 ]----
                Vector3 toEnemyVec = new Vector3();

                if (left[0].CompareTag("Statue"))
                {
                    t.isTarget_Statue = true;
                    t.isTarget_Beam = false;
                    t.isTarget_Boss = false;
                    t.TargetStatue = left[0];
                    t.TargetBeam = null;
                    t.TargetBoss = null;
                    toEnemyVec = (t.TargetStatue.transform.position - Player.transform.position);// 敵方向へのベクトル計算 ----[ 村岡追加 ]----
                }
                if (left[0].CompareTag("Beam"))
                {
                    t.isTarget_Beam = true;
                    t.isTarget_Statue = false;
                    t.isTarget_Boss = false;
                    t.TargetBeam = left[0];
                    t.TargetStatue = null;
                    t.TargetBoss = null;
                    toEnemyVec = (t.TargetBeam.transform.position - Player.transform.position);// 敵方向へのベクトル計算 ----[ 村岡追加 ]----
                }
                if (left[0].CompareTag("BOSS"))
                {
                    t.isTarget_Boss = true;
                    t.isTarget_Beam = false;
                    t.isTarget_Statue = false;
                    t.TargetBoss = left[0];
                    t.TargetStatue = null;
                    t.TargetBeam = null;
                    toEnemyVec = (t.TargetBoss.transform.position - Player.transform.position);// 敵方向へのベクトル計算 ----[ 村岡追加 ]----
                }
                s.Sort = false;
                Change = true;
                right.Clear();
                left.Clear();

                // カメラリセットを利用した敵方向への注目 ----[ 村岡追加 ]----
                MC.isCameraResetNow = true;
                MC.reset_angle = Mathf.Atan2(-toEnemyVec.x, toEnemyVec.z) * Mathf.Rad2Deg;
            }

        }
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown("joystick button 5"))
        {
            if (t.isTarget_Statue == true || t.isTarget_Beam == true || t.isTarget_Boss == true)
            {
                // 敵方向へのベクトル ----[ 村岡追加 ]----
                Vector3 toEnemyVec = new Vector3();

                if (right[0].CompareTag("Statue"))
                {
                    t.isTarget_Statue = true;
                    t.isTarget_Beam = false;
                    t.isTarget_Boss = false;
                    t.TargetStatue = right[0];
                    t.TargetBeam = null;
                    t.TargetBoss = null;
                    toEnemyVec = (t.TargetStatue.transform.position - Player.transform.position);// 敵方向へのベクトル計算 ----[ 村岡追加 ]----
                }
                if (right[0].CompareTag("Beam"))
                {
                    t.isTarget_Beam = true;
                    t.isTarget_Statue = false;
                    t.isTarget_Boss = false;
                    t.TargetBeam = right[0];
                    t.TargetStatue = null;
                    t.TargetBoss = null;
                    toEnemyVec = (t.TargetBeam.transform.position - Player.transform.position);// 敵方向へのベクトル計算 ----[ 村岡追加 ]----
                }
                if (right[0].CompareTag("BOSS"))
                {
                    t.isTarget_Boss = true;
                    t.isTarget_Beam = false;
                    t.isTarget_Statue = false;
                    t.TargetBoss = right[0];
                    t.TargetStatue = null;
                    t.TargetBeam = null;
                    toEnemyVec = (t.TargetBoss.transform.position - Player.transform.position);// 敵方向へのベクトル計算 ----[ 村岡追加 ]----
                }
                s.Sort = false;
                Change = true;
                right.Clear();
                left.Clear();

                // カメラリセットを利用した敵方向への注目 ----[ 村岡追加 ]----
                MC.isCameraResetNow = true;
                MC.reset_angle = Mathf.Atan2(-toEnemyVec.x, toEnemyVec.z) * Mathf.Rad2Deg;
            }

        }

        if (!(s.targetList.Count > 0))
        {
            right.Clear();
            left.Clear();
            t.isTarget_Statue = false;
            t.isTarget_Beam = false;
            t.isTarget_Boss = false;
            t.TargetBeam = null;
            t.TargetStatue = null;
            t.TargetBoss = null;
        }
    }

    private void SortLR()
    {
        right.Sort((a, b) =>
        {
            Vector3 screenPosA = RectTransformUtility.WorldToScreenPoint(Camera.main, a.transform.position);
            Vector3 screenPosB = RectTransformUtility.WorldToScreenPoint(Camera.main, b.transform.position);
            float xDifferenceA = Mathf.Abs(screenPosA.x - tc.targetPosition.x);
            float xDifferenceB = Mathf.Abs(screenPosB.x - tc.targetPosition.x);
            return xDifferenceA.CompareTo(xDifferenceB);
        });

        left.Sort((a, b) =>
        {
            Vector3 screenPosA = RectTransformUtility.WorldToScreenPoint(Camera.main, a.transform.position);
            Vector3 screenPosB = RectTransformUtility.WorldToScreenPoint(Camera.main, b.transform.position);
            float xDifferenceA = Mathf.Abs(screenPosA.x - tc.targetPosition.x);
            float xDifferenceB = Mathf.Abs(screenPosB.x - tc.targetPosition.x);
            return xDifferenceA.CompareTo(xDifferenceB);
        });
    }

}
