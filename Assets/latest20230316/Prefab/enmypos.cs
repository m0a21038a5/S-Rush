using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enmypos : MonoBehaviour
{
    private GameObject area;
    public GameObject enemyObject;
    public GameObject ememyPos;
    public bool isEnemy = false;
    public float StatueCenter;

    public Vector3 position;

    public target targetList;

    private target ta;

    public target target;

    // Start is called before the first frame update
    void Start()
    {
        area = GameObject.Find("Player");
        enemyObject = GameObject.Find("Player");
        ememyPos = GameObject.Find("Player");
        GameObject Playerobj = GameObject.Find("Player");
        ta = Playerobj.GetComponent<target>();
    }

    // Update is called once per frame
    void Update()
    {
        position = new Vector3(this.transform.position.x, this.transform.position.y + StatueCenter, this.transform.position.z);
        //敵の位置を目的の位置に指定
        if (isEnemy == true)
        {
            //enemyObject.GetComponent<target>().TargetStatue= this.gameObject;
            //ememyPos.GetComponent<target>().StatuePos2 = position;
        }
    }

    //敵がエリアに入っているか
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "PlayerArea")
        {
            isEnemy = true;
            AddDataTotarget(position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PlayerArea")
        {
            isEnemy = false;
        }
    }

    public void AddDataTotarget(Vector3 position)
    {
        Data data = new Data();
        data.position = position;
        Debug.Log(position) ;
        target.AddData(data);
    }

    private void OnDestroy()
    {
        ta.isTarget_Statue = false;
        ta.isTarget_Boss = false;
        ta.TargetStatue = null;
    }
}
