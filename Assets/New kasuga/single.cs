using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class single : MonoBehaviour
{
    /*
   public LayerMask raycastLayer; // Rayを飛ばす対象のレイヤーマスク
   public float raycastDistance = 10f; // Rayの飛ばす距離
   */

    [SerializeField]
    public List<GameObject> targetList;

    private target ta;



    [SerializeField]
    public GameObject Playerobj;

    public bool Sort = false;
    bool Change;



    void Start()
    {
        Playerobj = GameObject.FindGameObjectWithTag("Player");
        ta = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();

    }
    void Update()
    {
        if (Sort == true)
        {
            if(!ta.isMoving)
            {
                SortVectorList();
            }
          
            //ta.StatuePos2 = new Vector3(targetList[0].transform.position.x, targetList[0].transform.position.y + 4, targetList[0].transform.position.z);
            if (targetList[0].CompareTag("Statue"))
            {
                ta.TargetStatue = targetList[0];
                ta.TargetBeam = null;
                ta.TargetBoss = null;
                ta.isTarget_Statue = true;
                ta.isTarget_Beam = false;
                ta.isTarget_Boss = false;
            }
            if (targetList[0].CompareTag("Beam"))
            {
                ta.TargetBeam = targetList[0];
                ta.TargetStatue = null;
                ta.TargetBoss = null;
                ta.isTarget_Beam = true;
                ta.isTarget_Statue = false;
                ta.isTarget_Boss = false;
            }
            if (targetList[0].CompareTag("BOSS"))
            {
                ta.TargetBoss = targetList[0];
                ta.TargetStatue = null;
                ta.TargetBeam = null;
                ta.isTarget_Boss = true;
                ta.isTarget_Statue = false;
                ta.isTarget_Beam = false;
            }
        }

        
       
        
        /*
        if (ta.clear == true)
        {
            ListClear();
            ta.clear = false;
        }
        */

        if (!(targetList.Count > 0))
        {
            Sort = true;
        }

        if ((targetList[0] == null || targetList[0].activeSelf == false) && !ta.isMoving)
        {
            SortVectorList();
        }

       
    }






    /*
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Statue") || other.CompareTag("Beam") || other.CompareTag("BOSS")) // コライダーに入ったオブジェクトがStatueタグを持っているか確認
        {
            Vector3 raycastDirection = other.transform.position - transform.position; // プレイヤーの位置から自分の位置を引いたベクトルをRayの方向とする
            Ray ray = new Ray(TargetCamera.transform.position, raycastDirection); // 自分の位置を起点にRayを作成
            Debug.DrawRay(ray.origin, raycastDirection, Color.red, 3, false);
            RaycastHit[] hits = Physics.RaycastAll(ray, raycastDistance, raycastLayer); // Rayを飛ばして対象のレイヤーマスクにヒットした全てのオブジェクトを取得

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Statue") || hit.collider.CompareTag("Beam") || hit.collider.CompareTag("BOSS")) // ヒットしたオブジェクトがEnemyタグを持っているか確認
                {

                    if (!targetList.Contains(other.gameObject)) // List に含まれていない場合のみ追加する
                    {
                        targetList.Add(other.gameObject);
                        if (Sort == false)
                        {
                            SortVectorList();
                        }
                    }
                    
                }
            }


        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Statue")) // コライダーから出たオブジェクトがPlayerタグを持っているか確認
        {
            ListClear();
            ta.isTarget_Statue = false;
            ta.StatuePos2 = Vector3.zero;
            ta.TargetStatue = null;
        }
        if (other.CompareTag("Beam")) // コライダーから出たオブジェクトがPlayerタグを持っているか確認
        {
            ListClear();
            ta.isTarget_Beam = false;
            ta.BeamPos = Vector3.zero;
            ta.TargetBeam = null;
        }
        if(other.CompareTag("BOSS"))
        {
            ListClear();
            ta.isTarget_Boss = false;
            ta.BossPos = Vector3.zero;
            ta.TargetBoss = null;
        }
    }
    */
    public void ListClear()
    {
        targetList.Clear(); //Listすべての要素を削除
    }


    public void SortVectorList()
    {

        // キャラクターの位置を取得
        Vector3 characterPosition = Playerobj.transform.position;

        targetList.RemoveAll(item => item == null);

        // Vector3のリストをキャラクターに近い順にソートする
        targetList.Sort((a, b) => Vector3.Distance(a.transform.position, characterPosition).CompareTo(Vector3.Distance(b.transform.position, characterPosition)));
        //Debug.Log("sort");
    }

   
}
