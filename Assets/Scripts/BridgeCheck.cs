using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeCheck : MonoBehaviour {

	private bool bearPassedBy = false;
	private bool happenedAlready = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player" && !happenedAlready) {
			bearPassedBy = true;
			happenedAlready = true;
		}
	}

	public bool IsBearOverBridge() 
	{
		return bearPassedBy;
	}

	public bool DidItHappen() {
		return happenedAlready;
	}

	public void BearPassedBridge() 
	{
		bearPassedBy = false;
	}
}
