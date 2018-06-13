using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SeachSystem : MonoBehaviour {
	//要レイヤー確認

    private bool Found;
    public bool FOUND
    {
        get { return Found; }
    }

	private Vector3 target;
	public Vector3 TARGET {
		get { return target; }
		set { target = value; }
	}


	private GameObject pp;
	//public enum SeachType
	//{
	//    Player,
	//    Enemy
	//}
	[Tooltip("Playerの創作ならTrueに")]
	public bool isPlayer;

   // public SeachType what;
    private float timer = 0.0f , interval = 2.0f;


	void FixedUpdate()
	{
		timer += Time.deltaTime;
		//if (Target == null && Found && timer >= interval)
		//	Found = false;
	}


	void OnTriggerStay(Collider coll)
    {
		if (timer <= interval)
		{
			return;
		}

		timer = 0;

		if (isPlayer)
		{
			if (coll.gameObject.tag == "Player")
			{
				target = coll.transform.position;//GCAllocの原因
				Found = true;
			}
		}
		else
		{
			if (coll.gameObject.tag == "Enemy")
			{
				target = coll.transform.position;
				Found = true;
			}
		}
	}

    void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "Enemy")
        {
			ClearFunc();
        }
    }

	private void ClearFunc()
	{
		Found = false;
	}
}
