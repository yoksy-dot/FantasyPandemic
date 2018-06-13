using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[RequireComponent(typeof(Display_Parameter))]
//[RequireComponent(typeof(CharacterController))]
public class BattleObject : MonoBehaviour
{
	//殴れて壊れるオブジェクトには全部これがつく感じで
	//ディスプレイは必須
	[Tooltip("インスペクタでは触らない")]
	public int ID;//生産時に決定

	protected BattleSystem _battleSystem;

	[SerializeField, Tooltip("撃破条件の物")]
	protected bool Important;

	//public enum Group
	//{
	//	RED,
	//	BULE,
	//	NPC,
	//	RED_BUILD,
	//	BULE_BUILD
	//}
	//[SerializeField]
	//private Group _Group;
	//public Group GROUP
	//{
	//	get { return _Group; }
	//}

	protected Parameter _parameter = new Parameter();//ステータス
	public Parameter PARAMETER
	{
		get { return _parameter; }
	}
	//バフの種別判定用
	private Buff_Others.Kind _kind;

	protected List<Buff_Others> AwakeBuff_N = new List<Buff_Others>();//今自分についているステ変化バフ
	protected List<Buff_HPSP> AwakeBuff_HP = new List<Buff_HPSP>();//今自分についている時間ごとの体力増減バフ
																   //付与するバフのプレハブ
																   //[SerializeField]
																   //protected GameObject Buff_Prefabs;
	protected Buff_HPSP SendHP;
	protected Buff_Others SendO;

	public List<Buff_Others> AWAKE_N
	{
		get { return AwakeBuff_N; }
	}
	public List<Buff_HPSP> AWAKE_HP
	{
		get { return AwakeBuff_HP; }
	}


	//発射する弾
	[SerializeField]
	protected GameObject Bullet;
	//玉の発射口
	[SerializeField]
	protected Transform ShootPoint;
	//弾の発射時に照準がぶれる
	//1でもまあまあ影響ある
	[SerializeField, Tooltip("数値が高いと弾の発射時にぶれる.0はぶれない.ミサイル装備時には使用しないように")]
	protected float IsRandomShoot = 0.0f;
	//特殊
	[SerializeField]
	protected GameObject SPBullet;
	//特殊の発射口
	[SerializeField]
	protected Transform SPShootPoint;

	//ミサイル弾用の変数
	protected GameObject TargetObj;
	//近接用変数
	protected GameObject SordB, SordB2;

	//アニメーター
	[SerializeField]
	protected Animator[] AnimCtrl = new Animator[3];

	[SerializeField, Tooltip("パーティクル")]
	protected ParticleSystem _particleSystem;

	protected float timer = 0;

	//事前生産
	[SerializeField, Tooltip("弾の生産数")]
	protected int[] MakeNum = new int[2];
	protected GameObject[][] BulletArray = new GameObject[2][];
	protected int[] Current_B = new int[2];
	protected StringBuilder sb2 = new StringBuilder();
	private int[] _id = new int[2];

	//死亡関数が一回だけ呼ばれるようにするためのフラグ
	protected bool deathFlag = false;

	protected virtual void OnEnable()
	{
		deathFlag = false;
	}

	private void Awake()
	{
		if (_battleSystem == null)
			_battleSystem = GameObject.Find("ObjectCtrl").GetComponent<BattleSystem>();

		_id[0] = ID % 10;//1桁目(1)
		ID /= 10;
		_id[1] = ID % 10;//2桁目(10)

		if (_particleSystem)
			_particleSystem.Stop();
	}

