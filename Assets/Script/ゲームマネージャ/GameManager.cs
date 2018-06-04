using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : Singleton<GameManager>
{
	//プレイヤー関連
    [SerializeField, Tooltip("選ばれた操作するプレイヤーを指定")]
	private GameObject Player;
	[SerializeField, Tooltip("プレイヤーのプレハブを保持")]
	private List<GameObject> PlayerPrefabs = new List<GameObject>();
	private int PrefabNum = 0;
	//実際に動くプレイヤーのオブジェクト
	private GameObject PlayerObj;

	//プレイヤーのスタート位置番号
	private Transform StartPos;

	//プレイヤーのスタート位置候補
	public GameObject[] _StartPos;

	//プレイヤーコントローラ
	private PlayerCtrl PlayerSys;

	public GameObject PLAYER
	{
		get { return Player; }
	}
	public GameObject PLAYEROBJ
	{
		get { return PlayerObj; }
		set { PlayerObj = value;}
	}
	public Transform START_POS
	{
		get { return StartPos; }
	}
	public GameObject Candidate_POS
	{
		set { }
	}

	//次に移るシーンの名前
	private string nextSceneName;

	//メニュー関連
	private GameObject Title;
	private GameObject Menu;
	private Button[] StageBuuton = new Button[3];
	private Text ChoiseText;


	//ゲームマネージャがすでに生成されているかどうか
	bool instanced = false;
    /// <summary>
    /// ゲーム状態（大枠）
    /// </summary>
    public enum GameState
    {
        Menu,
		BREAK,
		BATTLE,
		DIFFENCE,
		Result,
        End,
    };

	[SerializeField, Tooltip("勝利条件のための数字,(float)につき注意")]
	private float WinnNum;


	public float WINNUM
	{
		get { return WinnNum; }
		set { WinnNum = value; }
	}

	/// <summary>現在の状態</summary>
	private GameState _CurrentState /*= GameState.Menu*/;
    public GameState STATE
    {
        get { return _CurrentState; }
    }


    //時間関係
    private float ElapsedTime;         //経過時間
    public float ELAPSEDTIME
    {
        get { return ElapsedTime; }
    }

	private float FinTime;
	public float FINTIME
	{
		get { return FinTime; }
	}

	[SerializeField, TooltipAttribute("制限時間")]
	private float LimitTime;    //制限(限界)時間
	public float LIMITTIME
	{
		set { LimitTime = value; }
		get { return LimitTime; }
	}

	private const int CountDownNum = 3; //スタート時のカウントダウン数
	public int COUNTDOWN
	{
		get { return CountDownNum; }
	}
	//キルスコア
	private int killscore;
	private int killImportant;

	public int KILL
	{
		set { killscore = value; }
		get { return killscore; }
	}
	public int KILL_IMP
	{
		set { killImportant = value; }
		get { return killImportant; }
	}

	//勝ち負けの判定
	private bool IsWinn;
	public bool WIN
	{
		set { IsWinn = value; }
		get { return IsWinn; }
	}

	//リスタートUI
	private GameObject ResurrectionPanel;
	private Text ReUI, UntilUI;
	private Slider _sli;
	public float RE_timer = 0;
	//private BattleStageSystem B_sys;

    public override void Awake()
    {
        base.Awake();
        if (GameObject.Find("ゲームマネージャー"))
            Destroy(this.gameObject);
        else
            name = "ゲームマネージャー";
    }

    private void Start()
    {
		//SceneManager.activeSceneChanged += OnActiveSceneChanged;
		//Cursor.lockState = CursorLockMode.Locked;
	}

	bool aaa = true;

    private void Update()
    {

		if (Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
			//Cursor.visible = !aaa;
		}
	}
    /// <summary>
    /// 更新関数
    /// </summary>
    private void FixedUpdate()
    {
		//Debug.Log(_CurrentState);
		if (ElapsedTime == 0)//各種スタート関数
		{
			switch (_CurrentState)
			{
				case GameState.Menu:
					StartMenu();
					break;

				case GameState.BREAK:
					StartInGame();
					break;
				case GameState.BATTLE:
					StartInGame();
					break;
				case GameState.DIFFENCE:
					StartInGame();
					break;
				case GameState.End:
					//updateEnd();
					break;
			}
		}
			
		ElapsedTime += Time.deltaTime;

        switch (_CurrentState)//各種アップデート関数
        {
            case GameState.Menu:
				UpdateMenu();

				break;

            case GameState.BREAK:
                //updateInGame();
                break;
			case GameState.BATTLE:
				
				break;
			case GameState.DIFFENCE:
				//updateInGame();
				break;
			case GameState.End:
                //updateEnd();
                break;
        }
    }

	//メニュー関連関数
	private void StartMenu()
	{
		ChoisePlayer(0);
		if (!Title)
			Title = GameObject.Find("Canvas/Panel").gameObject;
		if (!Menu)
			Menu = GameObject.Find("Canvas/MenuPanel").gameObject;
		//イベント処理
		//Title
		GameObject titleText = Title.transform.Find("Titlepl").gameObject;
		EventTrigger trigger = titleText.GetComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry
		{
			eventID = EventTriggerType.PointerClick
		};
		entry.callback.AddListener((data) => { SetActiveFunc((PointerEventData)data); });
		trigger.triggers.Add(entry);
		//キャラ選択
		//0
		GameObject CharButton = Menu.transform.Find("C_Button0").gameObject;
		EventTrigger trigger2 = CharButton.GetComponent<EventTrigger>();
		EventTrigger.Entry entry2 = new EventTrigger.Entry
		{
			eventID = EventTriggerType.PointerClick
		};
		entry2.callback.AddListener((data) => { ChoisePlayer(0); });
		trigger2.triggers.Add(entry2);
		//1
		GameObject CharButton1 = Menu.transform.Find("C_Button1").gameObject;
		EventTrigger trigger3 = CharButton1.GetComponent<EventTrigger>();
		EventTrigger.Entry entry3 = new EventTrigger.Entry
		{
			eventID = EventTriggerType.PointerClick
		};
		entry3.callback.AddListener((data) => { ChoisePlayer(1); });
		trigger3.triggers.Add(entry3);
		//2
		GameObject CharButton2 = Menu.transform.Find("C_Button2").gameObject;
		EventTrigger trigger4 = CharButton2.GetComponent<EventTrigger>();
		EventTrigger.Entry entry4 = new EventTrigger.Entry
		{
			eventID = EventTriggerType.PointerClick
		};
		entry4.callback.AddListener((data) => { ChoisePlayer(2); });
		trigger4.triggers.Add(entry4);
		//3
		GameObject CharButton3 = Menu.transform.Find("C_Button3").gameObject;
		EventTrigger trigger5 = CharButton3.GetComponent<EventTrigger>();
		EventTrigger.Entry entry5 = new EventTrigger.Entry
		{
			eventID = EventTriggerType.PointerClick
		};
		entry5.callback.AddListener((data) => { ChoisePlayer(3); });
		trigger5.triggers.Add(entry5);
		//ステージ
		//没ステ
		GameObject Stage0 = Menu.transform.Find("S_Button0").gameObject;
		EventTrigger trigger_S0 = Stage0.GetComponent<EventTrigger>();
		EventTrigger.Entry entryS0 = new EventTrigger.Entry
		{
			eventID = EventTriggerType.PointerClick
		};
		entryS0.callback.AddListener((data) => { SceneChengeManager(2); });
		trigger_S0.triggers.Add(entryS0);
		//Test
		GameObject Stage1 = Menu.transform.Find("S_Button1").gameObject;
		EventTrigger trigger_S1 = Stage1.GetComponent<EventTrigger>();
		EventTrigger.Entry entryS1 = new EventTrigger.Entry
		{
			eventID = EventTriggerType.PointerClick
		};
		entryS1.callback.AddListener((data) => { SceneChengeManager(1); });
		trigger_S1.triggers.Add(entryS1);
		//ストーリー
		GameObject Stage2 = Menu.transform.Find("S_Button2").gameObject;
		EventTrigger trigger_S2 = Stage2.GetComponent<EventTrigger>();
		EventTrigger.Entry entryS2 = new EventTrigger.Entry
		{
			eventID = EventTriggerType.PointerClick
		};
		entryS2.callback.AddListener((data) => { SceneChengeManager(4); });
		trigger_S2.triggers.Add(entryS2);
		//ストーリー2
		GameObject Stage3 = Menu.transform.Find("S_Button3").gameObject;
		EventTrigger trigger_S3 = Stage3.GetComponent<EventTrigger>();
		EventTrigger.Entry entryS3 = new EventTrigger.Entry
		{
			eventID = EventTriggerType.PointerClick
		};
		entryS3.callback.AddListener((data) => { SceneChengeManager(5); });
		trigger_S3.triggers.Add(entryS3);

		//ここまで

		ChoiseText = Menu.transform.Find("ChoiseText").GetComponent<Text>();
		//ChoiseText.text = "Player:" + PlayerPrefabs[PrefabNum].name;

		StageBuuton[0] = Stage0.GetComponent<Button>();
		StageBuuton[1] = Stage1.GetComponent<Button>();

		Menu.SetActive(false);

		/*イベント設定*/
		//titleText.PointerCheck.AddListener();
	}
    private void UpdateMenu()
    {
		if (Player == null)
		{
			//for (int i = 0; i > StageBuuton.Length - 1; i++)
			//	StageBuuton[i].interactable = false;
			Debug.Log("プレイヤーなしにつきやばい");
		}
		else
		{
			//for (int i = 0; i > StageBuuton.Length - 1; i++)
			//	StageBuuton[i].interactable = true;
			switch (PrefabNum)
			{
				case 0:
					ChoiseText.text = "Player:Kaine";
					break;
				case 1:
					ChoiseText.text = "Player:Shaile";
					break;
				case 2:
					ChoiseText.text = "Player:Elephant";
					break;
			}
		}

	}

	public void SetActiveFunc(PointerEventData d)
	{
		Title.SetActive(false);
		Menu.SetActive(true);
	}
	//ここまで

	/// <summary>
	/// インゲーム中の更新処理
	/// </summary>
	private void UpdateInGame()
    {
        //  Debug.Log(ELAPSEDTIME);
        //時間切れになったら終了
        //if (ELAPSEDTIME > LIMITTIME)
        //{
        //    
        //    //Debug.Log("時間切れ");
        //}

    }

	private void StartInGame()
	{

		StartPos = _StartPos[Random.Range(0, _StartPos.Length)].transform;
		//プレイヤー生産
		PlayerObj = Instantiate(PlayerPrefabs[PrefabNum], StartPos.position, StartPos.rotation);
		PlayerSys = PlayerObj.GetComponent<PlayerCtrl>();

		//リスタート
		ResurrectionPanel = GameObject.Find("Canvas/Resurrect").gameObject;
		ReUI = ResurrectionPanel.transform.Find("Count").GetComponent<Text>();
		UntilUI = ResurrectionPanel.transform.Find("Until").GetComponent<Text>();
		_sli = ResurrectionPanel.transform.Find("RESlider").GetComponent<Slider>();
		
		_sli.value = 0;
		ReUI.text = "0%";

		StartCoroutine("GameIsWinn");
	}

	IEnumerator REStartFunc(float RestratTime)
	{
		var waiting = new WaitForSeconds(0.1f);
		while (true)
		{
			if (RE_timer >= RestratTime)
			{
				
				RE_timer = 0;
				Destroy(PlayerObj);
				//GameObject.Find("Canvas/Resurrect").gameObject.SetActive(false);
				//プレイヤー生産
				GameManager.Instantiate.PLAYEROBJ =
					Instantiate(Instantiate.PLAYER, Instantiate.START_POS.position, Instantiate.START_POS.rotation);
				yield break;
			}
			ReStartUI(RestratTime);
			RE_timer += 0.1f;
			yield return waiting;
		}
	}

	void ReStartUI(float num)
	{
		int e = Mathf.RoundToInt(MathPercentage(num, RE_timer));
		_sli.value = e;
		ReUI.text = e + "%";
		if (e >= 90)
		{
			UntilUI.text = "Ready?";
		}
	}

	private float MathPercentage(float Max, float now)
	{
		return (now * 10 / Max) * 10;
	}

	/// <summary>
	/// ゲーム終了時の更新処理
	/// </summary>
	private void UpdateEnd()
    {
        //Debug.Log("終了です");

    }


    //bool IsGameEnd() { return _CurrentState == GameState.End; }


    public void SceneChengeManager(int SceneNum)
    {
		switch (SceneNum)
		{
			case 0://タイトル兼メニュー画面
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				_CurrentState = GameState.Menu;
				ElapsedTime = 0;
				FinTime = 0;
				killscore = 0;
				killImportant = 0;
				SceneManager.LoadScene("Menu");
				break;
			case 1://テストステージ
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
				_CurrentState = GameState.BATTLE;
				SceneManager.LoadScene("Test");
				WinnNum = 8;
				LimitTime = 150.0f;
				ElapsedTime = 0;

				break;
			case 2://実用ステージ
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
				_CurrentState = GameState.BREAK;
				ElapsedTime = 0;
				WinnNum = 1;
				LimitTime = 150.0f;
				SceneManager.LoadScene("HQBase");
				break;
			case 3://リザルト
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				_CurrentState = GameState.Result;
				FinTime = ElapsedTime;
				ElapsedTime = 0;
				SceneManager.LoadScene("Result");
				break;
			case 4://ストーリー
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
				_CurrentState = GameState.BREAK;
				ElapsedTime = 0;
				WinnNum = 1;
				LimitTime = 300.0f;
				SceneManager.LoadScene("Story");
				break;
			case 5://ストーリー2
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
				_CurrentState = GameState.BREAK;
				ElapsedTime = 0;
				WinnNum = 1;
				LimitTime = 300.0f;
				SceneManager.LoadScene("Story2");
				break;
		}
		
	}

	//UI用キャラ選択関数
	public void ChoisePlayer(int num)
	{
		PrefabNum = num;
		Player = PlayerPrefabs[PrefabNum];
	}



	IEnumerator GameIsWinn()
	{
		var waiting = new WaitForSeconds(3.0f);
		while (true)
		{
			switch (_CurrentState)
			{
				case GameState.BATTLE:
					//一定数撃破で勝利
					if (Mathf.CeilToInt(WinnNum) <= GameManager.Instantiate.KILL)
					{
						GameManager.Instantiate.WIN = true;
						GameManager.Instantiate.SceneChengeManager(3);
						yield break;
						//
					}
					//タイムオーバーで敗北
					else if (GameManager.Instantiate.LIMITTIME <= GameManager.Instantiate.ELAPSEDTIME)
					{
						GameManager.Instantiate.WIN = false;
						GameManager.Instantiate.SceneChengeManager(3);
						yield break;
					}
					break;
				case GameState.BREAK:
					//特定の敵の撃破で勝利
					if (Mathf.CeilToInt(WinnNum) <= GameManager.Instantiate.KILL_IMP)
					{
						GameManager.Instantiate.WIN = true;
						GameManager.Instantiate.SceneChengeManager(3);
						yield break;
					}
					//タイムオーバーで敗北
					else if (GameManager.Instantiate.LIMITTIME <= GameManager.Instantiate.ELAPSEDTIME)
					{
						GameManager.Instantiate.WIN = false;
						GameManager.Instantiate.SceneChengeManager(3);
						yield break;
					}
					break;//
				case GameState.DIFFENCE:
					//タイムオーバーで勝利
					if (Mathf.CeilToInt(WinnNum) <= GameManager.Instantiate.ELAPSEDTIME)
					{
						GameManager.Instantiate.WIN = true;
						GameManager.Instantiate.SceneChengeManager(3);
						yield break;
					}
					//特定の味方撃破で敗北
					else if (GameManager.Instantiate.KILL_IMP >= 1)
					{
						GameManager.Instantiate.WIN = false;
						GameManager.Instantiate.SceneChengeManager(3);
						yield break;
					}
					break;
			}
			yield return waiting;
		}
	}
}

