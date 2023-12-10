using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallFloor : MonoBehaviour
{//上に乗ると落ちる床です、時間経過で復活します PlayerにPlayerタグを入れないと動作しません
    //FallSpeed 落ちる速さ 　FallDistance 落ちる終点  FallStart 初期位置  FallRevivalFloorTime 復活時間
    public float FallDistance;
    public float FallSpeed;
    public float FallStart;
    public float FallRevivalFloorTime;
    public bool isStopAbilityFallFloor;
    private Vector3 Floorpos;
    public bool FallStartNow;
    void Start()
    {
        Floorpos = transform.position;
    }

    void Update()
    {
        if (FallStartNow == true)
        {
            FallFloorDrop();
        }
        else if (FallStartNow == false)
        {

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FallStartNow = true;
        }
    }
    void FallFloorDrop()
    {
        // FallDistanceだけ下に落ちる
        Floorpos.y -= Time.deltaTime * FallSpeed;
        transform.position = Floorpos;
        // FallDistanceになったら見えなくする
        if (Floorpos.y < FallDistance)
        {
            this.gameObject.SetActive(false);
            // 3秒後に復活する
            Invoke("Revival", FallRevivalFloorTime);
        }
    }
    void Revival()
    {
        //このメゾットが呼び出されたら復活
        this.gameObject.SetActive(true);
        Floorpos.y = FallStart;
        transform.position = Floorpos;
        FallStartNow = false;
    }
}