	// Use this for initialization
	protected virtual void Start()
	{
		_parameter += _parameter + GetComponent<Display_Parameter>();
		//_battleSystem.Pararray[_id[1], _id[0]] += _battleSystem.Pararray[_id[1], _id[0]] + GetComponent<Display_Parameter>();

		if (GetComponent<Buff_HPSP>())
			SendHP = GetComponent<Buff_HPSP>();
		if (GetComponent<Buff_Others>())
			SendO = GetComponent<Buff_Others>();

		StartCoroutine("ApplyReceiveBuff");//バフ処理

		if(Bullet && Bullet.GetComponent<Bullet_Sord>())//近接攻撃があれば
		{
			SordB = Instantiate(Bullet, transform);
			SordB.SetActive(false);
		}
		else if (SPBullet && SPBullet.GetComponent<Bullet_Sord>())
		{
			SordB2 = Instantiate(SPBullet, transform);
			SordB2.SetActive(false);
		}

		//事前生産
		if (Bullet)
		{
			BulletArray[0] = new GameObject[MakeNum[0]];
			//弾
			for (int j = 0; j < BulletArray[0].Length; j++)//とりま10
			{
				BulletArray[0][j] = Instantiate(Bullet, _battleSystem.transform) as GameObject;
				BulletClass b_bullet = BulletArray[0][j].GetComponent<BulletClass>();
				b_bullet.ATTACKER = this;
				b_bullet.POWER = _parameter.DISTANCE;
				b_bullet.SYS = _battleSystem;
				if (SendHP) b_bullet.BUFF1 = SendHP;
				if (SendO) b_bullet.BUFF2 = SendO;
				//ミサイルはしばらく封印
				//if (BulletArray[0][j].GetComponent<Bullet_Missile>() && TargetObj != null)
				//{
				//	BulletArray[0][j].GetComponent<Bullet_Missile>().Missile_TARGET = TargetObj;
				//}
				BulletArray[0][j].SetActive(false);
			}
		}
		if (SPBullet)
		{
			BulletArray[1] = new GameObject[MakeNum[1]];
			//弾
			for (int j = 0; j < BulletArray[1].Length; j++)//とりま10
			{
				BulletArray[1][j] = Instantiate(SPBullet, _battleSystem.transform) as GameObject;
				BulletClass b_bullet = BulletArray[1][j].GetComponent<BulletClass>();
				b_bullet.ATTACKER = this;
				b_bullet.POWER = _parameter.DISTANCE;
				b_bullet.SYS = _battleSystem;
				if (SendHP) b_bullet.BUFF1 = SendHP;
				if (SendO) b_bullet.BUFF2 = SendO;
				//if (BulletArray[1][j].GetComponent<Bullet_Missile>() && TargetObj != null)
				//{
				//	BulletArray[1][j].GetComponent<Bullet_Missile>().Missile_TARGET = TargetObj;
				//}
				BulletArray[1][j].SetActive(false);
			}
		}
	}

	// Update is called once per frame
	protected virtual void Update()
	{
		timer += Time.deltaTime;

		if (_parameter.HP <= 0)
		{
			//deathFlag = true;
			DeathFunc();
			if (!deathFlag)
			{
				deathFlag = true;
				FirstDeathFunc();
			}
		}
	}

	//死んだときの関数
	protected virtual void DeathFunc()
	{
		//Destroy(gameObject);
		gameObject.SetActive(false);
		if (Important)
			GameManager.Instantiate.KILL_IMP++;
		GameManager.Instantiate.KILL++;
		StopCoroutine("ApplyReceiveBuff");
		AwakeBuff_HP.Clear();
		AwakeBuff_N.Clear();

		//GameManager.Instantiate.QuestUIFunc();
	}

	//一度だけしか呼ばれたくない物
	protected virtual void FirstDeathFunc()
	{

	}


	//弾だとこっち
	public GameObject NewMakeBullet(int prefabsID, Vector3 pos, Quaternion rot)
	{
		//Currentが共有されている問題
		sb2.Append(BulletArray[prefabsID][Current_B[prefabsID]].name);
		if (transform.Find(sb2.ToString()) != null)
			return null;
		BulletArray[prefabsID][Current_B[prefabsID]].transform.localPosition = pos;
		BulletArray[prefabsID][Current_B[prefabsID]].transform.rotation = rot;
		BulletArray[prefabsID][Current_B[prefabsID]].SetActive(true);
		sb2.Length = 0;
		sb2.Append("Bullet_");
		sb2.Append(prefabsID);
		sb2.Append(":");
		sb2.Append(Current_B[prefabsID]);
		BulletArray[prefabsID][Current_B[prefabsID]].name = sb2.ToString();
		if (Current_B[prefabsID] == BulletArray[prefabsID].Length - 1)
		{
			Current_B[prefabsID] = 0;
			sb2.Length = 0;
			return BulletArray[prefabsID][BulletArray[prefabsID].Length - 1];
		}
		else
		{
			//sb2.Length = 0;
			return BulletArray[prefabsID][Current_B[prefabsID]++];
		}
			
	}

