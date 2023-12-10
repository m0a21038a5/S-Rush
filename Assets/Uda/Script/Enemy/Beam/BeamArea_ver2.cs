using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamArea_ver2 : MonoBehaviour
{
    public GameObject EnemyGameObject;
    private BeamEnemy_ver2 EnemyStatus;
    void Start()
    {
        EnemyStatus = EnemyGameObject.GetComponent<BeamEnemy_ver2>();
        
    }

    void Update()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {//プレイヤーに当たったらEnemyBeamにする
           EnemyStatus.beamEnemyStatus = BeamEnemy_ver2.BeamEnemyStatus.Beam;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {//プレイヤーが離れたらEnamyChaseにする
            EnemyStatus.beamEnemyStatus = BeamEnemy_ver2.BeamEnemyStatus.ChasePlayerWalk;
        }
    }
}
