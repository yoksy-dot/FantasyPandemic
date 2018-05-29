using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Grenade : PlayerCtrl
{
	[SerializeField]
	private Material GranadeMat;
	private GameObject[] sphere = new GameObject[100];


	[SerializeField]
	private int FlagPoint = 10;


	// Use this for initialization
	protected override void Start()
	{
		base.Start();
		//test_Sp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		//test_Sp.GetComponent<SphereCollider>().enabled = false;//当たり判定無効化
		//MeshRenderer render = test_Sp.GetComponent<MeshRenderer>();//レンダ取得
		//render.material.color = new Color(137, 46, 163, 0.4f);//透明で紫っぽい色

		MakePredictionLine(_parameter.DISTANCE, SPShootPoint);
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
		
	}

	//private void FixedUpdate()
	//{
	//	if (ShootFlag2)
			
	//}

	protected override void SPShoot_Down()
	{
		//base.SPShoot_Down();

		for (int i = 0; i < FlagPoint; i++)
		{
			sphere[i].SetActive(true);
		}
		Math_Pos(_parameter.DISTANCE, SPShootPoint);
	}

	protected override void SPShoot_UP()
	{
		//base.SPShoot_UP();
		//for (int i = 0; i < FlagPoint; i++)
		//	Destroy(sphere[i].gameObject);
		for (int i = 0; i < FlagPoint; i++)
		{
			sphere[i].SetActive(false);
		}
		ShootFlag2 = true;
		timer = 0;


	}

	void MakePredictionLine(float Power, Transform Start)
	{
		float t = 0;
		Vector3 aaa = Start.localEulerAngles;
		aaa.x = 0;
		for (int i = 0; i < FlagPoint; i++)
		{
			//t += Time.fixedDeltaTime;
			float x = ((Power * i) * Start.forward.x);
			float y = ((Power * t) * Start.forward.y) - 0.5f * (-Physics.gravity.y) * t * t;
			float z = ((Power * i) * Start.forward.z);
			sphere[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere[i].transform.parent = Start;
			//sphere[i].transform.rotation = Quaternion.Euler(0, 0, 0);
			sphere[i].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
			sphere[i].transform.position = Start.position + new Vector3(x, y, z);
			sphere[i].GetComponent<SphereCollider>().enabled = false;
			sphere[i].GetComponent<Renderer>().material = GranadeMat;
			sphere[i].SetActive(false);



		}
	}

	void Math_Pos(float Power, Transform Start)
	{
		float t = 0;
		Vector3 aaa = Start.position;
		aaa.x = 0;
		for (int i = 0; i < FlagPoint; i++)
		{
			t = (float)i / 30;
			float x = ((Power * t) * Start.forward.x);
			float y = ((Power * t) * Start.forward.y) - 0.5f * (-Physics.gravity.y) * t * t;
			float z = ((Power * t) * Start.forward.z);
			sphere[i].transform.position = Start.position + new Vector3(x, y, z) ;
		}
		
	}
}
