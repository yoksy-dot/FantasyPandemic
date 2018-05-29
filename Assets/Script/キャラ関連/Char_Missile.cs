using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Missile : Player_FlyingType
{
	//飛べる＆ミサイル

	[SerializeField]
	private GameObject Lazer;
	[SerializeField]
	private Material Red, Green, Green2;
	private GameObject L_obj;
	[SerializeField]
	private float range;
	public LayerMask mask;
	private Ray ray;
	private RaycastHit hit;
	private LineRenderer lineRenderer;

	private float atk_time = 5.0f, timer5 = 0;
	private bool Flag_m = false;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		L_obj = Instantiate(Lazer, SPShootPoint.position, SPShootPoint.rotation);
		lineRenderer = L_obj.GetComponent<LineRenderer>();
		
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update();
		
		ray = new Ray(SPShootPoint.position, SPShootPoint.forward);
		lineRenderer.SetPosition(0, SPShootPoint.position);
		lineRenderer.SetPosition(1, ray.origin + ray.direction * range);

		L_obj.GetComponent<Renderer>().material = Red;
		if (lineRenderer.enabled && Physics.SphereCast(ray, 2.0f, out hit, range, mask))
		{
			timer5 += Time.deltaTime;
			L_obj.GetComponent<Renderer>().material = Green;
			if (timer5 >= atk_time)
			{
				L_obj.GetComponent<Renderer>().material = Green2;
				TargetObj = hit.collider.gameObject;
				Flag_m = true;
			}
			else
			{
				Flag_m = false;
				TargetObj = null;
			}
				


			//Debug.Log(timer5);
			// hit 
		}
		else
		{
			L_obj.GetComponent<Renderer>().material = Red;
			timer5 = 0;
		}
	}

	protected override void SPShoot_Down()
	{
		//Shot();
		lineRenderer.enabled = true;


	}

	protected override void SPShoot_UP()
	{
		//base.SPShoot_UP();

		lineRenderer.enabled = false;
		if (Flag_m)
		{
			ShootFlag2 = true;
			Flag_m = false;
			timer = 0;
			_parameter.SPECIAL -= 29;
		}
	}

	private void Shot()
	{
		

		//lineRenderer.enabled = false;
		//if (Physics.Raycast(ray, out hit, range, mask))
		
		

	}
}
