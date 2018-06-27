using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixSprite : MonoBehaviour {

	GameObject Camera;
	// Use this for initialization
	void Start () {
		Camera = GameObject.FindGameObjectWithTag ("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
		var rotation = Camera.transform.rotation;
		transform.rotation = rotation;
	}
}
