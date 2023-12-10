using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerArea : MonoBehaviour
{

    [SerializeField] public List<GameObject> DangerList;
    [SerializeField]
    ParticleSystem particle;
    [SerializeField] DangerZoneMesh dm;
    public GameObject Boss;
    bool Touch;

    public Soundtest st;
    public bool isDestroy = false;

    target t;
    // Start is called before the first frame update
    void Start()
    {
        Touch = true;
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();
        st = GameObject.Find("SEPlayer").GetComponent<Soundtest>();// Ç±Ç±ÇÕàÍî‘â∫ÇÃçsÇ…ÇµÇƒÇ≠ÇæÇ≥Ç¢
    }

    // Update is called once per frame
    void Update()
    {

        if (Boss == null)
        {
           DestroyObjects();
            //StartCoroutine(DelayTarget());
        }
        if (Touch)
        {
            foreach (GameObject obj in DangerList)
            {
                obj.SetActive(false);
            }
        }
        else
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(RepopObjects());
            Touch = false;
            //dm.DangerMesh = this.gameObject.GetComponent<MeshRenderer>();
        }
        if ((other.gameObject.tag == "Beam" || other.gameObject.tag == "Statue") && !other.gameObject.name.Contains("Variant") && !DangerList.Contains(other.gameObject))
        {
            DangerList.Add(other.gameObject);
        }
    }


    private IEnumerator RepopObjects()
    {
        foreach (GameObject obj in DangerList)
        {
            Instantiate(particle, obj.transform.position, Quaternion.identity);
            yield return new WaitForSecondsRealtime(0.2f);
            if (obj.activeSelf == false)
            {
                obj.SetActive(true);
                st.SE_EnemyDead1Player();
            }

            if (Boss == null)
            {
                break;
            }
        }

        yield return null;
    }

    private void DestroyObjects()
    {
        foreach (GameObject obj in DangerList)
        {
            if (obj != null)
            {
                if (obj.CompareTag("Beam"))
                {
                    obj.GetComponent<BeamHPManager>().Boss = true;
                    Destroy(obj.GetComponent<BeamHPManager>().hp);
                    Destroy(obj);
                }
                else if (obj.CompareTag("Statue"))
                {
                    obj.GetComponent<StatueHPManager>().Boss = true;
                    Destroy(obj.GetComponent<StatueHPManager>().hp);
                    Destroy(obj);
                }
            }
        }
        dm.DangerMesh.Add(GetComponent<MeshRenderer>());
        isDestroy = true;
    }

    private IEnumerator DelayTarget()
    {
        t.isMoving = true;
        yield return null;
        t.isMoving = false;
    }
}
