using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display_Parameter : MonoBehaviour {


    public int hp;
    public int Maxhp;
    public int atk;
    public int def;
    public int MaxBuulet;//装填数
    public float interval;//玉の発射間隔
    public float Reload;//装填時間
    public float speed;
    public float jump;
    public int spcial;//特殊の残弾
    public int Maxspcial;//特殊の最大値
    [Tooltip("100たまると特殊残弾が一増える")]
    public float spcial_interval;//特殊がたまるまでの時間
    public float distance;//射程?


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
