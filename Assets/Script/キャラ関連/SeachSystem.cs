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

    private GameObject Target;
    public GameObject TARGET
    {
        get { return Target; }
    }

    public enum SeachType
    {
        Player,
        Enemy
    }

    public SeachType what;
    private GameObject pre_Obj;
    private float timer = 0.0f , interval = 2.0f;

	//// Update is called once per frame
	//void Update () {

	//}

	void FixedUpdate()
	{
		timer += Time.deltaTime;
		//if (Target == null && Found && timer >= interval)
		//	Found = false;
	}


	void OnTriggerStay(Collider coll)
    {
		if (timer >= interval && coll.gameObject == pre_Obj)
		{
			return;
		}
			
		timer = 0;
        switch (what)
        {
            case SeachType.Player:
                if (coll.gameObject.tag == "Player")
                {
                    Target = coll.gameObject;
                    Found = true;
                }
                break;
            case SeachType.Enemy:
                if (coll.gameObject.tag == "Enemy")
                {
                    Target = coll.gameObject;
                    Found = true;
                }
                break;
        }
		pre_Obj = coll.gameObject;
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "Enemy")
        {
			ClearFunc();
        }
    }

	public void ClearFunc()
	{
		Found = false;
	}
}
