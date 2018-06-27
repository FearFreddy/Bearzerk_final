using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lerp : MonoBehaviour {

	//Bäume
	public GameObject tree1;
	public GameObject tree2;
	public GameObject door;

	//Anfangsposition und Rotation (Baum 1)
	private Vector3 startPos;
	private Quaternion startRot;

	//Endposition und Rotation (Baum 2)
	private Vector3 endPos;
	private Quaternion endRot;

	//Lebenspunkte und Heilung pro Frame
	public float health = 100f;
	public float heal = 0.025f;

	//Die Zeit in Sek. wie lange der Baum fällt (von Position B1 zu B2)
	public float lerpTime = 3;

	//Wie lang der Baum schon fällt
	public float currentLerpTime = 0;

	private bool dead = false;

	//Sound
	public AudioClip aud;
	AudioSource audio;
	private bool alreadyPlayed = false;

	//Player
	public GameObject player;
	PlayerMovement script;

	bool justAttacked = false;
	bool justAttackedBig = false;

	public GameObject InfoText;

	public Slider treeLifeSlider;
	public GameObject treeLife;

	bool thisStartedIt = false;

	// Use this for initialization
	void Start () {

		startPos = tree1.transform.position;
		startRot = tree1.transform.rotation;
		endPos = tree2.transform.position;
		endRot = tree2.transform.rotation;
		audio = GetComponent<AudioSource> ();
		script = player.GetComponent<PlayerMovement> ();

		treeLife.SetActive(false);

	}

	// Update is called once per frame
	void Update () {

		if (!dead) {

			treeLifeSlider.value = health;

			RaycastHit hit;
			Vector3 forward = player.transform.TransformDirection (Vector3.forward);



			//20 Schaden auf Baum pro Angriff

			if (Physics.Raycast (player.transform.position, forward, out hit) && script.attackingStraight && script.attacking && hit.collider.tag == "Baum" && hit.distance < 5 && script.bearzerkOn && script.bearzerk.value > 0 && !justAttackedBig) {
				health -= 9.5f * script.bearzerkFactor;
				justAttackedBig = true;
				StartCoroutine (canAttackBigAgain ());
			}

			if (Physics.Raycast (player.transform.position, forward, out hit) && script.attackingLeft && script.attacking && hit.collider.tag == "Baum" && hit.distance < 5 && script.bearzerkOn && script.bearzerk.value > 0 && !justAttacked) {
				health -= 4.5f * script.bearzerkFactor;
				justAttacked = true;
				StartCoroutine (canAttackAgain ());
			}

			if (Physics.Raycast (player.transform.position, forward, out hit) && script.attacking && hit.collider.tag == "Baum" && hit.distance < 5 && script.bearzerkOn == false && !justAttacked) {
				health -= 2f * script.bearzerkFactor;
				justAttacked = true;
				StartCoroutine (canAttackAgain ());
			}

			if (Physics.Raycast (player.transform.position, forward, out hit) && hit.collider.tag == "Baum" && hit.distance < 5) {
				thisStartedIt = true;
				treeLife.SetActive(true);
				InfoText.SetActive (true);
				Text tempText = InfoText.GetComponent<Text> ();
				tempText.text = "Attack the tree with power!";
			} 
			else if(thisStartedIt)
			{
				InfoText.SetActive (false);
				treeLife.SetActive(false);
				thisStartedIt = false;
			}

			if (health <= 0) {
				dead = true;
			}

			//Heilung pro Frame
			if (health < 100 && dead == false)
				health += heal;
		
			//Berechnung der Zeit die der Baum fällt, nachdem er "getötet" wurde 
			//und Positions- und Rotationsveränderung abhängig von der Fallzeit

		}
		else {

			InfoText.SetActive (false);
			treeLife.SetActive(false);
			currentLerpTime += Time.deltaTime;

			if (!alreadyPlayed){
				audio.Play ();
				alreadyPlayed = true;
			}
			if(currentLerpTime>= lerpTime){
				currentLerpTime = lerpTime;
			}

			if (currentLerpTime > 3) {
				door.SetActive(false);
			}

			float Perc = currentLerpTime / lerpTime;
			tree1.transform.position = Vector3.Lerp (startPos, endPos, Perc);
			tree1.transform.rotation = Quaternion.Slerp (startRot, endRot, Perc);
		}
	}

	IEnumerator canAttackAgain() 
	{
		yield return new WaitForSecondsRealtime (1.5f);
		justAttacked = false;
	}

	IEnumerator canAttackBigAgain() 
	{
		yield return new WaitForSecondsRealtime (3.5f);
		justAttackedBig = false;
	}
}
