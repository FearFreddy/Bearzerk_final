using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {

	Canvas canvas;
	// Use this for initialization
	void Start () {
		canvas = GetComponent<Canvas>();

	}
	
	// Update is called once per frame
	void Update()
	{


		if (Input.GetKeyDown(KeyCode.Escape))
		{
			canvas.enabled = !canvas.enabled;
			Pause();
		}
	}

	public void Pause()
	{
		Time.timeScale = Time.timeScale == 0 ? 1 : 0;
	}
}
