using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamBody_ver2 : MonoBehaviour
{

    public float BeamSpeed = 10.0f;
    public int BemaInterval = 60;
    public float BeamDestroy;
    [SerializeField]
    private GameObject BeamPrefab;
    private int interval;
    void Start()
    {
        
    }

    void Update()
    {
        interval += 1;//* Time.deltaTime

        if (interval % BemaInterval * Time.deltaTime == 0)
        {
            GameObject shell = Instantiate(BeamPrefab, transform.position, Quaternion.identity);
            Rigidbody shellRb = shell.GetComponent<Rigidbody>();
            shellRb.AddForce(transform.forward * BeamSpeed);
            Destroy(shell, BeamDestroy);
        }
    }

    /*
    IEnumerator BeamCoroutine()
    {
       
        GameObject shell = Instantiate(BeamPrefab, transform.position, Quaternion.identity);
        Rigidbody shellRb = shell.GetComponent<Rigidbody>();
        shellRb.AddForce(transform.forward * BeamSpeed);
        Destroy(shell, BemaInterval);
        yield return new WaitForSecondsRealtime(BemaInterval);

    }*/
}