	//攻撃を受けたとき
	public void ReceiveAttack(DamageCalculate d)
	{
		if (d.SendBuff1 != null)
		{
			
			if (AwakeBuff_HP.Contains(d.SendBuff1) == false)
				AwakeBuff_HP.Add(d.SendBuff1);
		}
		if (d.SendBuff2 != null)
		{
			
			if (AwakeBuff_N.Contains(d.SendBuff2) == false)
			{
				AwakeBuff_N.Add(d.SendBuff2);
				//Debug.Log("www");
			}
				
		}
		//OnDefeated();//イベント送信
		_parameter.HP = _parameter.HP - (d.ATKPOWER - (_parameter.DEF + (int)BuffMathFunc(Buff_Others.Kind.DEF) ));
	}

	
	//受バフの処理
	IEnumerator ApplyReceiveBuff()
	{
		var qwe = new WaitForSeconds(1.0f);//GC対策らしい
		while (true)
		{
			for (int i = 0; i < AwakeBuff_HP.Count; i++)
			{
				_parameter.HP = _parameter.HP + AwakeBuff_HP[i].HP;
				AwakeBuff_HP[i].TIMER--;
				if (AwakeBuff_HP[i].TIMER <= 0)
					AwakeBuff_HP.Remove(AwakeBuff_HP[i]);
			}
			for (int i=0; i < AwakeBuff_N.Count; i++)
			{
				AwakeBuff_N[i].TIMER--;
				if(AwakeBuff_N[i].TIMER <= 0)
					AwakeBuff_N.Remove(AwakeBuff_N[i]);
			}
			yield return qwe;
		}

	}

	//バフの各種パラメータ計算のための関数
	protected float BuffMathFunc(Buff_Others.Kind type)
	{
		float iii = 0;
		foreach (Buff_Others s in AWAKE_N)//使うときに随時追加していく
		{
			switch (type)
			{
				case Buff_Others.Kind.ATK:
					iii += s.ATK;
					break;
				case Buff_Others.Kind.DEF:
					iii += s.DEF;
					break;

				case Buff_Others.Kind.SPEED:
					iii += s.SPEED;
					break;
				case Buff_Others.Kind.JUMP:
					iii += s.JUMP;
					break;
			}

			
		}
		return iii;
	}

	//自分に対してバフを付けたいときに使用
	protected void ADDBuff_Me(GameObject BuffPrefabs)
	{
		//GameObject tekitou = Instantiate(BuffPrefabs);//危なさそうなので直接参照はやめる
		if (BuffPrefabs.GetComponent<Buff_HPSP>())
		{
			Buff_HPSP buff1 = BuffPrefabs.GetComponent<Buff_HPSP>();
			if (AwakeBuff_HP.Contains(buff1) == false)
				AwakeBuff_HP.Add(buff1);
		}
		else if (BuffPrefabs.GetComponent<Buff_Others>())
		{
			Buff_Others buff2 = BuffPrefabs.GetComponent<Buff_Others>();
			if (AwakeBuff_N.Contains(buff2) == false)
				AwakeBuff_N.Add(buff2);
		}

	}

	//銃のブレを実装するための関数 旧
	protected Vector3 RandomShooterFunc(Transform tar)
	{
		return new Vector3(tar.position.x + Random.Range(-IsRandomShoot, IsRandomShoot), tar.position.y + Random.Range(-IsRandomShoot, IsRandomShoot), tar.position.z);
	}

	//今後はこっちに換装していく
	protected Vector3 RandomShooterFunc(Vector3 vec)
	{
		return new Vector3(vec.x + Random.Range(-IsRandomShoot, IsRandomShoot), vec.y + Random.Range(-IsRandomShoot, IsRandomShoot), vec.z);
	}

	//イベント
	//protected void OnDefeated()
	//{
	//	// ダメージを受けた通知
	//	Eventbus.Instance.NotifyDamegedObj(this);
	//}
}
