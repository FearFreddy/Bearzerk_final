using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss_KI : MonoBehaviour {

	public int startingHealth = 1000;
	public int currentHealth;

	public Animator anim;
	private Rigidbody rb;
	private Transform fpsTarget; // Target of the Endboss -> The Player

	//Various distances

	private float fpsTargetDistance, moveDistance;

	//Which way is he walking
	private float inputH, inputV;

	// Booleans

	private bool canAttack = true;
	private bool isAttacking = false;
	private bool attackIsOver = true;
	private bool somethingInReach = false;
	private bool inCollisionZone = false;
	bool isDead = false;


	//Pointers to player components

	GameObject player;

	PlayerHealth playerHealth;
	PlayerMovement playerMovement;

	//Pointer to SpriteRenderer & sprites

	SpriteRenderer damage;

	//other things

	private float falling = 0f;		//necessary for dying (lets him fall to the side)

	NavMeshAgent agent;

	private Transform endOfMap;
	NavMeshPath fpsPath;

	public Slider lifeSlider;
	public GameObject life;

	//Sound

	AudioSource playerAudio;

	public AudioClip attack1,attack2,attack3;

	private int whichSound = 1;

	public bool bossIsDefeated = false;

	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator>();

		currentHealth = startingHealth;

		player = GameObject.FindGameObjectWithTag ("Player");
		playerMovement = player.GetComponent <PlayerMovement> ();


		damage = GetComponentInChildren<SpriteRenderer> ();
		damage.enabled = false;

		moveDistance = 30;

		agent = GetComponent<NavMeshAgent>();

		rb = GetComponent<Rigidbody> ();

		fpsTarget = player.transform;		//Transform of the player is the target

		fpsPath = new NavMeshPath ();

		life.SetActive (false);

		playerAudio = GetComponent <AudioSource> ();

	}

	// Update is called once per frame


	void Update () {

		//IF NOT DEAD YET//

		if (!isDead) {

				
				//distance between players position and own position gets calculated

				fpsTargetDistance = Vector3.Distance (player.transform.position, transform.position);

				//Check if something is in Reach (moveDistance) of this wolf

				if (fpsTargetDistance < moveDistance) 
				{
					somethingInReach = true;
					inputV = 1;

					life.SetActive (true);
					lifeSlider.value = currentHealth;
					
					if (fpsTargetDistance < 6.5f) {	
						inCollisionZone = true;
						Debug.Log ("inCollision");

						if (canAttack) {
						if (!playerAudio.isPlaying) {
							//Random Clip playing
							whichSound = Random.Range (1, 4);
							if (whichSound == 1) {
								playerAudio.clip = attack1;
							} else if (whichSound == 2) {
								playerAudio.clip = attack2;
							} else {
								playerAudio.clip = attack3;
							}

							playerAudio.Play ();
						}
						attackIsOver = false;
						//set Timeout for attack
						StartCoroutine (setCanAttack ());
						StartCoroutine (attackStartTimeout ());
						StartCoroutine (attackEndTimeout ());
						}

					} 
					else inCollisionZone = false;

					//canAttack is only false for a few seconds after he attacked

					if (canAttack) 
					{	
						MoveTowardsPlayer ();
					} 
					else if(!isAttacking && attackIsOver)
					{
						MoveAwayFromPlayer ();	//walks away from player in this function
					} 
				} 

				else 
				{
					somethingInReach = false;
					inputV = 0;
				}


		} 
	}

	void FixedUpdate()
	{

		if (isDead) {
			life.SetActive (false);

			bossIsDefeated = true;

			agent.enabled = false;	//Nav Mesh Agent disabled

			rb.isKinematic = true;	//Rigidbody properties
			rb.freezeRotation = true;

			//Falling to the side 'animation' until 90 degrees

			if (falling < 90f) {
				Vector3 oldRot = transform.rotation.eulerAngles;
				transform.rotation = Quaternion.Euler (oldRot.x, oldRot.y, falling);
				 
				falling += 1.5f * Time.deltaTime;

				//little boost from 65 degrees (exponential falling growth)

				if (falling > 60f) {
					falling += 5f * Time.deltaTime;
				}
			} 

			//if reached 90, gameobject will be destroyed

			else {
				Destroy (this.gameObject);
			}

		}
	}

	void LateUpdate() 
	{
		//Animations get triggered if specific booleans are true / false
		anim.SetBool ("inCollisionZone", inCollisionZone);
		anim.SetBool ("playerInReach", somethingInReach);
		anim.SetFloat("inputH", inputH);
		anim.SetFloat("inputV", inputV);
		anim.SetBool ("canAttack", canAttack);
	}




	////////////////////////// 				FUNCTIONS 			//////////////////////////



	void MoveTowardsPlayer()
	{	
		agent.CalculatePath (fpsTarget.position, fpsPath);
		agent.path = fpsPath;

		anim.speed = 1f;
		agent.enabled = true;
		agent.speed = 6;
	}
		

	void MoveAwayFromPlayer()
	{	
		Quaternion rotation = Quaternion.LookRotation (fpsTarget.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime*2);

		Vector3 oldRot = transform.rotation.eulerAngles;
		transform.rotation = Quaternion.Euler (0, oldRot.y, oldRot.z);

		anim.speed = 0.8f;
		inputV = -1;
		agent.enabled = false;
		transform.position -= transform.forward * 15 * 0.004f;
	}

	void AttackPlayer()
	{

		playerHealth = player.GetComponent<PlayerHealth> ();
		playerHealth.TakeDamage (7);


	}
		

	public void TakeDamage (int amount)
	{
		currentHealth -= amount;
	

		if(currentHealth <= 0 && !isDead)
		{	
			isDead = true;
			anim.speed = 0;;
			canAttack = false;
		}
	}


	void OnCollisionStay(Collision other)
	{
		RaycastHit hit;
		Vector3 forward = transform.TransformDirection(Vector3.forward);

		if (Physics.Raycast (transform.position, forward, out hit) && hit.collider.tag == "Player" && other.gameObject == player && isAttacking && inCollisionZone)
		{
			AttackPlayer ();
		}

	}

	void OnTriggerStay(Collider other) 
	{
		// Raycast from Player forward

		RaycastHit hit;
		Vector3 forward = fpsTarget.TransformDirection(Vector3.forward);
		Physics.Raycast (fpsTarget.position, forward, out hit);

		if (other.gameObject == player && playerMovement.attackingLeft && playerMovement.attacking && hit.collider.gameObject == this.gameObject) 
		{	
			playerMovement.attacking = false;
			TakeDamage (25 * playerMovement.bearzerkFactor);
			damage.enabled = true;
			StartCoroutine (DisableImage ());
		} 
		else if (other.gameObject == player && playerMovement.attackingStraight && playerMovement.attacking && hit.collider.gameObject == this.gameObject) 
		{
			playerMovement.attacking = false;
			TakeDamage (50 * playerMovement.bearzerkFactor);
			damage.enabled = true;
			StartCoroutine (DisableImage ());
		}
		
	}




	// 				TIMEOUTS			//

	IEnumerator attackStartTimeout()
	{
		yield return new WaitForSecondsRealtime (0.5f);
		isAttacking = true;
		canAttack = false;
	}

	IEnumerator attackEndTimeout()
	{
		yield return new WaitForSecondsRealtime (1f);
		isAttacking = false;
		attackIsOver = true;
	}

	IEnumerator setCanAttack() {
		yield return new WaitForSecondsRealtime(3f);
		canAttack = true;
	}

	IEnumerator DisableImage() 
	{
		yield return new WaitForSecondsRealtime (0.5f);
		damage.enabled = false;
	}


}