using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTarget : MonoBehaviour
{
    MeshRenderer Player;
    single s;
    targetChange tc;
    IsRendered R;
    Combo c;
    multipleTarget mt;
    target t;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<MeshRenderer>();
        s = this.gameObject.GetComponent<single>();
        tc = this.gameObject.GetComponent<targetChange>();
        c = Player.GetComponent<Combo>();
        t = Player.GetComponent<target>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //targetListへの追加と条件
    public void IsVisibleFromWall(MeshRenderer renderer,MeshRenderer StatueRenderer ,  Camera camera, LayerMask layerMask, GameObject targetObject)
    {
        //カメラの画面
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

        Bounds bounds = renderer.bounds;

        Vector3 targetPosition;
       
        targetPosition = StatueRenderer.bounds.center;
      
        float dis = Vector3.Distance(Player.bounds.center, targetPosition) + Vector3.Distance(camera.transform.position, Player.bounds.center);
        //カメラの画面内にある場合
        if(GeometryUtility.TestPlanesAABB(planes, bounds) == true && dis < 120f)
        {
            RaycastHit hit;
            Vector3 origin;
            if (!c.SpecialMode || !mt.ChangeCamera)
            {
                origin = camera.transform.position + new Vector3(0.0f, 5.0f, 0.0f);
            }
            else
            {
                origin = camera.transform.position;
            }
            Vector3 rayDirection = targetPosition - origin;
            float rayDistance = 70f;
            Physics.Raycast(origin , rayDirection, out hit, rayDistance, layerMask);
            Debug.DrawRay(origin, rayDirection * rayDistance, Color.red, 0.1f, false);
            R = targetObject.GetComponent<IsRendered>();
            R.visible = true;
            //Playerと敵の間に障害物がない場合追加
            if(hit.collider.gameObject == targetObject)
            {
                if (!s.targetList.Contains(targetObject))
                {
                    if(targetObject.GetComponent<BeamHPManager>() != null)
                    {
                        if(!targetObject.GetComponent<BeamHPManager>().Boss || !targetObject.GetComponent<BeamHPManager>().isDefeat)
                        {
                            s.targetList.Add(targetObject);
                            s.Sort = true;
                            tc.Change = true;
                        }
                    }
                    else if (targetObject.GetComponent<StatueHPManager>() != null)
                    {
                        if (!targetObject.GetComponent<StatueHPManager>().Boss)
                        {
                            s.targetList.Add(targetObject);
                            s.Sort = true;
                            tc.Change = true;
                        }
                    }
                    else if (targetObject.CompareTag("BOSS"))
                    {
                        s.targetList.Add(targetObject);
                        s.Sort = true;
                        tc.Change = true;
                    }
                }               
            }
            else
            {
                if (!t.isMoving)
                {
                    s.targetList.Remove(targetObject);
                    R.visible = false;
                }
            }
        }
        else
        {
            //List内を更新
            if (!t.isMoving)
            {
                s.targetList.Remove(targetObject);
                R.visible = false;
            }
        }
    }

  
}
