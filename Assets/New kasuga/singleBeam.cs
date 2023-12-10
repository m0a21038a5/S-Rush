using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class singleBeam : MonoBehaviour
{
    public LayerMask raycastLayer; // Rayを飛ばす対象のレイヤーマスク
    public float raycastDistance = 10f; // Rayの飛ばす距離



    [SerializeField]
    private List<GameObject> targetList;

    public Transform character;// キャラクターのTransform

    private target ta;
    private single si;

    public bool SortB;

    [SerializeField]
    public GameObject Playerobj;

    [SerializeField] public GameObject TargetCamera;

    void Start()
    {
        Playerobj = GameObject.Find("Player");
        GameObject ConeObj = GameObject.Find("Cone");
        ta = Playerobj.GetComponent<target>();
        si = ConeObj.GetComponent<single>();
        SortB = false;
    }
    void Update()
    {
        if (SortB == false)
        {
            if (ta.isTarget_Beam == true)
            {
                //ta.BeamPos = targetList[0].transform.position;
                ta.TargetBeam = targetList[0];
            }
            else
            {
                ta.BeamPos = Vector3.zero;
                ta.TargetBeam = null;
            }
        }
        if (ta.clear == true)
        {
            ListClear();
            ta.clear = false;
        }

    }





    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Beam")) // コライダーに入ったオブジェクトがBeamタグを持っているか確認
        {
            ta.isTarget_Beam = true;
            Vector3 raycastDirection = other.transform.position - transform.position; // プレイヤーの位置から自分の位置を引いたベクトルをRayの方向とする
            Ray ray = new Ray(TargetCamera.transform.position, raycastDirection); // 自分の位置を起点にRayを作成
            Debug.DrawRay(ray.origin, raycastDirection, Color.red, 3, false);
            RaycastHit[] hits = Physics.RaycastAll(ray, raycastDistance, raycastLayer); // Rayを飛ばして対象のレイヤーマスクにヒットした全てのオブジェクトを取得

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Beam")) // ヒットしたオブジェクトがEnemyタグを持っているか確認
                {

                    if (!targetList.Contains(other.gameObject)) // List に含まれていない場合のみ追加する
                    {
                        targetList.Add(other.gameObject);

                    }
                    if (SortB == false)
                    {
                        si.SortVectorList();
                    }
                }
            }


        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Beam")) // コライダーから出たオブジェクトがPlayerタグを持っているか確認
        {
            si.ListClear();
            ta.isTarget_Beam = false;
        }
    }
    public void ListClear()
    {
        targetList.Clear(); //Listすべての要素を削除

    }
}
