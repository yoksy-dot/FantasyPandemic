using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Zombie : EnemyClass
{

	//死んだとき
	protected override void DeathFunc()
	{
		base.DeathFunc();
		if (AnimCtrl[0])
		{
			AnimCtrl[0].SetTrigger("Dead");
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
