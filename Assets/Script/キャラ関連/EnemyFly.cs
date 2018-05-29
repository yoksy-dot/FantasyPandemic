using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFly : EnemyClass
{
    [SerializeField]
    private float FlyPosition = 10.0f;

    // Use this for initialization
    protected override void Start () {
        base.Start();

        _navAI = GetComponent<NavAI>();
	}

    // Update is called once per frame
    protected override void Update () {
        base.Update();
        transform.position = new Vector3(transform.position.x, FlyPosition, transform.position.z);
	}

    protected override void EnemyAI(MoveType type)
    {

        if (type == MoveType.AIMove && _navAI.NAV.remainingDistance < 0.5f)
            _navAI.GotoNextPoint(MovePoint);
    }
}
