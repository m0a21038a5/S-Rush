using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    private Rigidbody rb;
    public MeshRenderer BeamMesh;
    public TrailRenderer BeamLineMesh;
    public float StopPlayer;
    //ビームに当たったらプレイヤーの動きを止める
    void Start()
    {
        GameObject PlayerObject = GameObject.FindGameObjectWithTag("Player");
        rb = PlayerObject.GetComponent<Rigidbody>();


    }

    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {//プレイヤーの動きを止める
            StartCoroutine(StopPlayerCoroutine());
            //プレイヤーをちょっと上に挙げる　埋まり防止

        }else if (other.gameObject.tag == "Enemy")
        {
            
        }
        else if (other.gameObject.tag == "Floor")
        {
           // Destroy(this.gameObject);
        }

    }

    IEnumerator StopPlayerCoroutine()
    {
        BeamMesh.enabled = false;
        BeamLineMesh.enabled = false;
        rb.isKinematic = true;
        yield return new WaitForSecondsRealtime(StopPlayer);
        //Debug.Log("再会");
        rb.isKinematic = false;
        Destroy(this.gameObject);
    }
}
