using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Beam : BulletClass
{
    //弾のサブクラス
    //ビーム用クラス

    [SerializeField]
    private float range;
    //」ビームの飛ぶ速度
    [SerializeField]
    private float BeamSpeed = 2.0f;

	public LayerMask mask;
	private Ray ray;
	private RaycastHit hit;
	[SerializeField]
	private LineRenderer lineRenderer;

    //ビームの始点と終点
    [SerializeField]
    private Transform Start_pos, Fin_pos;
	private Vector3 posa, posb;

    private Transform saihate;
	private float timer2 = 0;
	[SerializeField,Tooltip("先端と末尾の発射間隔")]
    private float zure = 0.5f; 
    private bool StopFlag = false;

	private Transform T_me;
	[SerializeField]
	private GameObject Light;


	private void Awake()
	{
		T_me = transform;
	}

	protected override void OnEnable()
    {
        timer = 0;
		//rig = GetComponent<Rigidbody>();
		//lineRenderer = GetComponent<LineRenderer>();

		ray = new Ray(transform.position, transform.forward);
		posa = transform.position;//頭
		posb = transform.position;//尾
		Light.transform.position = posa;
	}

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

		Shot();

		if (!StopFlag)
		{
			posa += transform.forward * BeamSpeed;
			Light.transform.position = posa;

		}
			
		if (timer2 >= zure)
			posb += transform.forward * BeamSpeed;

		if (Physics.Raycast(ray, out hit, BeamSpeed, mask))
		{
			// hit 
			if (hit.collider.gameObject.GetComponent<BattleObject>() != null )
			{
				//DamageCalculate dam = new DamageCalculate(Attaker, BulletATK, buff1, buff2);
				//hit.collider.gameObject.GetComponent<BattleObject>().ReceiveAttack(dam);
				OnDefeated(hit.collider.gameObject.GetComponent<BattleObject>());
				_battle.MakeObject(1, hit.collider.gameObject.transform.position, hit.collider.gameObject.transform.rotation, "DamegedEffect");
				StopFlag = true;
			}
			else
			{
				StopFlag = true;
			}

		}
	}

    private void Shot()
    {
        lineRenderer.enabled = true;
		lineRenderer.SetPosition(0, posa);
		ray.origin = posa;
        ray.direction = transform.forward;

		lineRenderer.SetPosition(1, posb);

	}

    protected override void DestroyFunc()
    {
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		StopFlag = false;
		lineRenderer.enabled = false;
		timer2 = 0;
    }
}
