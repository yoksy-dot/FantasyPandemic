using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCtrl : MonoBehaviour {

	[SerializeField]
	private ParticleSystem particle;

	IEnumerator ParticleWorking()
	{
		yield return new WaitWhile(() => particle.IsAlive(true));

		gameObject.SetActive(false);
	}

	private void OnDisable()
	{
		
		particle.Stop();
		StopCoroutine(ParticleWorking());
	}
}
