using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushEmpty : MonoBehaviour {

	private bool bushFull = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setBushEmpty() 
	{
		bushFull = false;
	}

	public bool getBushState() {
		return bushFull;
	}
}
