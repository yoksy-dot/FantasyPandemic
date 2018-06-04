using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletClass : MonoBehaviour
{

    [SerializeField]
    protected int BulletATK;
    public int BULLETATK
    {
        get { return BulletATK; }
    }

	protected BattleObject Attaker;
    public BattleObject ATTACKER
    {
        set { Attaker = value; }
    }

    protected float FlyPower;
    public float POWER
    {
        set { FlyPower = value; }
    }

	[SerializeField]
    protected Rigidbody rig;

    protected Buff_HPSP buff1;
    public Buff_HPSP BUFF1
    {
        get { return buff1; }
        set { buff1 = value; }
    }

    protected Buff_Others buff2;
    public Buff_Others BUFF2
    {
        get { return buff2; }
        set { buff2 = value; }
    }

	protected BattleSystem _battle;
	public BattleSystem SYS
	{
		set { _battle = value; }
	}


    //消滅時間
    [SerializeField]
    protected float BreakTime;
    protected float timer;

	[SerializeField]
	protected ParticleSystem _particleSystem;

	//void Awake()
	//{
	//	if (_battle == null)
	//		_battle = GameObject.Find("ObjectCtrl").GetComponent<BattleSystem>();
	//}

	protected virtual void OnEnable()
	{
		//rig = GetComponent<Rigidbody>();
		//transform.rotation = Quaternion.identity;
		rig.AddForce(FlyPower * transform.forward, ForceMode.Impulse);
		timer = 0;

		if (_particleSystem)
			_particleSystem.Stop();
	}

    // Update is called once per frame
    protected virtual void Update () {
        timer += Time.deltaTime;
        if (BreakTime <= timer)
        {
            DestroyFunc();
			//Destroy(gameObject);
			gameObject.SetActive(false);
			rig.velocity = Vector3.zero;
			rig.angularVelocity = Vector3.zero;
		}

	}

	protected virtual void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.GetComponent<BattleObject>() != null)
        {
			OnDefeated(coll.gameObject.GetComponent<BattleObject>());
			_battle.MakeObject(1, coll.gameObject.transform.position, coll.gameObject.transform.rotation, "DamegedEffect");

			DestroyFunc();
			gameObject.SetActive(false);
			rig.velocity = Vector3.zero;
			rig.angularVelocity = Vector3.zero;
		}
        if (coll.gameObject.tag == "Ground")
        {
            DestroyFunc();
			gameObject.SetActive(false);
			rig.velocity = Vector3.zero;
			rig.angularVelocity = Vector3.zero;
		}
    }

    //消滅前に行う処理
    protected virtual void DestroyFunc()
    {
		if (_particleSystem)
			_particleSystem.Stop();
	}

	//イベント
	protected void OnDefeated(BattleObject ukemi)
	{
		// ダメージを与えた通知
		Eventbus.Instance.NotifyDamegedObj(Attaker, ukemi, BulletATK, buff1, buff2);
	}
}
