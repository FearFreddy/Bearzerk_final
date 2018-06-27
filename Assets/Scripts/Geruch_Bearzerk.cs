using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Geruch_Bearzerk : MonoBehaviour {


	private bool geruch; 
	public int duration;
	public GameObject Geruch;

	public float bearzerk_Bar;

	public GameObject InfoText;

	public Slider smell;

	bool heSmelled = false;

	public GameObject slider;

	// Use this for initialization
	void Start () {
		
		geruch = false;
		smell.value = 0f;
		duration = 5000;

	}
	
	// Update is called once per frame
	void Update () {

		if (smell.value == 0) {
			slider.SetActive (false);
		} else {
			slider.SetActive (true);
		}
		
		// Bei Q Geruchssinn

		if (Input.GetKeyDown (KeyCode.Q)) {
			
			if (geruch == false && smell.value > 0f) {
				Geruch.SetActive (true);
				geruch = true;
			} else if (geruch == true) {
				Geruch.SetActive (false);
				geruch = false;
			}
		} 

		Smell();
	}

	void Smell(){

		if (geruch==true) {			
			smell.value -=  3f * (Time.deltaTime);

			if (smell.value <= 0) {
				geruch = false;
				Geruch.SetActive (false);
			}
		}

	}

	void OnTriggerStay(Collider other) 
	{
		if (other.gameObject.tag == "Poop") {

			if (Input.GetKeyDown (KeyCode.Q) && !heSmelled) {
				heSmelled = true;
				smell.value += 20;
			}

			if (!heSmelled) {
				InfoText.SetActive (true);
				Text tempText = InfoText.GetComponent<Text> ();
				tempText.text = "Press Q to smell the poo";
			}


		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Poop") {
			InfoText.SetActive (false);
		}
	}

}
