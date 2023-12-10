using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{


    [System.Serializable]
    public class StatueData
    {
        //近接敵の初期情報
        public GameObject StatuePrefab;
        public Vector3 StatueSize;
        public Vector3 firstPos;

        //StatueEnemyMoveの変数
        public Vector3[] wayPoints;
        public int[] patrolRoute;
        public float[] patrolSeconds;
        public float patrolSpeed;
        public float chaseSpeed;
        public float rotateSpeed;
        public float accelPower;
        public float breakPower;
        public float stopSeconds;
        public float chaseStartSeconds;
        public float attackAimSeconds;
        public float attackStartSeconds;
        public float attackStopSeconds;
        public float patrolStopSeconds;
        public float searchRange;
        public float attackRange;
        public float wayPointsRadius;

        //MoveObjectの変数
        public string _earthTag;
        public float _moveSpeeds;

        //StatueHPManagerの変数
        public float DamageValue;
        public float MaxHP;
        public float height;
        public float DefencePower;
        public int bosscount;

        //リポップの秒数
        public float respawnInterval_Statue;
    }

    public List<StatueData> statueDataList = new List<StatueData>();
    //GameObject[] StatueObjects;
    //リポップに使用するprefab
    public GameObject Statue;
    public float respawnInterval;

    [SerializeField]
    GameObject particle;

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        /*
        StatueObjects = GameObject.FindGameObjectsWithTag("Statue");
        foreach (GameObject enemyObject in StatueObjects)
        {
            AddStatueData(enemyObject);
        }
        */

        gm = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RespawnStatue(StatueData statueData)
    {
        StartCoroutine(RespawnCoroutine(statueData));
    }
    /// <summary>
    /// リスポーンコルーチン
    /// </summary>
    /// <param name="statueData"></param>
    /// <returns></returns>
    private IEnumerator RespawnCoroutine(StatueData statueData)
    {
        //中ボスが倒された場合この条件の敵はリスポーンしない
        if (statueData.bosscount == 1 && gm.Boss1) yield break;
        else if (statueData.bosscount == 2 && gm.Boss2) yield break;
        else if (statueData.bosscount == 3 && gm.Boss3) yield break;
        else
        {
          
        }

        yield return new WaitForSecondsRealtime(statueData.respawnInterval_Statue - 0.2f);
        
        Instantiate(particle, statueData.firstPos, Quaternion.identity);

        yield return new WaitForSecondsRealtime(0.2f);

        GameObject newStatue = Instantiate(Statue, statueData.firstPos, Quaternion.identity);
        newStatue.transform.localScale = statueData.StatueSize;

        //StatueEnemyMoveの変数代入
        for (int i = 0; i < statueData.wayPoints.Length; i++)
        {
            statueData.wayPoints[i] -= statueData.firstPos;
        }
         newStatue.GetComponent<StatueEnemyMove>().wayPoints = statueData.wayPoints;
         newStatue.GetComponent<StatueEnemyMove>().patrolRoute = statueData.patrolRoute;
         newStatue.GetComponent<StatueEnemyMove>().patrolSeconds = statueData.patrolSeconds;
         newStatue.GetComponent<StatueEnemyMove>().patrolSpeed = statueData.patrolSpeed;
         newStatue.GetComponent<StatueEnemyMove>().chaseSpeed = statueData.chaseSpeed;
         newStatue.GetComponent<StatueEnemyMove>().rotateSpeed = statueData.rotateSpeed;
         newStatue.GetComponent<StatueEnemyMove>().accelPower = statueData.accelPower;
         newStatue.GetComponent<StatueEnemyMove>().breakPower = statueData.breakPower;
         newStatue.GetComponent<StatueEnemyMove>().stopSeconds = statueData.stopSeconds;
         newStatue.GetComponent<StatueEnemyMove>().chaseStartSeconds = statueData.chaseStartSeconds;
         newStatue.GetComponent<StatueEnemyMove>().attackAimSeconds = statueData.attackAimSeconds;
         newStatue.GetComponent<StatueEnemyMove>().attackStartSeconds = statueData.attackStartSeconds;
         newStatue.GetComponent<StatueEnemyMove>().attackStopSeconds = statueData.attackStopSeconds;
         newStatue.GetComponent<StatueEnemyMove>().patrolStopSeconds = statueData.patrolStopSeconds;
         newStatue.GetComponent<StatueEnemyMove>().searchRange = statueData.searchRange;
         newStatue.GetComponent<StatueEnemyMove>().attackRange = statueData.attackRange;
         newStatue.GetComponent<StatueEnemyMove>().wayPointsRadius = statueData.wayPointsRadius;

        //M0veObjectの変数代入
        newStatue.GetComponent<MoveObject>()._earthTag = statueData._earthTag;
        newStatue.GetComponent<MoveObject>()._moveSpeeds = statueData._moveSpeeds;

        //StatueHPManagerの変数代入
        newStatue.GetComponent<StatueHPManager>().DamageValue = statueData.DamageValue;
        newStatue.GetComponent<StatueHPManager>().MaxHP = statueData.MaxHP;
        newStatue.GetComponent<StatueHPManager>().height = statueData.height;
        newStatue.GetComponent<StatueHPManager>().DefencePower = statueData.DefencePower;
        RemoveData(statueData);

    }

    private void RemoveData(StatueData statueData)
    {
        statueDataList.Remove(statueData);
    }
    /// <summary>
    /// リスポーンさせたい敵の情報を保存
    /// </summary>
    /// <param name="enemyObject"></param>
    public void AddStatueData(GameObject enemyObject)
    {
        StatueData statueData = new StatueData();
        statueData.StatuePrefab = enemyObject;
        statueData.StatueSize = enemyObject.transform.localScale;

        StatueEnemyMove s = enemyObject.GetComponent<StatueEnemyMove>();

        //StatueEnemyMoveの変数
        statueData.firstPos = s.firstPos;
        statueData.wayPoints = s.wayPoints;
        statueData.patrolRoute = s.patrolRoute;
        statueData.patrolSeconds = s.patrolSeconds;
        statueData.patrolSpeed = s.patrolSpeed;
        statueData.chaseSpeed = s.chaseSpeed;
        statueData.rotateSpeed = s.rotateSpeed;
        statueData.accelPower = s.accelPower;
        statueData.breakPower = s.breakPower;
        statueData.stopSeconds = s.stopSeconds;
        statueData.chaseStartSeconds = s.chaseStartSeconds;
        statueData.attackAimSeconds = s.attackAimSeconds;
        statueData.attackStartSeconds = s.attackStartSeconds;
        statueData.attackStopSeconds = s.attackStopSeconds;
        statueData.patrolStopSeconds = s.patrolStopSeconds;
        statueData.searchRange = s.searchRange;
        statueData.attackRange = s.attackRange;
        statueData.wayPointsRadius = s.wayPointsRadius;

        MoveObject m = enemyObject.GetComponent<MoveObject>();

        //MoveObjectの変数
        statueData._earthTag = m._earthTag;
        statueData._moveSpeeds = m._moveSpeeds;

        StatueHPManager sHP = enemyObject.GetComponent<StatueHPManager>();

        //StatueHPManagerの変数
        statueData.DamageValue = sHP.DamageValue;
        statueData.MaxHP = sHP.MaxHP;
        statueData.height = sHP.height;
        statueData.DefencePower = sHP.DefencePower;
        statueData.bosscount = sHP.BossCount;

        //リポップ時間
        statueData.respawnInterval_Statue = respawnInterval;

        statueDataList.Add(statueData);
    }
    /// <summary>
    /// リスポーンさせたい敵を登録
    /// </summary>
    /// <param name="statueObject"></param>
    public void RequestsRespawn(GameObject statueObject)
    {
        foreach(StatueData statueData in statueDataList)
        {
            if(statueData.StatuePrefab == statueObject)
            {
                RespawnStatue(statueData);
                break;
            }
        }
    }
}
