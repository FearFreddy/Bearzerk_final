using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

	public Animator anim;

	private float inputH, inputV;
	private float moveX, moveZ;

	private bool running;
	public bool attacking,attackingLeft,attackingStraight;

	public AudioClip attackSound, attackSound2, bigAttackSound, bigAttackSound2,hugeAttackSound,walkingSound;
	private int whichSound = 1;

	AudioSource playerAudio;

	public GameObject InfoText;

	private bool honeyIsHere = false;
	private bool bushIsHere = false;

	public bool bearzerkOn = false;

	public Slider ausdauer, bearzerk, leben;

	public int bearzerkFactor = 1;

	ParticleSystem ps;
	ParticleSystem.EmissionModule em;

	PlayerHealth myHealth;

	public GameObject boss;

	public GameObject endText;
	Boss_KI bossScript;

	private int kidsFollowing = 0;


	// Use this for initialization
	void Start ()
	{	
		anim = GetComponent<Animator>();
		playerAudio = GetComponent <AudioSource> ();
		myHealth = GetComponent<PlayerHealth> ();

		running = false;
		attacking = false;
		attackingLeft = false;
		attackingStraight = false;
	
		// Mauszeiger verbergen/zeigen
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		ps = GetComponent<ParticleSystem> ();
		em = ps.emission;

		em.enabled = false;

		bossScript = boss.GetComponent<Boss_KI> ();
	
	}

	// Update is called once per frame
	void Update ()
	{

		if (bossScript.bossIsDefeated) {
			Text tempText = endText.GetComponent<Text> ();
			if(kidsFollowing == 1) 
				tempText.text = "SPIEL VORBEI! \n Du hast 1 Kind gerettet\n Eine passable Leistung!";
			else if (kidsFollowing == 2) 
				tempText.text = "SPIEL VORBEI! \n Du hast 2 Kinder gerettet\n Eine mutige Bärenmutter!";
			else if (kidsFollowing == 3)
				tempText.text = "SPIEL VORBEI! \n Du hast 3 Kinder gerettet\n Unglaublich, wieviel Willen du gezeigt hast!";
			else tempText.text = "SPIEL VORBEI! \n Du hast kein Kind gerettet\n Was war da los?";

			endText.SetActive (true);
		}

		// Bearzerk-Mode

		if (Input.GetKey (KeyCode.Space)) {
			bearzerkOn = true;
		} else
			bearzerkOn = false;

		if (bearzerk.value == 0) {
			bearzerkOn = false;
		}

		if (bearzerkOn && bearzerk.value > 0) 
		{
			bearzerkFactor = 2;	
			bearzerk.value -= 0.15f * Time.deltaTime * 25f;
			em.enabled = true;
			em.rateOverTime = 200;
		} 
		else
		{
			bearzerkFactor = 1;
			em.enabled = false;
		}

		//Raycasts for Power-Ups

		RaycastHit hit;
		Vector3 forward = transform.TransformDirection(Vector3.forward);

		// Honey

		if (Physics.Raycast (transform.position, forward, out hit) && hit.collider.tag == "Honey" && hit.distance < 4 && hit.rigidbody.useGravity == false) {
			InfoText.SetActive (true);
			honeyIsHere = true;

			Text tempText = InfoText.GetComponent<Text> ();
			tempText.text = "Attack the tree to get the honey";

			if (attacking) {
				myHealth.Heal(200);
				hit.rigidbody.useGravity = true;
				InfoText.SetActive (false);
			} 
			
		} 
		else if (honeyIsHere) {
			InfoText.SetActive (false);
			honeyIsHere = false;
		} 

		// Bush

		else if (Physics.Raycast (transform.position, forward, out hit) && hit.collider.tag == "Bush" && hit.distance < 2) {
			InfoText.SetActive (true);
			bushIsHere = true;

			BushEmpty scriptBush = hit.collider.GetComponent<BushEmpty> ();

			if (scriptBush.getBushState ()) {
				Text tempText = InfoText.GetComponent<Text> ();
				tempText.text = "Press E to eat berries";
			} else {
				InfoText.SetActive (false);
			}

			if (Input.GetKey (KeyCode.E) && scriptBush.getBushState()) {
				myHealth.Heal(100);
				scriptBush.setBushEmpty();
			}

		} 
		else if (bushIsHere) {
			InfoText.SetActive (false);
			bushIsHere = false;
		}

		// Speed up animations a little bit

		anim.speed = 1.5f;
		
		//Show / hide Mousecursor on Escape

		if (Input.GetKeyDown (KeyCode.Escape)) {
			Cursor.visible = !Cursor.visible;
			if (Cursor.visible == false) {
				Cursor.lockState = CursorLockMode.Locked;
			} else {
				Cursor.lockState = CursorLockMode.Confined;
			}
		}
			
	
		//Bei MouseDown Attackieren

		if(Input.GetMouseButtonDown(0) && attackingStraight == false && attacking == false)
		{
			anim.Play("attack_straight",-1,0f);
			attackingStraight = true;



			whichSound = Random.Range (1, 3);
			if (whichSound == 1) {
				playerAudio.clip = bigAttackSound;
			} else {
				playerAudio.clip = bigAttackSound2;
			}

			if (bearzerkOn) {
				playerAudio.clip = hugeAttackSound;
			}
			playerAudio.Play ();

			//Timeouts
			StartCoroutine (attackStraightTimeout());
			StartCoroutine (attackStartTimeout ());
			StartCoroutine (attackEndTimeout ());

		}

		if (Input.GetMouseButtonDown (1) && attackingLeft == false && attacking == false) 
		{
			anim.Play ("attack_left", -1, 0f);
			attackingLeft = true;

			playerAudio.clip = attackSound;

			if (bearzerkOn)
				playerAudio.clip = attackSound2;

			playerAudio.Play ();

			//Timeouts
			StartCoroutine (attackLeftTimeout());
			StartCoroutine (attackStartTimeout ());
			StartCoroutine (attackEndTimeout ());
		}

		// Input der Pfeiltasten holen

		inputH = Input.GetAxis("Horizontal");
		inputV = Input.GetAxis("Vertical");

		// Bei gedrücktem Shift Rennen

		if(Input.GetKey(KeyCode.LeftShift) && ausdauer.value > 0)
		{
			running = true;
			ausdauer.value -= 0.5f * Time.deltaTime * 25;

		}
		else
		{
			running = false;
			if (ausdauer.value < 100) {
				ausdauer.value += 0.1f * Time.deltaTime *25;
			}
		}



		if (inputV == 0 && inputH != 0) {
			inputV = 0.3f;
		}

		// Animatorvariablen setzen

		anim.SetFloat("inputH", inputH);
		anim.SetFloat("inputV", inputV);
		anim.SetBool("run", running);


		MovePlayer();

		RotatePlayer();
	

	}


	//////////////////////////    FUNCTIONS    //////////////////////////////


	void MovePlayer()
	{	
		// Aus Input Movevariablen setzen, je nach gedrückter Shift-Taste (running -> true / false) schneller / langsamer laufen lassen

		moveX = inputH * Time.deltaTime;
		moveZ = inputV * Time.deltaTime;

		if (moveZ != 0 && !attackingLeft && !attackingStraight) {
			if (!playerAudio.isPlaying) {
				playerAudio.clip = walkingSound;
				playerAudio.Play ();
			}
		} else if(playerAudio.clip == walkingSound) {
			playerAudio.Pause ();
		}




		if (running && moveZ > 0) {
			moveZ = inputV * 3.5f;
		} else if (!running && moveZ > 0) {
			moveZ = inputV * 2.5f;
		} else if (running && moveZ < 0) {
			moveZ = inputV * 2.5f;
		} else if (!running && moveZ < 0) {
			moveZ = inputV * 1.5f;
		} 
		//Player nach vorne (hinten) bewegen -> je nach moveZ positiv oder negativ

		if (moveZ != 0) {
			transform.position += transform.forward * moveZ * Time.deltaTime;
			
			//Rotieren auf Z-Achse vermeiden

			Vector3 oldRot = transform.rotation.eulerAngles;
			Quaternion tempRot = Quaternion.Euler (oldRot.x, oldRot.y, 0);
			transform.rotation = Quaternion.Slerp (transform.rotation, tempRot, Time.deltaTime*2);
		}
	}


	void RotatePlayer()
	{	
		//Player nach rechts und links bewegen (rotieren) je nach Vor- bzw. Zurücklaufen

		if (moveX < 0 && moveZ > 0) {
			transform.Rotate (0, -3f, 0);
		} else if (moveX > 0 && moveZ > 0) {
			transform.Rotate (0, 3f, 0);
		} else if (moveX < 0 && moveZ < 0) {
			transform.Rotate (0, 3f, 0);
		} else if (moveX > 0 && moveZ < 0) {
			transform.Rotate (0, -3f, 0);
		} else if (moveX < 0 && moveZ == 0) {
			transform.Rotate (0, -3f, 0);
		} else if (moveX > 0 && moveZ == 0) {
			transform.Rotate (0, 3f, 0);
		} 
			
	}

	public void FillBearzerk(int amount) 
	{
		bearzerk.value += amount;
	}

	// Function to call from child's script to count number of following children

	public void setFollowing(bool isIt) {
		if (isIt) {
			kidsFollowing += 1;
		} else
			kidsFollowing -= 1;
	}

		
	/////////////////////////       TIMEOUTS         ////////////////////////////



	IEnumerator attackStartTimeout()
	{
		yield return new WaitForSecondsRealtime (0.4f);
		attacking = true;
	}

	IEnumerator attackEndTimeout()
	{
		yield return new WaitForSecondsRealtime (0.85f);
		attacking = false;
	}

	IEnumerator attackLeftTimeout()
	{
		yield return new WaitForSecondsRealtime (1.5f);
		attackingLeft = false;
	}

	IEnumerator attackStraightTimeout()
	{
		yield return new WaitForSecondsRealtime (3.5f);
		attackingStraight = false;
	}

}
