using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakOffSys : MonoBehaviour {
	//スタート時に動くものにつける

		[SerializeField]
	private bool Awaking = false;
	[SerializeField,Tooltip("操作キャラの高さの値")]
	private float FlyPos;
	private GameObject _player;

	// Use this for initialization
	void Start () {
		//if(GetComponentInChildren<PlayerCtrl>() == true)
		//{
		//	Awaking = true;
		//}
		
	}
	
	// Update is called once per frame
	void Update () {
		if(_player == null)
			_player = GameObject.FindGameObjectWithTag("Player").gameObject;

		if (Awaking)
		{
			_player.transform.localPosition = new Vector3(0, 0, 0);
			if (Input.GetButton("Jump"))
			{
				
				_player.transform.parent = null;
				Awaking = false;
			}
		}
	}

}
