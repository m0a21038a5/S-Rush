using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamManager : MonoBehaviour
{
 

    [System.Serializable]
    public class BeamData
    {
        //�r�[���G�̏������
        public GameObject BeamPrefab;
        public Vector3 BeamPosition;

        public Vector3 BeamSize;

        //BeamEnemyBody�̕ϐ�
        public float BeamSpeed;
        public GameObject Beam;

        //BeamHPManager�̕ϐ�
        public float DamageValue;
        public float MaxHP;
        public float DefencePower;
        public int bosscount;

        //���|�b�v�̕b��
        public float respawnInterval_Beam;
    }
    public List<BeamData> beamDataList = new List<BeamData>();
    public GameObject Beam;
    public float respawnInterval;

    [SerializeField]
    GameObject particle;

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RespawnBeam(BeamData beamData)
    {
        StartCoroutine(RespawnCoroutine(beamData));
    }

    /// <summary>
    /// ���X�|�[���R���[�`��
    /// </summary>
    /// <param name="beamData"></param>
    /// <returns></returns>
    private IEnumerator RespawnCoroutine(BeamData beamData)
    {
        if (beamData.bosscount == 1 && gm.Boss1) yield break;
        if (beamData.bosscount == 2 && gm.Boss2) yield break;
        if (beamData.bosscount == 3 && gm.Boss3) yield break;
        else
        {

        }
        yield return new WaitForSecondsRealtime(beamData.respawnInterval_Beam - 0.3f);

        Instantiate(particle, beamData.BeamPosition, Quaternion.identity);

        yield return new WaitForSecondsRealtime(0.3f);

        GameObject newBeam = Instantiate(Beam, beamData.BeamPosition, Quaternion.identity);
        newBeam.transform.localScale = beamData.BeamSize;

        //BeamEnemyBody�̕ϐ�
        newBeam.GetComponent<BeamEnemy_ver2>().BeamSpeed = beamData.BeamSpeed;
        newBeam.GetComponent<BeamEnemy_ver2>().BeamPrefab = beamData.Beam;

        //BeamHPManager�̕ϐ�
        newBeam.GetComponent<BeamHPManager>().DamageValue = beamData.DamageValue;
        newBeam.GetComponent<BeamHPManager>().MaxHP = beamData.MaxHP;
        newBeam.GetComponent<BeamHPManager>().DefencePower = beamData.DefencePower;

        RemoveData(beamData);
    }

    private void RemoveData(BeamData beamData)
    {
        beamDataList.Remove(beamData);
    }
    /// <summary>
    /// ���X�|�[�����������G�̏���ۑ�
    /// </summary>
    /// <param name="enemyObject"></param>
    public void AddBeamData(GameObject enemyObject)
    {
        BeamData beamData = new BeamData();
        beamData.BeamPrefab = enemyObject;
        beamData.BeamSize = enemyObject.transform.localScale;

        BeamEnemy_ver2 b = enemyObject.GetComponent<BeamEnemy_ver2>();

        //BeamEnemy_ver2�̕ϐ�
        beamData.BeamSpeed = b.BeamSpeed;
        beamData.Beam = b.BeamPrefab;

        BeamHPManager bHP = enemyObject.GetComponent<BeamHPManager>();

        //BeamHPManager�̕ϐ�
        beamData.DamageValue = bHP.DamageValue;
        beamData.MaxHP = bHP.MaxHP;
        beamData.BeamPosition = bHP.firstPos;
        beamData.DefencePower = bHP.DefencePower;
        beamData.bosscount = bHP.BossCount;

        //���|�b�v����
        beamData.respawnInterval_Beam = respawnInterval;

        beamDataList.Add(beamData);
    }

    /// <summary>
    /// ���X�|�[�����������G�̏���o�^
    /// </summary>
    /// <param name="beamObject"></param>
    public void RequestsRespawn(GameObject beamObject)
    {
        foreach(BeamData beamData in beamDataList)
        {
            if(beamData.BeamPrefab == beamObject)
            {
                RespawnBeam(beamData);
                break;
            }
        }
    }
}
