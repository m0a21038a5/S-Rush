using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


[System.Serializable]
public class MyData
{
    public List<Vector3> CheckPoints = new List<Vector3>();
    public List<Vector3> colliderSizes = new List<Vector3>();
    public List<Vector3> colliderCenter = new List<Vector3>();

    public List<Vector3> LookAtPoint = new List<Vector3>();
    public List<Vector3> LookAtSizes = new List<Vector3>();
    public List<Vector3> LookAtCenter = new List<Vector3>();

    public List<Vector3> DestroyEnemy = new List<Vector3>();
    public List<Vector3> DestroyEnemySizes = new List<Vector3>();
    public List<Vector3> DestroyEnemyCenter = new List<Vector3>();
}



public class Respawn : MonoBehaviour
{
    public MyData data;
    public GameObject CheckPoints;
    private static GUIStyle labelStyle = new GUIStyle();
    int i = 0;


    GameObject player;
    target t;

    public GameObject LookAtPoint;
    public bool LAP = false;
    RespawnManager R;
    [SerializeField]
    public List<GameObject> LookAtList;

    private static int idCounter = 1;

    //kasuga
    public GameObject DestroyEnemy;
    [SerializeField]
    public List<GameObject> DestroyEnemyList;
    public bool DEP = false;
    public int areaon = 0;
    public GameObject desenobj;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < data.CheckPoints.Count; i++)
        {
            GameObject obj = Instantiate(CheckPoints,data.CheckPoints[i] , Quaternion.identity);
            BoxCollider boxCollider = obj.GetComponent<BoxCollider>();
            boxCollider.size = data.colliderSizes[i];
            boxCollider.center = data.colliderCenter[i];
        }

        for (int i = 0; i < data.LookAtPoint.Count; i++)
        {
            GameObject LookAtobj = Instantiate(LookAtPoint, data.LookAtPoint[i], Quaternion.identity);
            LookAtList.Add(LookAtobj);
            BoxCollider boxCollider = LookAtobj.GetComponent<BoxCollider>();
            boxCollider.size = data.LookAtSizes[i];
            boxCollider.center = data.LookAtCenter[i];
        }

        //kasuga
        for (int i = 0; i < data.DestroyEnemy.Count; i++)
        {
            GameObject DestroyEnemyobj = Instantiate(DestroyEnemy, data.DestroyEnemy[i], Quaternion.identity);
            DestroyEnemyList.Add(DestroyEnemyobj);
            BoxCollider boxCollider = DestroyEnemyobj.GetComponent<BoxCollider>();
            boxCollider.size = data.DestroyEnemySizes[i];
            boxCollider.center = data.DestroyEnemyCenter[i];
        }

        player = GameObject.FindGameObjectWithTag("Player");
        t = player.GetComponent<target>();
        R = GameObject.FindGameObjectWithTag("Player").GetComponent<RespawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (i > data.CheckPoints.Count)
            {
                i = 0;
                LookAtUP();
                player.transform.position = data.CheckPoints[i];
            }
            else
            {
                i++;
                LookAtUP();
                player.transform.position = data.CheckPoints[i];
            }
        }

        if (R.lookup == true)
        {
            for (int i = 0; i < data.CheckPoints.Count; i++)
            {
                if (R.CPobj.transform.position == data.CheckPoints[i])
                {
                    Debug.Log("lookup" + i);
                    R.lookpos = data.LookAtPoint[i];
                    R.lookatobj = LookAtList[i];
                    R.lookup = false;
                }
            }
        }

        //kasuga
        for (int i = 0; i < data.CheckPoints.Count; i++)
        {
            if (R.position == data.CheckPoints[i])
            {

                areaon = i - 1;
                if (areaon >= 0)
                {
                    desenobj = DestroyEnemyList[areaon];
                    desenobj.gameObject.SetActive(true);
                }
            }
        }

    }

    void LookAtUP()
    {
        if (LAP == true)
        {
            player.transform.LookAt(data.LookAtPoint[i]);
            LAP = false;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        labelStyle.fontSize = 50;
        labelStyle.normal.textColor = Color.white;

        
        
        for (int i = 0; i < data.CheckPoints.Count; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(data.CheckPoints[i],0.5f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(data.CheckPoints[i] + data.colliderCenter[i], data.colliderSizes[i]);
            // Display the index of this object in the list
            Vector3 worldPos = data.CheckPoints[i] + Vector3.up;
            UnityEditor.Handles.Label(worldPos, (i + 1).ToString());
        }
        for (int i = 0; i < data.LookAtPoint.Count; i++)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(data.LookAtPoint[i], 0.5f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(data.LookAtPoint[i] + data.LookAtCenter[i], data.LookAtSizes[i]);
            // Display the index of this object in the list
            Vector3 worldPos = data.LookAtPoint[i] + Vector3.up;
            UnityEditor.Handles.Label(worldPos, (i + 1).ToString());
        }

        for (int i = 0; i < data.DestroyEnemy.Count; i++)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(data.DestroyEnemy[i], 0.5f);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(data.DestroyEnemy[i] + data.DestroyEnemyCenter[i], data.DestroyEnemySizes[i]);
            // Display the index of this object in the list
            Vector3 worldPos = data.DestroyEnemy[i] + Vector3.up;
            UnityEditor.Handles.Label(worldPos, (i + 1).ToString());
        }
    }
#endif

}
