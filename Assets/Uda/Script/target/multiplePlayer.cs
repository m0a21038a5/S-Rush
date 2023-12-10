using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class multiplePlayer : MonoBehaviour
{

    multipleTarget mt;
    target t;
    [SerializeField]
    float RushSpeed;

    single s;

    public bool At;

    private PlayerSounds ps;
    private int next_i = 0;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        //複数ターゲット用Listを取得
        mt = GameObject.FindGameObjectWithTag("Manager").GetComponent<multipleTarget>();
        s = GameObject.FindGameObjectWithTag("Manager").GetComponent<single>();
        At = false;
        t = gameObject.GetComponent<target>();
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<PlayerSounds>();
    }

    // Update is called once per frame
    void Update()
    {
        if (At && mt.multipleTargetObject.Count > 1)
        {
            MoveTarget();
            t.isMoving = true;
        }
        
    }

    private void MoveTarget()
    {
        MoveMulti();
    }

    private IEnumerator MoveTargetsCoroutine()
    {
        yield return new WaitForSeconds(1f);
    }

    //複数ターゲット移動
    private void MoveMulti()
    {
        var FirstPoint = new Vector3(mt.multipleTargetObject[mt.multipleTargetObject.Count - 2].GetComponent<IsRendered>().StatueRenderer.bounds.center.x,
            mt.multipleTargetObject[mt.multipleTargetObject.Count - 1].GetComponent<IsRendered>().StatueRenderer.bounds.center.y,
            mt.multipleTargetObject[mt.multipleTargetObject.Count - 2].GetComponent<IsRendered>().StatueRenderer.bounds.center.z);

        var LastPoint = new Vector3(mt.multipleTargetObject[mt.multipleTargetObject.Count - 1].GetComponent<IsRendered>().StatueRenderer.bounds.center.x,
            mt.multipleTargetObject[mt.multipleTargetObject.Count - 2].GetComponent<IsRendered>().StatueRenderer.bounds.center.y,
            mt.multipleTargetObject[mt.multipleTargetObject.Count - 2].GetComponent<IsRendered>().StatueRenderer.bounds.center.z);

        var direction = (LastPoint - FirstPoint).normalized;
        
        //移動
        transform.DOPath(
            mt.multipleTargetObject.Select(target => target.GetComponent<IsRendered>().StatueRenderer.bounds.center).ToArray(),
            0.7f,
            PathType.CatmullRom
            )
            .SetLookAt(0.1f, Vector3.forward)
            .OnWaypointChange(pointNO =>
            {              
                    ps.isPlayAttackHitSound = true;
            })
            .OnComplete(() => {
            // すべてのターゲットに到達したらリセットします。
            mt.multipleTargetObject.Clear();
            mt.multiple = false;
            s.targetList.Clear();
            At = false;
            t.isMoving = false;
            float moveAmount = 10.0f;
            this.transform.position = Vector3.Lerp(this.transform.position, this.transform.position + direction * moveAmount, Time.deltaTime * RushSpeed);
            this.transform.eulerAngles = Vector3.zero;
        });
    }
}
