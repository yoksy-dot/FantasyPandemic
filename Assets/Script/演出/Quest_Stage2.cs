using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Quest_Stage2 : QuestCtrl
{
	public Quest_Stage2(int quest_id)
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
				Flaver = "少しでも倒そう";
				KillEnmey = 5;
				BreakBuild = 2;
				QuestStartFunc();
				if (IN_KillEnmey <= 0)
				{
					ID++;
					firsttime = true;
				}

				break;
			case 1:
				Flaver = "原因は何かな";
				KillEnmey = 99;
				BreakBuild = 99;
				QuestStartFunc();
				if (IN_BreakBuild <= 0)
				{
					ID = 100;
					firsttime = true;
				}
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
