using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FlyingType : PlayerCtrl
{

	float timer3 = 0;
	[SerializeField,Tooltip("空中起動時の自己バフ")]
	protected GameObject AirAtaackBuff;

	// Use this for initialization
	protected override void Start () {
		base.Start();
	}

	// Update is called once per frame
	protected override void Update() {
		base.Update();
		
	}

	protected override void PlayerMove()
	{
		//Debug.Log(_parameter.SPECIAL);
		//　キャラクターコントローラのコライダが地面と接触してるかどうか
		if (cCon.isGrounded/*GroundCheck()*/)
		{
			velocity = Vector3.zero;
			Vec_Y = 0;
			velocity = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized;
			velocity *= _parameter.SPEED + BuffMathFunc(Buff_Others.Kind.SPEED);

			if (Input.GetButtonDown("Jump"))
			{
				velocity.y += _parameter.JUMP + BuffMathFunc(Buff_Others.Kind.JUMP);
				//if (AnimCtrl[0])
				//{
				//	AnimCtrl[0].SetTrigger("Jump");
				//}

			}

		}
		else//つまり空中
		{
			Vec_Y = velocity.y;
			velocity = Vector3.zero;
			velocity = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized;
			velocity *= _parameter.SPEED + BuffMathFunc(Buff_Others.Kind.SPEED);

			if (_parameter.SPECIAL >= 1 && Input.GetButton("Jump"))
			{
				_parameter.SPECIAL --;
				ADDBuff_Me(AirAtaackBuff);
				if (velocity.y < 4)
					velocity.y += (_parameter.JUMP + BuffMathFunc(Buff_Others.Kind.JUMP)) / 10;
				else
					velocity.y = 4;

				//if (AnimCtrl[0])//飛行用
				//{
				//	//AnimCtrl.SetTrigger("Jump");
				//}

			}
		}
		velocity.y += Vec_Y + Physics.gravity.y * Time.deltaTime; //　重力値を計算
		cCon.Move(velocity * Time.deltaTime); //　キャラクターコントローラのMoveを使ってキャラクターを移動させる
		//if (AnimCtrl[0] && Vector3.SqrMagnitude(velocity) > 0.1f)
		//{
		//	AnimCtrl[0].SetBool("Run", true);
		//}
		//else if(velocity == Vector3.zero)
		//{
		//	AnimCtrl[0].SetBool("Run", false);
		//}
	}
}
