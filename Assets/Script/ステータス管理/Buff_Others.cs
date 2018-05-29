using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Others : BuffClass
{
	public enum Kind
	{
		HP,
		MaxHP,
		ATK,
		DEF,
		MaxBullet,
		Interval,
		Reload,
		SPEED,
		JUMP,
		SPECIAL,
		MaxSPECIAL,
		SPECIALInterval,
		Distance
	}

    //固定値上昇分
    [SerializeField]
    private int Maxhp;
    public int MAXHP
    {
        get { return Maxhp; }
        set { Maxhp = value; }
    }
    [SerializeField]
    private int atk;
    public int ATK
    {
        get { return atk; }
        set { atk = value; }
    }
    [SerializeField]
    private int def;
    public int DEF
    {
        get { return def; }
        set { def = value; }
    }
    [SerializeField]
    private int MaxBuulet;//装填数
    public int MAXBULLET
    {
        get { return MaxBuulet; }
        set { MaxBuulet = value; }
    }
    [SerializeField]
    private float interval;//玉の発射間隔
    public float INTERVAL
    {
        get { return interval; }
        set { interval = value; }
    }
    [SerializeField]
    private float Reload;//装填時間
    public float RELOAD
    {
        get { return Reload; }
        set { Reload = value; }
    }
    [SerializeField]
    private float speed;
    public float SPEED
    {
        get { return speed; }
        set { speed = value; }
    }
    [SerializeField]
    private float jump;
    public float JUMP
    {
        get { return jump; }
        set { jump = value; }
    }
    [SerializeField]
    private int spcial;//特殊の残弾
    public int SPECIAL
    {
        get { return spcial; }
        set { spcial = value; }
    }
    [SerializeField]
    private int Maxspcial;//特殊の最大値
    public int MAXSPECIAL
    {
        get { return Maxspcial; }
        set { Maxspcial = value; }
    }
    [SerializeField]
    private float spcial_interval;//特殊がたまるまでの時間
    public float SPECIALINTERVAL
    {
        get { return spcial_interval; }
        set { spcial_interval = value; }
    }
    [SerializeField]
    private float distance;//射程?
    public float DISTANCE
    {
        get { return distance; }
        set { distance = value; }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //バフ同士の計算
    public static Buff_Others operator +(Buff_Others a, Buff_Others b)
    {
        Buff_Others result = new Buff_Others();

        result.HP = a.HP + b.HP;
        result.MAXHP = a.MAXHP + b.MAXHP;//これいる？
        result.ATK = a.ATK + b.ATK;
        result.DEF = a.DEF + b.DEF;
        result.MAXBULLET = a.MAXBULLET + b.MAXBULLET;
        result.INTERVAL = a.INTERVAL + b.INTERVAL;
        result.RELOAD = a.RELOAD + b.RELOAD;
        result.SPEED = a.SPEED + b.SPEED;
        result.JUMP = a.JUMP + b.JUMP;
        result.SPECIAL = a.SPECIAL + b.SPECIAL;
        result.MAXSPECIAL = a.MAXSPECIAL + b.MAXSPECIAL;
        result.SPECIALINTERVAL = a.SPECIALINTERVAL + b.SPECIALINTERVAL;
        result.DISTANCE = a.DISTANCE + b.DISTANCE;

        return result;
    }
}
