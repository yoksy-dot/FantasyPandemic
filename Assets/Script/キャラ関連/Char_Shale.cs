using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Shale : Player_FlyingType
{
	/*	キャラ情報
	 * Name:シェイル
	 * Main:ガトリング
	 * Sub :爆撃
	 * 特殊:飛行
	 * Q   :盾
	*/

	//TODO
	//Qキーで盾を構えることができるようにする
	//5/12 スーパークラスをChar_MissileからPlayer_FlyingType に変更

	//特殊　追加
	[SerializeField,Tooltip("追加分")]
	protected GameObject SPBullet2;
	//特殊の発射口　追加
	[SerializeField, Tooltip("追加分")]
	protected Transform SPShootPoint2;

	[SerializeField]
	private TrailRenderer T_r1, T_r2;

	[SerializeField]
	private Light _Light, _Light2;

	protected override void PlayerMove()
	{
		float F_Flag = 0;
		//　キャラクターコントローラのコライダが地面と接触してるかどうか
		if (cCon.isGrounded/*GroundCheck()*/)
		{
			AnimCtrl[0].SetBool("Jump", false);
			if(T_r1.startWidth < 0)
			{
				T_r1.startWidth -= 0.01f;
				T_r2.startWidth -= 0.01f;
			}
			else if(T_r1.startWidth >= 0)
			{
				T_r1.startWidth = T_r2.startWidth = 0;
			}
			
			velocity = Vector3.zero;
			Vec_Y = 0;
			velocity = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized;
			velocity *= _parameter.SPEED + BuffMathFunc(Buff_Others.Kind.SPEED);

			if (Input.GetButtonDown("Jump"))
			{
				velocity.y += _parameter.JUMP + BuffMathFunc(Buff_Others.Kind.JUMP);
				if (AnimCtrl[0])
				{
					F_Flag = 1;
					AnimCtrl[0].SetBool("Jump", true);
					if (_Light.intensity < 5.0f)
					{
						_Light.intensity += 0.5f;
						_Light2.intensity += 0.5f;
					}
						
				}

			}
		}
		else//つまり空中
		{
			Vec_Y = velocity.y;
			velocity = Vector3.zero;
			velocity = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized;
			velocity *= _parameter.SPEED + BuffMathFunc(Buff_Others.Kind.SPEED);

			T_r1.startWidth = 0.1f;
			T_r2.startWidth = 0.1f;

			if (_parameter.SPECIAL >= 1 && Input.GetButton("Jump"))
			{
				_parameter.SPECIAL--;
				ADDBuff_Me(AirAtaackBuff);
				if (velocity.y < 4)
					velocity.y += (_parameter.JUMP + BuffMathFunc(Buff_Others.Kind.JUMP)) / 10;
				else
					velocity.y = 4;

				if (AnimCtrl[0])//飛行用
				{
					F_Flag = 1;
					AnimCtrl[0].SetBool("Fly", true);
				}
				if (_Light.intensity < 5.0f)
				{
					_Light.intensity += 0.5f;
					_Light2.intensity += 0.5f;
				}
					
				_particleSystem.Play(true);

			}
			else
			{
				F_Flag = 5;
				AnimCtrl[0].SetBool("Fly", false);//落下中
				if (_Light.intensity > 0.0f)
				{
					_Light.intensity -= 0.5f;
					_Light2.intensity -= 0.5f;
				}
					
				_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			}
				

		}

		velocity.y += Vec_Y + Physics.gravity.y * Time.deltaTime; //　重力値を計算
		cCon.Move(velocity * Time.deltaTime); //　キャラクターコントローラのMoveを使ってキャラクターを移動させる
		if (AnimCtrl[0] && velocity.x + velocity.z < 1.0f && F_Flag == 0)
		{
			AnimCtrl[0].SetBool("Run", false);
		}
		else if (AnimCtrl[0] && Vector3.SqrMagnitude(velocity) > 2.0f && F_Flag == 0)
		{
			AnimCtrl[0].SetBool("Run", true);
		}
	}

	protected override void EX_SP_ShootFunc()
	{		
		_parameter.SPECIAL-=39;//このキャラは特殊値がかなり多いので追加減少させる 計40
							   //MakeBullet(SPBullet2, SPShootPoint2);
		NewMakeBullet(1, SPShootPoint2.position, SPShootPoint2.rotation);
	}
}
