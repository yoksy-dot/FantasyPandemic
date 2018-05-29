using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculate
{
    private int AttackPower;//実際の与ダメ(防御減退前)
    public int ATKPOWER
    {
        get { return AttackPower; }
    }

    //環境や爆風ダメなどの固定ダメージ格納
    int BulletATK;

    Buff_HPSP _sendBuff1;//与バフリスト
    Buff_Others _sendBuff2;

    BattleObject _status;//ステータス

    //与バフリストを取得
    public Buff_HPSP SendBuff1
    {
        get { return _sendBuff1; }
    }
    public Buff_Others SendBuff2
    {
        get { return _sendBuff2; }
    }

    //コンストラクタ
    public DamageCalculate(/*BattleObject Attaker_status ,int Attack_Other, Buff_HPSP sendBuff1,Buff_Others sendBuff2*/)
    {
        //_status = Attaker_status;
        
        //BulletATK = Attack_Other;

        //if (sendBuff1 != null)
        //{
        //    _sendBuff1 = sendBuff1;
        //}
        //if (sendBuff2 != null)
        //{
        //    //Debug.Log("ggg");
        //    _sendBuff2 = sendBuff2;
        //}

        //CalculateAttackPower();//ダメージ計算

    }

	public void NewCalculateFunc(BattleObject Attaker_status, int Attack_Other, Buff_HPSP sendBuff1, Buff_Others sendBuff2)
	{
		_status = Attaker_status;

		BulletATK = Attack_Other;

		if (sendBuff1 != null)
		{
			_sendBuff1 = sendBuff1;
		}
		if (sendBuff2 != null)
		{
			//Debug.Log("ggg");
			_sendBuff2 = sendBuff2;
		}

		CalculateAttackPower();//ダメージ計算
	}

	//実ダメージ計算
	private void CalculateAttackPower()
    {
        int Buff_ATK = 0;//仮置き場

        if (_status.AWAKE_N.Count > 0)
        {
            foreach (Buff_Others s in _status.AWAKE_N)//起動中のバフの攻撃増加をすべて足し合わせる
                Buff_ATK += s.ATK;

            AttackPower = _status.PARAMETER.ATK + BulletATK + Buff_ATK;
        }
        else
            AttackPower = _status.PARAMETER.ATK + BulletATK;


        //Debug.Log("ALLATK" + AttackPower);

    }
}
