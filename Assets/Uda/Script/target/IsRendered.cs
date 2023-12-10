using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsRendered : MonoBehaviour
{
    //åüèoÇ≥ÇπÇΩÇ¢Renderer
    [SerializeField] MeshRenderer targetRenderer;
    [SerializeField] public MeshRenderer StatueRenderer;
    [SerializeField] MeshRenderer StatueRenderer_01;
    [SerializeField] MeshRenderer StatueRenderer_02;
    [SerializeField] Camera cam;
    [SerializeField] LayerMask layerMask;
    [SerializeField] GameObject enemy_01;
    [SerializeField] GameObject enemy_02;

    public bool visible;
   
    SingleTarget st;

    target t;



    void Start()
    {
        st = GameObject.FindGameObjectWithTag("Manager").GetComponent<SingleTarget>();
        if (this.gameObject.CompareTag("BOSS"))
        {
            GameObject first = this.transform.GetChild(3).gameObject;
            targetRenderer = first.transform.GetChild(0).GetComponent<MeshRenderer>();
        }
        else
        {
            targetRenderer = this.gameObject.GetComponent<MeshRenderer>();
        }
        if(this.gameObject.CompareTag("Statue"))
        {
            GameObject first = this.transform.GetChild(5).gameObject;
            enemy_01 = first.transform.GetChild(0).gameObject;
            enemy_02 = first.transform.GetChild(1).gameObject;
            StatueRenderer_01 = enemy_01.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
            StatueRenderer_02 = enemy_02.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        }
        cam = Camera.main;
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();
    }

    void Update()
    {
        cam = Camera.main;
        if (this.gameObject.CompareTag("Statue"))
        {
            if (enemy_01.activeSelf == true)
            {
                StatueRenderer = StatueRenderer_01;
            }
            if (enemy_02.activeSelf == true)
            {
                StatueRenderer = StatueRenderer_02;
            }
        }
       
        st.IsVisibleFromWall(targetRenderer, StatueRenderer, cam, layerMask, this.gameObject);
        

        if(this.gameObject.CompareTag("Statue") && t.TargetStatue == this.gameObject)
        {
          t.StatuePos2 = StatueRenderer.bounds.center;
        }
        if (this.gameObject.CompareTag("BOSS") && t.TargetBoss == this.gameObject)
        {
            t.BossPos = StatueRenderer.bounds.center;
        }

    }
}
