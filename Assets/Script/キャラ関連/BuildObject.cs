using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObject : BattleObject
{
	[SerializeField, Tooltip("敵などを生産するかどうか")]
	private bool UseMaker;

	[SerializeField, Tooltip("生産ID")]
	private int makeID;

	[SerializeField, Tooltip("何を生産するか")]
	private GameObject Prefabs;

	[SerializeField, Tooltip("生産距離(同心円)")]
	private float MakeDistance;

	[SerializeField]
	private float Interval;

	[SerializeField, Tooltip("デバッグ用生産回数")]
	private int DebugMakeNum;

	

	float timer2 = 0;

	int debugnum =0;

	// Use this for initialization
	protected override void Start () {
		base.Start();
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update();
		timer2 += Time.deltaTime;

		//if (Input.GetKeyDown(KeyCode.P))
		//{
		//	UseMaker = false;
		//	Debug.Log("数は" + debugnum + "です");
		//}

		if(UseMaker && timer2 >= Interval && DebugMakeNum > debugnum)
		{
			timer2 = 0;
			Maker(makeID); debugnum++;
		}
	}

	void Maker(int id)
	{
		Vector2 xy = Random.insideUnitCircle * MakeDistance;
		GameObject ene = _battleSystem.MakeObject(id, new Vector3(xy.x + transform.position.x, transform.position.y, xy.y + transform.position.z), transform.rotation);
		//GameObject ene = Instantiate(Prefabs, new Vector3(xy.x + transform.position.x, transform.position.y, xy.y + transform.position.z), transform.rotation);
	}
}
