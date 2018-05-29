using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour {

	public Text kill, _Time;
	public Text Winner;

	private float timer = 0;

	// Use this for initialization
	void Start () {
		if (GameManager.Instantiate.WIN)
			Winner.text = "Winner!";
		else
			Winner.text = "Lose...";

		kill.text = "KILL SCORE :" + GameManager.Instantiate.KILL;
		_Time.text = "TIME :" + GameManager.Instantiate.FINTIME;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer >= 2.0f && Input.GetMouseButtonDown(0))
			GameManager.Instantiate.SceneChengeManager(0);
	}
}
