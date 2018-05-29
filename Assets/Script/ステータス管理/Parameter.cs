using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameter  {
    //MonoBehaviourを継承しないクラス
    //各種パラメータの型を持つクラス
    //演算子オーバーロードできる

    private int hp;
    public int HP
    {
        get { return hp; }
        set
        {
            hp = value;
			if (hp <= 0)
				hp = 0;
			else if (hp > Maxhp)
				hp = Maxhp;
		}
    }

    private int Maxhp;
    public int MAXHP
    {
        get { return Maxhp; }
        set { Maxhp = value; }
    }

    //基本的に0,しかしバフで上昇することがある
    private int atk;
    public int ATK
    {
        get { return atk; }
        set { atk = value; }
    }

    //基本的に0,しかしバフで上昇することがある
    private int def;
    public int DEF
    {
        get { return def; }
        set { def = value; }
    }

    private int MaxBuulet;//装填数
    public int MAXBULLET
    {
        get { return MaxBuulet; }
        set { MaxBuulet = value; }
    }

    private float interval;//玉の発射間隔
    public float INTERVAL
    {
        get { return interval; }
        set { interval = value; }
    }

    private float Reload;//装填時間
    public float RELOAD
    {
        get { return Reload; }
        set { Reload = value; }
    }

    private float speed;
    public float SPEED
    {
        get { return speed; }
        set { speed = value; }
    }

    private float jump;
    public float JUMP
    {
        get { return jump; }
        set { jump = value; }
    }

    private int spcial;//特殊の残弾
    public int SPECIAL
    {
        get { return spcial; }
        set
		{
			spcial = value;
			if (spcial > Maxspcial)
				spcial = Maxspcial;

		}
    }

    private int Maxspcial;//特殊の最大値
    public int MAXSPECIAL
    {
        get { return Maxspcial; }
        set { Maxspcial = value; }
    }

    private float spcial_interval;//特殊がたまるまでの時間
    public float SPECIALINTERVAL
    {
        get { return spcial_interval; }
        set { spcial_interval = value; }
    }

    private float distance;//射程?
    public float DISTANCE
    {
        get { return distance; }
        set { distance = value; }
    }


    //ここに演算子オーバーロードを書こう
    public static Parameter operator +(Parameter a, Parameter b)//使わないかも
    {
        Parameter result = new Parameter();

		result.MAXHP = a.MAXHP + b.MAXHP;//これいる？
		result.MAXSPECIAL = a.MAXSPECIAL + b.MAXSPECIAL;
		result.HP = a.HP + b.HP;
        result.ATK = a.ATK + b.ATK;
        result.DEF = a.DEF + b.DEF;
        result.MAXBULLET = a.MAXBULLET + b.MAXBULLET;
        result.INTERVAL = a.INTERVAL + b.INTERVAL;
        result.RELOAD = a.RELOAD + b.RELOAD;
        result.SPEED = a.SPEED + b.SPEED;
        result.JUMP = a.JUMP + b.JUMP;
        result.SPECIAL = a.SPECIAL + b.SPECIAL;
        result.SPECIALINTERVAL = a.SPECIALINTERVAL + b.SPECIALINTERVAL;
        result.DISTANCE = a.DISTANCE + b.DISTANCE;

        return result;
    }

    public static Parameter operator +(Parameter a, Display_Parameter b)//ディスプレイとの計算
    {
        Parameter result = new Parameter();

		result.MAXHP = a.MAXHP + b.Maxhp;//これいる？
		result.MAXSPECIAL = a.MAXSPECIAL + b.Maxspcial;
		result.MAXBULLET = a.MAXBULLET + b.MaxBuulet;
		result.HP = a.HP + b.hp;
        result.ATK = a.ATK + b.atk;
        result.DEF = a.DEF + b.def;
        result.INTERVAL = a.INTERVAL + b.interval;
        result.RELOAD = a.RELOAD + b.Reload;
        result.SPEED = a.SPEED + b.speed;
        result.JUMP = a.JUMP + b.jump;
        result.SPECIAL = a.SPECIAL + b.spcial;
        result.SPECIALINTERVAL = a.SPECIALINTERVAL + b.spcial_interval;
        result.DISTANCE = a.DISTANCE + b.distance;

        return result;
    }

    //バフとの計算
    public static Parameter operator +(Parameter a, Buff_Others b)
    {
        Parameter result = new Parameter();

		result.MAXHP = a.MAXHP + b.MAXHP;//これいる？
		result.MAXSPECIAL = a.MAXSPECIAL + b.MAXSPECIAL;
		result.HP = a.HP + b.HP;
        result.ATK = a.ATK + b.ATK;
        result.DEF = a.DEF + b.DEF;
        result.MAXBULLET = a.MAXBULLET + b.MAXBULLET;
        result.INTERVAL = a.INTERVAL + b.INTERVAL;
        result.RELOAD = a.RELOAD + b.RELOAD;
        result.SPEED = a.SPEED + b.SPEED;
        result.JUMP = a.JUMP + b.JUMP;
        result.SPECIAL = a.SPECIAL + b.SPECIAL;
        result.SPECIALINTERVAL = a.SPECIALINTERVAL + b.SPECIALINTERVAL;
        result.DISTANCE = a.DISTANCE + b.DISTANCE;

        return result;
    }

    public static Parameter operator -(Parameter a, Buff_Others b)//引き算
    {
        Parameter result = new Parameter();

		result.MAXHP = a.MAXHP - b.MAXHP;//これいる？
		result.MAXSPECIAL = a.MAXSPECIAL - b.MAXSPECIAL;
		result.HP = a.HP - b.HP;
        result.ATK = a.ATK - b.ATK;
        result.DEF = a.DEF - b.DEF;
        result.MAXBULLET = a.MAXBULLET - b.MAXBULLET;
        result.INTERVAL = a.INTERVAL - b.INTERVAL;
        result.RELOAD = a.RELOAD - b.RELOAD;
        result.SPEED = a.SPEED - b.SPEED;
        result.JUMP = a.JUMP - b.JUMP;
        result.SPECIAL = a.SPECIAL - b.SPECIAL;
        result.SPECIALINTERVAL = a.SPECIALINTERVAL - b.SPECIALINTERVAL;
        result.DISTANCE = a.DISTANCE - b.DISTANCE;

        return result;
    }

    //リジェネ分
    public static Parameter operator +(Parameter a, Buff_HPSP b)
    {
        Parameter result = new Parameter();

        result.HP = a.HP + b.HP;

        return result;
    }

}
