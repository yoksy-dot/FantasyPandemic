using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestCtrl 
{
	protected int ID;
	protected int KillEnmey = 0, BreakBuild = 0;
	protected int IN_KillEnmey = 0, IN_BreakBuild = 0;
	protected float QuestTimer = 0;
	protected string Flaver;//全角で9文字まで
	public int KILL
	{
		get { return IN_KillEnmey; }
		set {
			IN_KillEnmey = value;
			if (IN_KillEnmey < 0)
				IN_KillEnmey = 0;
		}
	}
	public int BREAK
	{
		get { return IN_BreakBuild; }
		set { IN_BreakBuild = value;
			if (IN_BreakBuild < 0)
				IN_BreakBuild = 0;
		}
	}
	public string F_TEXT
	{
		get { return Flaver; }
		set { Flaver = value; }
	}

	protected bool ClearFlag;
	//最初ならtrue
	protected bool firsttime = true;

	//public QuestCtrl(int quest_id)
	//{
	//	//コンストラクタ
	//	ID = quest_id;
	//}

	protected abstract void QuestHolder();


	//protected void InitCount()
	//{
	//	//クエストクリア時にカウントの初期化を行う
	//	KillEnmey = BreakBuild = 0;
	//}

	public bool IsQuestClear()
	{
		//外部からクリア状況を呼べるように
		return ClearFlag;
	}

	protected virtual void QuestStartFunc()
	{
		//if (!firsttime) return;
	}
}
