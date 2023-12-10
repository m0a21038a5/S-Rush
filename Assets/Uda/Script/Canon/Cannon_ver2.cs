using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon_ver2 : MonoBehaviour
{
    
    [SerializeField] float launchCycle = 5.0f;
    [SerializeField] float offset = 0.0f;
    private float launchCount;

    [SerializeField] GameObject Bullet;

    void Start()
    {
        launchCount += offset;
    }

    void Update()
    {
        if (launchCount >= launchCycle)
        {
            GameObject newBullet = Instantiate(Bullet, transform.position +  new Vector3(0.0f, 4.0f, 0.0f), Quaternion.identity);
            newBullet.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 80.0f, 80.0f);

            launchCount = 0.0f;
        }

        launchCount += Time.deltaTime;
    }
}
