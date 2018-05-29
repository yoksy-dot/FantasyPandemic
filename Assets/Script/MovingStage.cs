using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingStage : MonoBehaviour {

	[SerializeField]
	private List<GameObject> PosList = new List<GameObject>();
	[SerializeField]
	private float Speed;

	private int PosNum = 0;

	// Use this for initialization
	void Start () {
		//GotoNextFunc(PosList);
		transform.position = Vector3.MoveTowards(transform.position, PosList[PosNum].transform.position, Speed);
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(transform.position, PosList[PosNum].transform.position) < 0.4f)
		{
			GotoNextFunc(PosList);
		}
		transform.position = Vector3.MoveTowards(transform.position, PosList[PosNum].transform.position, Speed);
	}

	void GotoNextFunc(List<GameObject> ppp)
	{
		PosNum = (PosNum + 1) % PosList.Count;
	}
}
