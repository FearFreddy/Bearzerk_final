using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawning : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int whichOne = Random.Range (0, 5);
		switch (whichOne) {
		case 0:
			transform.position = new Vector3 (467f, 12f, 476f);
			break;
		case 1:
			transform.position = new Vector3 (485f, 12f, 312f);
			break;
		case 2:
			transform.position = new Vector3 (480f, 12f, 77f);
			break;
		case 3:
			transform.position = new Vector3 (300f, 12f, 482f);
			break;
		case 4:
			transform.position = new Vector3 (110f, 12f, 479f);
			break;
		}
		

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
