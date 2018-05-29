using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Elephant : PlayerCtrl
{
	protected override void SPShoot_Down()
	{
		base.SPShoot_Down();
		AnimCtrl[0].SetTrigger("SP");
	}
}
