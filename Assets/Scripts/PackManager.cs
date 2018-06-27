using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackManager : MonoBehaviour {

	Wolf_KI [] scripts;
	GameObject thisOne;
	Wolf_KI testNull;
 	GameObject player;

	EnemyManager enemyManagerScript;

	AudioSource howling;

	int defaultCount;

	private bool nobodyInReach = true;

	private bool packAlive = true;

	GameObject bridge2;
	BridgeCheck bridgeScript;


	// Use this for initialization

	void Start () {

		player = GameObject.FindGameObjectWithTag ("Player");

		howling = GetComponent<AudioSource> ();

		defaultCount = transform.childCount;
		
		thisOne = gameObject;
		scripts = new Wolf_KI[transform.childCount];

		enemyManagerScript = GameObject.Find ("PrefabManager").GetComponent<EnemyManager> ();

		for (int i = 0; i < transform.childCount; i++) 
		{
			testNull = thisOne.transform.Find("Wolf" + (i +1)).GetComponent<Wolf_KI>();

			if (testNull != null) 
			{
				scripts [i] = thisOne.transform.Find("Wolf" + (i +1)).GetComponent<Wolf_KI>();
			}
		}

		bridge2 = GameObject.FindGameObjectWithTag ("Bridge2");

		bridgeScript = bridge2.GetComponent<BridgeCheck> ();
	}
	
	// Update is called once per frame

	void Update () 
	{
		float tempDistance;
		Transform ofAChild = this.gameObject.transform.GetChild (0).transform;
		tempDistance = Vector3.Distance (player.transform.position, ofAChild.position);
		float tempVolume;
		tempVolume = tempDistance / 50;

		if (tempVolume > 1) {
			tempVolume = 1;
		}

		howling.volume = 1 - tempVolume;
		nobodyInReach = true;

		for (int i = 0; i < scripts.Length; i++) 
		{
			if (scripts[i].GetInReach()) 
			{
				LetThemAllAttack ();
				nobodyInReach = false;
			}

			if (scripts [i].GetInReachOfPlayer()) 
			{
				SetAllInReachOfPlayer ();
			}
		}

		if (nobodyInReach) 
		{
			SetReachBack ();
		}

		if (transform.childCount <= (defaultCount / 3)) 
		{
			RunAway ();

			if (packAlive) {
				packAlive = false;
				if (bridgeScript.DidItHappen ()) {
					enemyManagerScript.spawnNewPack2 ();
				}
				enemyManagerScript.spawnNewPack ();
				StartCoroutine (killPack ());
			}
		}
	}





	// OTHER FUNCTIONS //



	void LetThemAllAttack()
	{
		for (int i = 0; i < scripts.Length; i++) 
		{	
			if (scripts [i] != null) 
			{
				scripts[i].SetInReach (true);
			}

		}
	}

	void SetReachBack()
	{
		for (int i = 0; i < scripts.Length; i++) 
		{	
			if (scripts [i] != null) 
			{
				scripts[i].SetInReach (false);
			}

		}
	}

	void SetAllInReachOfPlayer()
	{
		for (int i = 0; i < scripts.Length; i++) 
		{
			scripts [i].SetInReachOfPlayer (true);
		}
	}

	void RunAway()
	{
		for (int i = 0; i < scripts.Length; i++) 
		{
			if (scripts [i] != null) 
			{
				scripts [i].RunAwayFromPlayer ();
			}
		}
	}

	IEnumerator killPack() {
		yield return new WaitForSeconds (20);
		Destroy (this.gameObject);
	}
}
