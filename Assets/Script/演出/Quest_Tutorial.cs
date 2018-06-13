using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Quest_Tutorial : QuestCtrl
{
	public Quest_Tutorial(int quest_id)
	{
		//ここのIDは=進行度
		ID = quest_id;
		//IN_KillEnmey = KillEnmey;
		//IN_BreakBuild = BreakBuild;
		//StartCoroutine("QuestHolder",0);
		Timer timer = new Timer(500);

		// タイマーの処理
		timer.Elapsed += (sender, e) =>
		{
			if (!ClearFlag)
			{
				QuestHolder();
			}
			else
			{
				timer.Stop();
			}
		};

		// タイマーを開始する
		timer.Start();
	}

	protected override void QuestHolder()
	{
		switch (ID)
		{
			case 0:
				Flaver = "敵を倒してみよう";
				//Flaver = ID + "";
				KillEnmey = 3;
				BreakBuild = 0;
				QuestStartFunc();
				if (IN_KillEnmey <= 0)
				{
					ID++;
					firsttime = true;
				}
					
				break;
			case 1:
				Flaver = "建物を壊そう";
				BreakBuild = 1;
				KillEnmey = 0;
				QuestStartFunc();
				if (IN_BreakBuild <= 0)
				{
					ID++;
					firsttime = true;
				}
				break;
			case 2:
				Flaver = "敵をもっと倒そう";
				KillEnmey = 3;
				BreakBuild = 0;
				QuestStartFunc();
				if (IN_KillEnmey <= 0)
					ID = 100;
				break;
			case 100://終わりならここに
				ClearFlag = true;
				break;
		}

	}

	protected override void QuestStartFunc()
	{
		if (!firsttime) return;
		IN_KillEnmey = KillEnmey;
		IN_BreakBuild = BreakBuild;
		firsttime = false;
	}
}
