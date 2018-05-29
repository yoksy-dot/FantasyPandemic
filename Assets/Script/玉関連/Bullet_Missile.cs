using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Missile : Bullet_Bomb
{
	//要最適化
    [SerializeField]
    private GameObject seachObj;
    [SerializeField]
    private GameObject Target;
    public GameObject Missile_TARGET
    {
        set { Target = value; }
        get { return Target; }
    }

    [SerializeField]
    private float RotationSpeed;

    [SerializeField]
    private float ADDPower;

    [SerializeField,Tooltip("発射後しばらくは追尾しなくなる")]
    private bool VLSSystem;

    [SerializeField,Tooltip("非誘導時間")]
    private float FlyTime;

    //ベクトル関係
    Vector3 TarVec;
    Vector3 ForwardVec;
    float AngleDiff;
    float AngleADD;
    Quaternion ToRot;
    float t;

    private SeachSystem _seach;
    private float timer2 = 0;
    private bool first = true,addfalg = false;

    // Use this for initialization
    protected override void OnEnable() {
        base.OnEnable();
        _seach = seachObj.GetComponent<SeachSystem>();
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
        timer2 += Time.deltaTime;

        if (_seach.FOUND )
        {
            if (VLSSystem == false || timer2 >= FlyTime)
            {
                MathVector();
            }
        }

        addfalg = true;
        //this.transform.Translate(transform.forward);
	}

    private void FixedUpdate()
    {
        if (first && timer2 >= FlyTime)
        {
            rig.velocity = new Vector3(rig.velocity.x / 10, rig.velocity.y / 10, rig.velocity.z / 10);
			if(Target)
				transform.LookAt(Target.transform);
            first = false;
        }
            
        if (addfalg)
        {
			//Debug.Log("tra");
            rig.AddForce(transform.forward * ADDPower);
            addfalg = false;
        }
            
    }

    void MathVector()
    {
        if (Target == null)
            return;
        //ターゲットまでのベクトルを取得
        TarVec = Target.transform.position - transform.position;

        ForwardVec = transform.TransformDirection(transform.forward);

        AngleDiff = Vector3.Angle(TarVec, ForwardVec);
        //1Fの回転速度
        AngleADD = RotationSpeed * Time.deltaTime;
        //ターゲットまでのクオータニオン
        ToRot = Quaternion.LookRotation(TarVec);
        //実際にいくら回転するか
        t = AngleADD / AngleDiff;
        //回転
        transform.rotation = Quaternion.Slerp(transform.rotation, ToRot, t);

    }

	//そのままにしたいのでbaseのみ
	protected override void OnCollisionEnter(Collision coll)
	{
		base.OnCollisionEnter(coll);
	}
}
