using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Particle : BulletClass
{

	protected override void OnEnable()
	{
		timer = 0;
	}

	// Update is called once per frame
	//void Update () {
		
	//}

	//protected override void DestroyFunc()
	//{
	//	if (_particleSystem)
	//		_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
	//}

	protected override void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject.GetComponent<BattleObject>() != null)
		{
			OnDefeated(coll.gameObject.GetComponent<BattleObject>());


			//DestroyFunc();
			//gameObject.SetActive(false);
			//rig.velocity = Vector3.zero;
			//rig.angularVelocity = Vector3.zero;
		}
		if (coll.gameObject.tag == "Ground")
		{
			//DestroyFunc();
			//gameObject.SetActive(false);
			//rig.velocity = Vector3.zero;
			//rig.angularVelocity = Vector3.zero;
		}
	}
}
