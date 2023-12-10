using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    // プレイヤーの足音(左)
    public bool isPlayWalkLSound = false; // このスクリプトでON
    // プレイヤーの足音(右)
    public bool isPlayWalkRSound = false; // このスクリプトでON
    // プレイヤーが突進するときの音
    public bool isPlayRushSound = false; // プレイヤーのtargetスクリプトでON
    // プレイヤーの攻撃音
    public bool isPlayAttackSound = false; // プレイヤーのtargetスクリプトでON
    // プレイヤーの攻撃ヒット音
    public bool isPlayAttackHitSound = false; // プレイヤーのtargetスクリプトでON
    // プレイヤーが敵の攻撃に当たった時の音
    public bool isPlayDamageSound = false; // 敵のAttackCollisionのEnemyDamageでON
    // プレイヤーが死亡した時のジングル
    public bool isPlayDeadSound = false;// 敵のAttackCollisionのEnemyDamageでON
    // プレイヤーのジャンプ音
    public bool isPlayJumpSound = false; // プレイヤーのmove1_ver2スクリプトでON
    // プレイヤーが穴に落ちた時の音
    public bool isPlayFallSound = false; // このスクリプトでON
    // プレイヤーが水に落ちた時の音
    public bool isPlayFallWaterSound = false; // このスクリプトでON

    // プレイヤーのチェイン数
    public int playerChainCount = 0;

    // プレイヤーのtargrtスクリプト
    private target target;
    // プレイヤーのmove1スクリプト
    private move1_ver2 move1;
    // プレイヤーのComboスクリプト
    private Combo combo;

    // 疑似足音実装用
    private bool isCollisionFloor;
    private float moveValue;
    private Vector3 beforeFramePos;
    private string footKind = "L";

    void Start()
    {
        // プレイヤーのtargrtスクリプト取得
        target = GetComponent<target>();
        // プレイヤーのmove1スクリプト取得
        move1 = GetComponent<move1_ver2>();
        // プレイヤーのComboスクリプト取得
        combo = GetComponent<Combo>();
    }

    void Update()
    {
        //試しに疑似足音だけ実装
        if (isCollisionFloor == true)
        {
            moveValue += Vector3.Magnitude(transform.position - beforeFramePos);
        }
        beforeFramePos = transform.position;

        if (moveValue >= 5.0f)
        {
            if (footKind == "L")
            {
                isPlayWalkLSound = true;
                footKind = "R";
            }
            else
            {
                isPlayWalkRSound = true;
                footKind = "L";
            }
            moveValue = 0.0f;
        }

        //コンボ取得
        playerChainCount = combo.ComboCount;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Floor")
        {
            isCollisionFloor = true;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Floor")
        {
            isCollisionFloor = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "FallTrigger")
        {
            isPlayFallSound = true;
        }
        if (other.transform.tag == "WaterTrigger")
        {
            isPlayFallWaterSound = true;
        }
    }
}
