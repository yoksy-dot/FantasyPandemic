using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eventbus
{

	// シングルトンにする
	static Eventbus instance;
	public static Eventbus Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new Eventbus();
			}
			return instance;
		}
	}
	private Eventbus()
	{
	}

	// 通知受け取りデリゲート定義
	public delegate void OnDamegedObj(BattleObject Attacker, BattleObject Diffender,int BulletAtk, Buff_HPSP buff1, Buff_Others buff2);
	event OnDamegedObj _OnDamegedObj;

	// 通知受け取り登録
	public void Subscribe(OnDamegedObj onDameged)
	{
		_OnDamegedObj += onDameged;
	}

	// 通知受け取り解除
	public void Unsubscribe(OnDamegedObj onDameged)
	{
		_OnDamegedObj -= onDameged;
	}

	// 通知実行
	public void NotifyDamegedObj(BattleObject Attacker, BattleObject Diffender, int BulletAtk, Buff_HPSP buff1, Buff_Others buff2)
	{
		
		if (_OnDamegedObj != null)
		{
			_OnDamegedObj(Attacker, Diffender,BulletAtk, buff1, buff2);
		}
			
	}
}
