using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffClass : MonoBehaviour {
    //バフのスーパークラス
    //HPだけは共通
    [SerializeField]
    private bool permanence;//永続かどうか
    public bool PERMANENCE
    {
        get { return permanence; }
    }
    [SerializeField]
    private int hp;
    public int HP
    {
        get { return hp; }
        set
        {
            hp = value;
            if (hp <= 0)
                hp = 0;
        }
    }
    [SerializeField]
    private float timer;//効果時間
    

	private float gametimer;
	public float TIMER
	{
		get { return gametimer; }
		set { gametimer = value; }
	}

	private void Awake()
	{
		gametimer = timer;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
}
