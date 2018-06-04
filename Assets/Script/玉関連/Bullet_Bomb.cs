using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Bomb : BulletClass
{
    //弾クラスのサブクラス
    //通常爆風発生クラス

    [SerializeField]
    private GameObject BombSphere;
    //0.01秒ごとの拡大値
    [SerializeField]
    private float Expansion = 0.1f;
    //最大拡大値
    [SerializeField]
    private float MaxExpansion = 10.0f;
    //爆風の攻撃力
    [SerializeField]
    private int BombAtk;


    protected override void DestroyFunc()
    {
        GameObject bomb = Instantiate(BombSphere, transform.position, transform.rotation);
        BombSystem _Bsys = bomb.GetComponent<BombSystem>();
        _Bsys.EXP = Expansion;
        _Bsys.MAX_EXP = MaxExpansion;
        _Bsys.ATK = BombAtk;
        _Bsys.ATACKER = Attaker;
		if (_particleSystem)
			_particleSystem.Stop();
	}
}
