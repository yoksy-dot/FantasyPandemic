using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSystem : MonoBehaviour
{

    //0.01秒ごとの拡大値
    private float Expansion;
    //最大拡大値
    private float MaxExpansion;
    //爆風の攻撃力
    private int BombAtk;
    //攻撃者
    private BattleObject Attaker;

    public float EXP
    {
        set { Expansion = value; }
    }
    public float MAX_EXP
    {
        set { MaxExpansion = value; }
    }

    public int ATK
    {
        set { BombAtk = value; }
    }
    public BattleObject ATACKER
    {
        set { Attaker = value; }
    }
    
    Buff_HPSP buff1;
    Buff_Others buff2;

    private float timer = 0;
    private Vector3 nowScale;

	[SerializeField]
	private ParticleSystem _particleSystem;

    // Use this for initialization
    void Start()
    {
        transform.localScale = nowScale = new Vector3(0.1f, 0.1f, 0.1f);
		//if (_particleSystem)
		//	_particleSystem.transform.localPosition = transform.localPosition;

	}

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.localScale = nowScale;
        if (timer >= 0.01f)
        {
            nowScale = new Vector3(nowScale.x += Expansion, nowScale.y += Expansion, nowScale.z += Expansion);
            timer = 0;
            if (nowScale.x >= MaxExpansion)//最大値に達すると消滅
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.GetComponent<BattleObject>() != null &&
			Attaker.GROUP != coll.gameObject.GetComponent<BattleObject>().GROUP)
        {
			//DamageCalculate dam = new DamageCalculate(Attaker, BombAtk, buff1, buff2);
			//coll.gameObject.GetComponent<BattleObject>().ReceiveAttack(dam);
			OnDefeated(coll.gameObject.GetComponent<BattleObject>());
		}
    }

	protected void OnDefeated(BattleObject ukemi)
	{
		// ダメージを与えた通知
		Eventbus.Instance.NotifyDamegedObj(Attaker, ukemi, BombAtk, buff1, buff2);
	}
}
