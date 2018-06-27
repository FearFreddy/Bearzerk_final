using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildrenManager : MonoBehaviour {

	public GameObject child1, child2, child3;

	public Transform[] spawnPointsFirstChild;
	public Transform[] spawnPointsSecondChild;

	// Use this for initialization
	void Start () {
		int spawnPointIndex = Random.Range (0, spawnPointsFirstChild.Length);
		Vector3 fuck = spawnPointsFirstChild [spawnPointIndex].localPosition;
		child1.SetActive(false);
		child1.transform.position += fuck;
		child1.SetActive(true);



		int spawnPointIndex2 = Random.Range (0, spawnPointsSecondChild.Length);
		Vector3 fuckEy = spawnPointsSecondChild [spawnPointIndex2].localPosition;
		child2.SetActive(false);
		child2.transform.position += fuckEy;
		child2.SetActive(true);
	}
	

}
