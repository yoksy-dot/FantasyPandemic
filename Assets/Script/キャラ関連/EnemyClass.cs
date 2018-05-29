using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyClass : BattleObject
{
    /*AI関連*/
    //[SerializeField]
    //private GameObject SeachObj;
	[SerializeField]
	private SeachSystem _seach;
    [SerializeField,Tooltip("LookAtを使うかどうか")]
    private bool useLook;
	//[SerializeField, Tooltip("狙いを定めるかどうか")]
	//private bool AutoAtk = true;

    public enum MoveType
    {
        nonMove,
        AIMove,
        ChaseMove,
		RandMove
    }
    [SerializeField]
    protected MoveType Moving;
    [SerializeField,Tooltip("Chaseモードの時,この距離未満なら追いかけ続ける")]
    protected float ChaseDis = 0;
    [SerializeField]
    protected List<GameObject> MovePoint = new List<GameObject>();
    protected NavAI _navAI;

    /*第2射出口関連*/
    [SerializeField,Tooltip("第2射出口を使用するかどうか")]
    private bool UseSecond;
    [SerializeField]
    private float Sec_Interval;
    [SerializeField]
    private List<GameObject> SecondBullet = new List<GameObject>();
    [SerializeField]
    private List<GameObject> SecondShootPoint = new List<GameObject>();
	//Shooterの子
	//[SerializeField,Tooltip("Shooterの子")]
	//private Transform Child;

    private float timer2 = 0;

    private Vector3 Tar_Vec;
    //private Transform nav_pos;

	// Use this for initialization
	protected override void Start () {
        base.Start();

		//GameObject ctrl = GameObject.Find("ObjectCtrl").gameObject;
		transform.parent = _battleSystem.transform;

        //_seach = SeachObj.GetComponent<SeachSystem>();
        if ((int)Moving >= 1)
        {
            _navAI = GetComponent<NavAI>(); 
        }

            

        if (Moving == MoveType.AIMove)
            _navAI.GotoNextPoint(MovePoint);
		else if (Moving == MoveType.RandMove)
		{
			_navAI.RandStart(5);
			_navAI.RandomAI();
		}
			
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
        timer2 += Time.deltaTime;

        EnemyAI(Moving);

        if (_seach.FOUND && timer >= _parameter.INTERVAL)
        {
			if (useLook)
			{
				Vector3 vec = _seach.TARGET.transform.localPosition;//移動
				ShootPoint.transform.LookAt(RandomShooterFunc(vec)); 
			}
			if (Bullet)
				//MakeBullet();
				NewMakeBullet(0, ShootPoint.position, ShootPoint.rotation);
			timer = 0;
			Ani_AtkFunc();
        }
        if(_seach.FOUND && UseSecond && timer2 >= Sec_Interval)
        {

            for(int i =0;i < SecondBullet.Count; i++)
            {
                if (useLook)
				{
					Vector3 vec = _seach.TARGET.transform.localPosition;//移動
					ShootPoint.transform.LookAt(RandomShooterFunc(vec));
				}
				//MakeBullet(SecondBullet[i], SecondShootPoint[i].transform);
				NewMakeBullet(i + 1, SecondShootPoint[i].transform.localPosition, SecondShootPoint[i].transform.localRotation);

			}
                
            timer2 = 0;
        }
    }

    protected virtual void EnemyAI(MoveType type)
    {
        switch (type)
        {
            case MoveType.AIMove:
                //プレイヤー発見時
                if (_seach.FOUND)
                {
                    _navAI.AgentMoveBoolFunc(false);
                    Tar_Vec = _seach.TARGET.transform.localPosition - transform.localPosition;
                    transform.rotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(Tar_Vec), Time.deltaTime * Random.Range(0, _parameter.SPEED));
                    
                }
                //プレイヤー未発見
                else if (!_seach.FOUND)
                {
                    _navAI.AgentMoveBoolFunc(true);
                    if (_navAI.NAV.remainingDistance < 0.5f)
                        _navAI.GotoNextPoint(MovePoint);
                }
				Ani_MoveFunc();
				break;
            case MoveType.ChaseMove:
                //プレイヤー発見時
                if (_seach.FOUND)
                {
                    _navAI.SetTarget(_seach.TARGET);
                    if(_navAI.NAV.remainingDistance > ChaseDis)//ターゲットまでの距離が開きすぎている
                        _navAI.GotoNextPoint(MovePoint);
                    Tar_Vec = _seach.TARGET.transform.localPosition - transform.localPosition;
                    transform.rotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(Tar_Vec), Time.deltaTime * Random.Range(0, _parameter.SPEED));
				}
                //プレイヤー未発見
                else if (!_seach.FOUND)
                {
                    if (_navAI.NAV.remainingDistance < 0.1f)
                        _navAI.GotoNextPoint(MovePoint);
                }
				Ani_MoveFunc();
				break;

			case MoveType.RandMove:
				if (_seach.FOUND)
				{
					_navAI.SetTarget(_seach.TARGET);
					if (_navAI.NAV.remainingDistance > ChaseDis)//ターゲットまでの距離が開きすぎている
					{
						_navAI.RandomAI();
						break;
					}
					Tar_Vec = _seach.TARGET.transform.localPosition - transform.localPosition;
					transform.rotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(Tar_Vec), Time.deltaTime * Random.Range(0, _parameter.SPEED));
				}
				else if (!_seach.FOUND)
				{
					if (_navAI.NAV.remainingDistance < 0.1f)
						_navAI.RandomAI();
				}
				Ani_MoveFunc();
				break;
            case MoveType.nonMove:
				//何もしない
				Ani_WaitFunc();
				break;

        }
	}

	//アニメ用関数
	//wait
	protected virtual void Ani_WaitFunc()
	{

	}
	//移動時に呼ばれる
	protected virtual void Ani_MoveFunc()
	{

	}
	//攻撃
	protected virtual void Ani_AtkFunc()
	{

	}

	////再利用用初期化関数
	//protected virtual void ALL_ClearFunc()
	//{
	//	//Search
	//	_seach.ClearFunc();

	//	//NAVI
	//	_navAI.ClearFunc();
	//	//バフはしなくてもよい
	//	//Transform
	//	transform.localPosition = Vector3.zero;
	//	transform.localRotation = Quaternion.identity;

	//	//timer
	//	timer = 0;
	//	timer2 = 0;

	//	//Parameter

	//}
}
