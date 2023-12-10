using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamArea : MonoBehaviour
{
    public GameObject EnemyGameObject;
    private BeamEnemy EnemyStatus;
    void Start()
    {
        EnemyStatus = EnemyGameObject.GetComponent<BeamEnemy>();
        
    }

    void Update()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {//プレイヤーに当たったらEnemyBeamにする
           EnemyStatus.beamEnemyStatus = BeamEnemy.BeamEnemyStatus.Beam;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {//プレイヤーが離れたらEnamyChaseにする
            EnemyStatus.beamEnemyStatus = BeamEnemy.BeamEnemyStatus.WaitWalk;
        }
    }
}
