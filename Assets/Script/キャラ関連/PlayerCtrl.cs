using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerCtrl : BattleObject
{

    //キャラクターコントローラー
    protected CharacterController cCon;
    //　キャラクターの速度
    protected Vector3 velocity;
	//空中制御のための一時保管変数
	protected float Vec_Y = 0;
	//　キャラクター視点のカメラ
	private Transform myCamera;
    //　キャラクター視点のカメラで回転出来る限度
    [SerializeField]
    private float cameraRotateLimit;
    //　カメラの上下の移動方法。マウスを上で上を向く場合はtrue、マウスを上で下を向く場合はfalseを設定
    [SerializeField]
    private bool cameraRotForward = true;
    //　カメラの角度の初期値
    private Quaternion initCameraRot;
    //　キャラクター、カメラ（視点）の回転スピード
    [SerializeField]
    private float rotateSpeed;
    //　カメラのX軸の角度変化値
    private float xRotate;
    //　キャラクターのY軸の角度変化値
    private float yRotate;
    //　マウス移動のスピード
    [SerializeField]
    private float mouseSpeed;
    //マウスのホイールのスピード
    [SerializeField]
    private float zoomSpeed;
    //ズームの初期値
    private float zoom;
	[SerializeField,Tooltip("接近側")]
	private float zoomMin = 0.5f;
	[SerializeField,Tooltip("ひき側")]
	private float zoomMax = -5f;
    //　キャラクターのY軸の角度
    private Quaternion charaRotate;
    //　カメラのX軸の角度
    private Quaternion cameraRotate;

	private Vector3 CameraStartPos;

    //銃系の狙いを定める動き
    [SerializeField]
    private bool CanMoveGun;
    //狙いを定めるための点
    [SerializeField]
    private GameObject GunTarget;
    private GameObject Gun;
	[SerializeField,Tooltip("復活までの時間")]
	private float ResurrectionTime = 3.0f;
	//反動用座標
	private Transform RandTarget;

    private int RemainingBullet;
    private float timer2 = 0,SPtimer = 0;

	//UIここから
	private GameObject MoveCanvas, NonMoveCanvas;
    private GameObject InfoPanel, nonInfoPanel;//情報パネル
    //private GameObject BulletPanle;
    private Text HPText, SPText, SPText2, Name, /*WeaponName,*/ BulletNum;//テキスト取得
    private Slider HPSlider, SPSlider, BulletNumSlider;//スライダー取得
	//リスタートUI
	private GameObject ResurrectionPanel;
	//private Text ReUI, UntilUI;
	//private Slider _sli;
	//private ReStartUI _startui;

	protected bool ShootFlag, ShootFlag2,DeathFlag=false;
    private bool RelaodTime = false;

	private StringBuilder sb = new StringBuilder();

	//SPECIAL
	[SerializeField,Tooltip("Qで発生するバフ")]
	private GameObject Q_ADD_Buff_Prefabs;

    //接地判定
    private bool m_IsGrounded;

    protected override void Start()
    {
        ShootFlag = false;
        ShootFlag2 = false;
        base.Start();
        //SPtimer += Time.deltaTime;

        RemainingBullet = _parameter.MAXBULLET;

        //キャラクターコントローラの取得
        cCon = GetComponent<CharacterController>();
        myCamera = GetComponentInChildren<Camera>().transform;  //　キャラクター視点のカメラの取得
        initCameraRot = myCamera.localRotation;
        charaRotate = transform.localRotation;
        cameraRotate = myCamera.localRotation;
        if (CanMoveGun)
            Gun = transform.Find("Model/Gun").gameObject;
		RandTarget = GunTarget.transform.Find("Object").transform;//代入

		MoveCanvas = GameObject.Find("MoveCanvas");
		NonMoveCanvas = GameObject.Find("NonMoveCanvas");

		InfoPanel = MoveCanvas.transform.Find("InfoPanel").gameObject;
		nonInfoPanel = NonMoveCanvas.transform.Find("InfoPanel").gameObject;

		Name = nonInfoPanel.transform.Find("Name").GetComponent<Text>();
        HPText = InfoPanel.transform.Find("HPText").GetComponent<Text>();
        SPText = InfoPanel.transform.Find("SPText").GetComponent<Text>();
		SPText2 = InfoPanel.transform.Find("SPText2").GetComponent<Text>();
		HPSlider = InfoPanel.transform.Find("HPSlider").GetComponent<Slider>();
        SPSlider = InfoPanel.transform.Find("SPSlider").GetComponent<Slider>();

        //BulletPanle = GameObject.Find("Canvas/BulletPanel");
        //WeaponName = BulletPanle.transform.Find("WeaponName").GetComponent<Text>();
        BulletNum = InfoPanel.transform.Find("Bullet Text").GetComponent<Text>();
        BulletNumSlider = InfoPanel.transform.Find("ReloadSlider").GetComponent<Slider>();
		//リスタート
		ResurrectionPanel = MoveCanvas.transform.Find("Resurrect").gameObject;
		ResurrectionPanel.SetActive(false);

		CameraStartPos = myCamera.localPosition;
	}

    protected override void Update()
    {
        base.Update();
		if(RelaodTime)
			timer2 += Time.deltaTime;
        SPtimer += Time.deltaTime;
        //リロード処理
        if (RelaodTime && timer2 >= _parameter.RELOAD)
        {
            RelaodTime = false;
            timer2 = 0;
            RemainingBullet = _parameter.MAXBULLET;
        }

		//SP増加 3秒ごと
		if (SPtimer >= 3 && _parameter.SPECIAL != _parameter.MAXSPECIAL)
		{
			_parameter.SPECIAL++;
			SPtimer = 0;
		}

		//　キャラクターの向きを変更する
		RotateChara();
        //　視点の向きを変える
        RotateCamera();
        //UI更新
        UpDateUIFunc();

        if (CanMoveGun)
            Gun.transform.LookAt(GunTarget.transform);

		PlayerMove();

		

        //射撃系統
        if (Input.GetButton("Fire1") && RemainingBullet > 0 && !RelaodTime && timer >= _parameter.INTERVAL)
        {
            ShootFlag = true;
            timer = 0;

        }
        if (Input.GetButtonDown("Fire2") && timer >= _parameter.SPECIALINTERVAL && _parameter.SPECIAL > 0)
        {
			SPShoot_Down();
        }
		if(Input.GetButtonUp("Fire2") && timer >= _parameter.SPECIALINTERVAL && _parameter.SPECIAL > 0)
		{
			SPShoot_UP();
		}

		//リロード入力
        if (Input.GetKeyDown(KeyCode.R) && RelaodTime == false)
        {
            RelaodTime = true;
        }

		//特殊行動入力
		if (Input.GetKey(KeyCode.Q) && Q_ADD_Buff_Prefabs != null)
		{
			ADDBuff_Me(Q_ADD_Buff_Prefabs);
		}

    }

    void FixedUpdate()
    {
        if (ShootFlag)
        {
			ShootPoint.LookAt(RandomShooterFunc(RandTarget));

			//MakeBullet();
			NewMakeBullet(0, ShootPoint.position, ShootPoint.rotation);
			RemainingBullet--;
            UpDateUIFunc();
			ShootPoint.LookAt(GunTarget.transform); 
			 ShootFlag = false;
			if (AnimCtrl[0])
			{
				AnimCtrl[0].SetTrigger("Shoot");
			}
		}
        if (ShootFlag2)
        {
			EX_SP_ShootFunc();
			//MakeBullet(SPBullet, SPShootPoint);
			NewMakeBullet(1, SPShootPoint.position, SPShootPoint.rotation);
			UpDateUIFunc();
			_parameter.SPECIAL--;
			//ShootPoint.LookAt(GunTarget.transform);

			ShootFlag2 = false;
        }
    }

	protected virtual void PlayerMove()
	{
		
		//　キャラクターコントローラのコライダが地面と接触してるかどうか
		if (cCon.isGrounded/*GroundCheck()*/)
        {
            velocity = Vector3.zero;
			Vec_Y = 0;
            velocity = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized;
            velocity *= _parameter.SPEED + BuffMathFunc(Buff_Others.Kind.SPEED);

            if (Input.GetButton("Jump"))
            {
                velocity.y += _parameter.JUMP+ BuffMathFunc(Buff_Others.Kind.JUMP);
				if (AnimCtrl[0])
				{
					AnimCtrl[0].SetTrigger("Jump");
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
		if (AnimCtrl[0] && Vector3.SqrMagnitude(velocity) > 0.1f)
		{
			AnimCtrl[0].SetBool("Run", true);
		}
		else if(velocity == Vector3.zero)
		{
			AnimCtrl[0].SetBool("Run", false);
		}
	}

    //UIの更新
    void UpDateUIFunc()
    {
        int a = Mathf.RoundToInt(MathPercentage(_parameter.MAXHP, _parameter.HP));
        int b = Mathf.RoundToInt(MathPercentage(_parameter.MAXSPECIAL, _parameter.SPECIAL));
        Name.text = gameObject.name;

		sb.Append(a);
		sb.Append("%");
		HPText.text = sb.ToString();
		sb.Length = 0;//初期化

		HPSlider.value = a;

		sb.Append(_parameter.SPECIAL);
		sb.Append("/");
		sb.Append(_parameter.MAXSPECIAL);
		SPText2.text = sb.ToString();
		sb.Length = 0;

		SPSlider.value = b;

        int c = Mathf.RoundToInt(MathPercentage(_parameter.MAXBULLET, RemainingBullet));
        if (RelaodTime == false)
        {
			sb.Append(RemainingBullet);
			sb.Append("/");
			sb.Append(_parameter.MAXBULLET);
			BulletNum.text = sb.ToString();
			sb.Length = 0;

			BulletNumSlider.value = c;
        }
        else
        {
            BulletNum.text = "Reload!";
            int d = Mathf.RoundToInt(MathPercentage(_parameter.RELOAD, timer2));
            BulletNumSlider.value = d;
        }

	}

    //　キャラクターの角度を変更
    void RotateChara()
    {
        //　横の回転値を計算
        float yRotate = Input.GetAxis("Mouse X") * mouseSpeed;

        charaRotate *= Quaternion.Euler(0f, yRotate, 0f);

        //　キャラクターの回転を実行
        transform.localRotation = Quaternion.Slerp(transform.localRotation, charaRotate, rotateSpeed * Time.deltaTime);
    }
    //　カメラの角度を変更
    void RotateCamera()
    {

        float xRotate = Input.GetAxis("Mouse Y") * mouseSpeed;
        zoom += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        zoom = Mathf.Clamp(zoom, -5, 0.5f);


        //　マウスを上に移動した時に上を向かせたいなら反対方向に角度を反転させる
        if (cameraRotForward)
        {
            xRotate *= -1;
        }
        //　一旦角度を計算する	
        cameraRotate *= Quaternion.Euler(xRotate, 0f, 0f);
        //　カメラのX軸の角度が限界角度を超えたら限界角度に設定
        var resultYRot = Mathf.Clamp(Mathf.DeltaAngle(initCameraRot.eulerAngles.x, cameraRotate.eulerAngles.x), -cameraRotateLimit, cameraRotateLimit);
        //　角度を再構築
        cameraRotate = Quaternion.Euler(resultYRot, cameraRotate.eulerAngles.y, cameraRotate.eulerAngles.z);
        //　カメラの視点変更を実行
        myCamera.localRotation = Quaternion.Slerp(myCamera.localRotation, cameraRotate, rotateSpeed * Time.deltaTime);

		//myCamera.localPosition = new Vector3(0, CameraStartPos.y - ((zoom + CameraStartPos.y) / 3),zoom+ CameraStartPos.z);
		myCamera.localPosition = new Vector3(0, CameraStartPos.y, zoom);
	}

    private bool GroundCheck()
    {
        //m_PreviouslyGrounded = m_IsGrounded;
        CapsuleCollider m_Capsule = GetComponentInChildren<CapsuleCollider>();
        RaycastHit hitInfo;

        if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f), Vector3.down, out hitInfo,
                               ((m_Capsule.height / 2f) - m_Capsule.radius) + 0.1f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            m_IsGrounded = true;

            //m_GroundContactNormal = hitInfo.normal;
        }
        else
        {
            m_IsGrounded = false;
            //m_GroundContactNormal = Vector3.up;
        }

        //if (!m_PreviouslyGrounded && m_IsGrounded && m_Jumping)
        //{
        //    m_Jumping = false;
        //}
        //aaav = hitInfo;
        return m_IsGrounded;
    }

    //2つの数から割合を算出
    private float MathPercentage(float Max, float now)
    {
        return (now * 10 / Max) * 10;
    }

	//死亡時はゲームマネージャーに書いてあるので空白
	protected override void DeathFunc()
	{
		DeathFlag = true;
		ResurrectionPanel.SetActive(true);
		GameManager.Instantiate.PLAYEROBJ.SetActive(false);
		//RE_timer += Time.deltaTime;
		GameManager.Instantiate.StartCoroutine("REStartFunc",ResurrectionTime);
		//_startui.


	}

	//右クリックを押した時によばれる
	protected virtual void SPShoot_Down()
	{
		ShootFlag2 = true;
		timer = 0;
	}
	//右クリックを離したときに呼ばれる
	protected virtual void SPShoot_UP()
	{

	}
	//SP発射時に何か追加したいことがあるときのための関数
	protected virtual void EX_SP_ShootFunc()
	{

	}

}
