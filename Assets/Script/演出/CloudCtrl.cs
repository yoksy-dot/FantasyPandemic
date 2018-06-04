using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCtrl : MonoBehaviour {

	[SerializeField]
	private float Str = 0.0f;
	[SerializeField]
	private float Speed = 0.0f;
	[SerializeField, Tooltip("初期地点ワープまでの時間")]
	private int EndTime = 1;

	private Vector3 stratpos;
	private Material _material;
	private float timer =0;

	// Use this for initialization
	void Start () {
		_material = GetComponent<MeshRenderer>().material;
		_material.SetFloat("Strength", Str);
		stratpos = transform.position;
	}
	
	//// Update is called once per frame
	//void Update () {
		
	//}
	private void FixedUpdate()
	{
		timer++;
		transform.Translate(transform.forward * Speed);
		if (timer >= EndTime)
		{
			transform.position = stratpos;
			timer = 0;
		}
			
	}
}
