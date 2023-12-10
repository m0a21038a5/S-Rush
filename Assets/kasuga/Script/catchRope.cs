using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catchRope : MonoBehaviour
{
	//プレイヤーの親をロープにするスクリプト
	[SerializeField]
	private Transform arrivalPoint;

	public bool freezePos = false;
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
		{
			//　キャラクターの親をロープにする
			col.transform.SetParent(transform);
			//FreezePositionのオンとオフを切り替える変数
			freezePos = true;
		}
	}
}
