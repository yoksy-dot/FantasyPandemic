using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(NavMeshAgent))]
public class NavAI : MonoBehaviour {

	[SerializeField]
    private NavMeshAgent _nav;
    public NavMeshAgent NAV
    {
        get { return _nav; }
    }
    
    List<GameObject> Pos = new List<GameObject>();
	List<Vector3> ePos = new List<Vector3>();

	private int destPoint = 0;
	bool eflag = false;

    public void SetTarget(GameObject Target)
    {
        _nav.SetDestination(Target.transform.position);
    }

    public void GotoNextPoint(List<GameObject> points)
    {
		try
		{
			// Returns if no points have been set up
			if (points.Count == 0 || points == null)
				eflag = true;
			else
				Pos = points;

			if (eflag)
			{
				RandomAI();
				return;
			}

			// Set the agent to go to the currently selected destination.
			_nav.destination = Pos[destPoint].transform.position;

			// Choose the next point in the array as the destination,
			// cycling to the start if necessary.
			destPoint = (destPoint + 1) % Pos.Count;
		}
		catch (NullReferenceException e)
		{
			//Debug.Log("nade");
			RandStart(4);
			eflag = true;
		}
    }

    //AIを止めたり動かし始めたりする関数
    public void AgentMoveBoolFunc(bool flag)
    {
        _nav.isStopped = !flag;
    }

	public void RandStart(int num)
	{
		
		for (int i = 0; i <= num; i++)
		{
			Vector2 xy = UnityEngine.Random.insideUnitCircle;
			ePos.Add((transform.forward + new Vector3(xy.x, 0, xy.y) )* 10.0f + transform.localPosition);
		}
			
	}

	public void RandomAI()
	{
		_nav.destination = ePos[destPoint++];
		if (destPoint == ePos.Count)
		{
			ePos.Clear();
			RandStart(destPoint-1);
			destPoint = 0;
		}
			
	}

	public void ClearFunc()
	{
		ePos.Clear();
		destPoint = 0;
		eflag = false;
	}
}
