using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_kaine : Char_Grenade
{
	/*	キャラ情報
	 * Name:カイネ
	 * Main:ビーム
	 * Sub :手榴弾
	 * 特殊:未定
	 * Q   :加速(ローラーダッシュ)
	*/

	[SerializeField]
	private ParticleSystem _p2, _p3, _p4;
	[SerializeField]
	private Light[] lights = new Light[4];

	protected override void Start()
	{
		base.Start();
		_p2.Stop();
		_p3.Stop();
		_p4.Stop();
	}


	//TODO
	//
	protected override void PlayerMove()
	{
		if (cCon.isGrounded/*GroundCheck()*/)
		{
			velocity = Vector3.zero;
			Vec_Y = 0;
			velocity = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized;
			velocity *= _parameter.SPEED + BuffMathFunc(Buff_Others.Kind.SPEED);

			if (Input.GetButton("Jump"))
			{
				velocity.y += _parameter.JUMP + BuffMathFunc(Buff_Others.Kind.JUMP);
				if (AnimCtrl[0])
				{
					AnimCtrl[0].SetTrigger("Jump");
					AnimCtrl[1].SetTrigger("Jump");
					_particleSystem.Stop();
					_p2.Stop();
					_p3.Stop();
					_p4.Stop();
				}
			}

		}
		else//つまり空中
		{
			Vec_Y = velocity.y;
			velocity = Vector3.zero;
			velocity = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized;
			velocity *= _parameter.SPEED + BuffMathFunc(Buff_Others.Kind.SPEED);
		}
		velocity.y += Vec_Y + Physics.gravity.y * Time.deltaTime; //　重力値を計算
		cCon.Move(velocity * Time.deltaTime); //　キャラクターコントローラのMoveを使ってキャラクターを移動させる

		//Debug.Log(Vector3.SqrMagnitude(velocity));

		if (AnimCtrl[0] && Vector3.SqrMagnitude(velocity) < 1.0f)//停止
		{
			AnimCtrl[0].SetBool("Run", false);
			AnimCtrl[1].SetBool("Walk", false);
			AnimCtrl[1].SetBool("Run", false);
		}
		else if (Vector3.SqrMagnitude(velocity) > 900.0f)//高速帯
		{
			AnimCtrl[0].SetBool("Run", true);
			AnimCtrl[1].SetBool("Run", true);
			AnimCtrl[1].SetBool("Walk", false);
		}
		else if (AnimCtrl[0] && Vector3.SqrMagnitude(velocity) > 36.0f)//低速帯
		{
			AnimCtrl[0].SetBool("Run", true);
			AnimCtrl[1].SetBool("Walk", true);
			AnimCtrl[1].SetBool("Run", false);


		}

		if (Input.GetKeyDown(KeyCode.Q))
		{
			_particleSystem.Play(true);
			_p2.Play(true);
			_p3.Play(true);
			_p4.Play(true);
		}
		else if (Input.GetKeyUp(KeyCode.Q))
		{
			_particleSystem.Stop();
			_p2.Stop();
			_p3.Stop();
			_p4.Stop();
		}
		//明かり用
		if (Input.GetKey(KeyCode.Q))
		{
			if (lights[0].intensity < 8)
			{
				for (int i = 0; i < 4; i++)
				{
					lights[i].intensity += 0.2f;
				}
			}
			else
			{
				for (int i = 0; i < 4; i++)
				{
					lights[i].intensity = Random.Range(8.0f, 10.0f);
				}
			}
		}
		else
		{
			if (lights[0].intensity > 0)
			{
				for (int i = 0; i < 4; i++)
				{
					lights[i].intensity -= 0.4f;

				}
			}
		}
	}
	protected override void DeathFunc()
	{
		base.DeathFunc();
		AnimCtrl[1].SetTrigger("Dead");
		_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		_p2.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		_p3.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		_p4.Stop(true, ParticleSystemStopBehavior.StopEmitting);

	}
}
