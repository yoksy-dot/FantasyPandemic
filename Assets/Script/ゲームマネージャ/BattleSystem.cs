using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class BattleSystem : MonoBehaviour
{

	[Tooltip("シーン内のMaxオブジェクト数")]
	private static int ObjNum = 40;
	//初期配置以外のバトルオブジェクトのリスト
	[Tooltip("種類")]
	public GameObject[] ObjKind = new GameObject[10];
	//生産済みオブジェクトのリスト
	public GameObject[,] ObjArray = new GameObject[10, ObjNum];
	//オブジェクトのステータス
	public Parameter[,] Pararray = new Parameter[10, ObjNum];
	private int[] Current_O = new int[10];

	private BattleObject b_obj;
	//private BulletClass bullet_obj;

	//そのHPリスト
	private int[] HPArray = new int[100];

	private StringBuilder sb = new StringBuilder();

	private DamageCalculate dam = new DamageCalculate();

	public void Awake() {
		//オブジェクト生産
		for (int i = 0; i < ObjKind.Length; i++)
		{
			for (int j = 0; j < ObjNum; j++)//とりま10
			{
				ObjArray[i,j] = Instantiate(ObjKind[i], transform) as GameObject;
				if (ObjArray[i, j].GetComponent<BattleObject>())
				{
					b_obj = ObjArray[i, j].GetComponent<BattleObject>();
					Pararray[i, j] = b_obj.PARAMETER;//パラメータ
					b_obj.ID = 10 * i + j;//ID振る
				}
				//if (ObjArray[i, j].GetComponent<BulletClass>())
				//{
				//	bullet_obj = ObjArray[i, j].GetComponent<BulletClass>();
				//	bullet_obj.SYS = this;

				//}
				ObjArray[i,j].SetActive(false);
			}
		}
	}


	// Use this for initialization
	void Start () {
		// 通知受け取り登録
		Eventbus.Instance.Subscribe((Eventbus.OnDamegedObj)OnDamegedObject);
	}
	
	//// Update is called once per frame
	//void Update () {
		

	//}

	void OnDamegedObject(BattleObject Attacker, BattleObject Diffender, int BulletAtk, Buff_HPSP buff1, Buff_Others buff2)
	{
		//Debug.Log("痛い!");
		//DamageCalculate dam = new DamageCalculate(Attacker, BulletAtk, buff1, buff2);
		dam.NewCalculateFunc(Attacker, BulletAtk, buff1, buff2);
		Diffender.ReceiveAttack(dam);

		// 通知受け取り解除
		//Eventbus.Instance.Unsubscribe((Eventbus.OnDamegedObj)OnDamegedObject);
	}

	//使用するオブジェクトID,Position,Rotetion
	//オブジェクトをその場所に呼び出す関数
	public GameObject MakeObject(int prefabsID,Vector3 pos ,Quaternion rot,string Rename)
	{
		sb.Append(ObjArray[prefabsID, Current_O[prefabsID]].name);
		if (transform.Find(sb.ToString()) != null)
			return null;
		sb.Length=0;
		ObjArray[prefabsID,Current_O[prefabsID]].transform.position = pos;
		//ObjArray[prefabsID,Current_O[prefabsID]].transform.localRotation = rot;
		ObjArray[prefabsID,Current_O[prefabsID]].SetActive(true);
		sb.Append(Rename);
		sb.Append(prefabsID);
		sb.Append(":" );
		sb.Append(Current_O[prefabsID]);

		ObjArray[prefabsID, Current_O[prefabsID]].name = sb.ToString();
		if (Current_O[prefabsID] == ObjNum - 1)
		{
			Current_O[prefabsID] = 0;
			sb.Length = 0;
			return ObjArray[prefabsID, ObjNum-1]; 
		}
		else
			return ObjArray[prefabsID, Current_O[prefabsID]++];
	}
}
