using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Zombie : EnemyClass
{

	//死んだとき
	protected override void DeathFunc()
	{

		//base.DeathFunc();
		if (AnimCtrl[0])
		{
			AnimCtrl[0].SetTrigger("Dead");

			if (AnimCtrl[0].GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.Dead") && AnimCtrl[0].GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
			{
				gameObject.SetActive(false);
				if (Important)
					GameManager.Instantiate.KILL_IMP++;
				GameManager.Instantiate.KILL++;
				StopCoroutine("ApplyReceiveBuff");
				AwakeBuff_HP.Clear();
				AwakeBuff_N.Clear();
			}
			
		}
	}

	//移動時に呼ばれる
	protected override void Ani_MoveFunc()
	{
		if (AnimCtrl[0])
		{
			AnimCtrl[0].SetBool("Move", true);
		}
		
	}
	//攻撃
	protected override void Ani_AtkFunc()
	{
		if (AnimCtrl[0])
		{
			AnimCtrl[0].SetTrigger("Attack");
		}
	}
	protected override void Ani_WaitFunc()
	{
		if (AnimCtrl[0])
		{
			AnimCtrl[0].SetBool("Move", false);
		}
	}
}
