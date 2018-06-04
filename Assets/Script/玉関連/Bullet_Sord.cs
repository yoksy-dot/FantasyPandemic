using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet_Sord : BulletClass
{
	protected override void OnEnable()
	{
		rig = GetComponent<Rigidbody>();
		timer = 0;
	}

	protected void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.GetComponent<BattleObject>() != null)
		{
			//DamageCalculate dam = new DamageCalculate(Attaker, BulletATK, buff1, buff2);
			//coll.gameObject.GetComponent<BattleObject>().ReceiveAttack(dam);
			OnDefeated(coll.gameObject.GetComponent<BattleObject>());
			_battle.MakeObject(1, coll.gameObject.transform.position, coll.gameObject.transform.rotation, "DamegedEffect");
		}
	}
	protected override void OnCollisionEnter(Collision collision)
	{
		
	}


}
