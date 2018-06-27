using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemys;
	public GameObject honey;
    public Transform[] spawnPoints;
	public Transform[] spawnPointsSide2;
	public Transform[] spawnPointsHoney;
	public Transform[] spawnPointsHoney2;

	private int[] alreadySpawned;
	private int[] alreadySpawned2;
	private int[] alreadySpawnedHoney;

	public GameObject bridgeCheckpoint;
	public GameObject bridgeCheckpoint2;
	BridgeCheck bridgeScript, bridgeScript2;

	private int packsSpawned, packsSpawned2;

	private bool overSecondBridge = false;
	private bool overFirstBridge = false;


    void Start ()
    {

		Instantiate (enemys[0], spawnPoints[0].position, spawnPoints[0].rotation);

		bridgeScript = bridgeCheckpoint.GetComponent<BridgeCheck> ();

		bridgeScript2 = bridgeCheckpoint2.GetComponent<BridgeCheck> ();

		alreadySpawned = new int[12];
		alreadySpawned2 = new int[12];
		alreadySpawnedHoney = new int[12];

		packsSpawned = 1;
		packsSpawned2 = 0;

		alreadySpawned [0] = 0;

	}

	void Update()
	{
		// First Checkpoint -> Side 1

		if (bridgeScript.IsBearOverBridge() && !overFirstBridge) 
		{
			overFirstBridge = true;
			for (int i = 0; i < 8; i++) {

				StartCoroutine (SpawningFirst (i));

			}
		}

		//Second Checkpoint -> Side 2

		if (bridgeScript2.IsBearOverBridge() && !overSecondBridge) 
		{
			overSecondBridge = true;
			for (int i = 0; i < 8; i++) {
				StartCoroutine (SpawningSecond (i));

			}
		}
	}

	bool spawnPointAlreadyInThere(int numberToCheck, int [] arrayToCheck) 
	{
		bool isInThere = false;

		for (int i = 0; i < arrayToCheck.Length; i++) 
		{
			if (numberToCheck == arrayToCheck [i]) 
			{
				isInThere = true;
			}
		}

		return isInThere;
	}

	public void spawnNewPack() 
	{
		if (packsSpawned < 12) {
			
			Random.InitState (System.DateTime.Now.Millisecond);
			int spawnPointIndex = 1;

			int whichPack = Random.Range (0, 4);

			do {
				spawnPointIndex = Random.Range (0, spawnPoints.Length);
			} while(spawnPointAlreadyInThere (spawnPointIndex, alreadySpawned)); 

			alreadySpawned [packsSpawned] = spawnPointIndex;

			Instantiate (enemys [whichPack], spawnPoints [spawnPointIndex].position, spawnPoints [spawnPointIndex].rotation);

			packsSpawned += 1;
		} 

	}

	public void spawnNewPack2() 
	{
		if (packsSpawned2 < 12) {

			Random.InitState (System.DateTime.Now.Millisecond);
			int spawnPointIndex = 1;

			int whichPack = Random.Range (0, 4);

			do {
				spawnPointIndex = Random.Range (0, spawnPointsSide2.Length);
			} while(spawnPointAlreadyInThere (spawnPointIndex, alreadySpawned2)); 

			alreadySpawned [packsSpawned2] = spawnPointIndex;

			Instantiate (enemys [whichPack], spawnPointsSide2 [spawnPointIndex].position, spawnPointsSide2 [spawnPointIndex].rotation);

			packsSpawned2 += 1;
		} 

	}

	IEnumerator SpawningFirst(int number) {
		yield return new WaitForSeconds (number);

		Random.InitState (System.DateTime.Now.Millisecond);
		int spawnPointIndex = 1;

		int whichPack = Random.Range (0, 4);

		do {
			spawnPointIndex = Random.Range (1, spawnPoints.Length);
		} while(spawnPointAlreadyInThere (spawnPointIndex, alreadySpawned)); 

		alreadySpawned [packsSpawned] = spawnPointIndex;
		packsSpawned += 1;

		Instantiate (enemys[whichPack], spawnPoints [spawnPointIndex].position, spawnPoints [spawnPointIndex].rotation);
		Instantiate (honey, spawnPointsHoney [spawnPointIndex].position, spawnPointsHoney [spawnPointIndex].rotation);

		if (number == 6) {
			bridgeScript.BearPassedBridge ();
		}

	}

	IEnumerator SpawningSecond(int number) {
		yield return new WaitForSeconds (number);

		Random.InitState (System.DateTime.Now.Millisecond);
		int spawnPointIndex = 1;

		int whichPack = Random.Range (0, 4);

		do {
			spawnPointIndex = Random.Range (0, spawnPointsSide2.Length);
		} while(spawnPointAlreadyInThere (spawnPointIndex, alreadySpawned2)); 

		alreadySpawned2 [packsSpawned2] = spawnPointIndex;

		packsSpawned2++;

		Instantiate (enemys[whichPack], spawnPointsSide2 [spawnPointIndex].position, spawnPointsSide2 [spawnPointIndex].rotation);

		Instantiate (honey, spawnPointsHoney2 [spawnPointIndex].position, spawnPointsHoney2 [spawnPointIndex].rotation);



		if (number == 6) {
			bridgeScript2.BearPassedBridge ();
		}

	}


		


}
