using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPos_Class : MonoBehaviour {

	[SerializeField,Tooltip("降下作戦用")]
	private bool RindOn = false;

	private GameObject _player;

	// Use this for initialization
	void Start () {
		GameManager.Instantiate._StartPos = GameObject.FindGameObjectsWithTag("START");
		
			
	}
	
	// Update is called once per frame
	void Update () {
		if (RindOn)
		{
			_player = GameObject.FindGameObjectWithTag("Player").gameObject;
			_player.transform.parent = GameManager.Instantiate.START_POS;
			RindOn = false;
		}
	}
}
