using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wolf_KI : MonoBehaviour {

	public int startingHealth = 100;
	private int currentHealth;
	public float enemyMovementSpeed;

	public Animator anim;
	private Rigidbody rb;
	private Transform fpsTarget; // Target of the enemy -> The Player or a child bear

	//Various distances

	public float fpsTargetDistance, moveDistance;

	// Booleans

	private bool canAttack = true;
	private bool running = false;
	private bool somethingInReach = false;
	bool isDead = false;
	private bool randomWalking = false;
	private bool packMemberInReachOfSomething = false;
	private bool packMemberSeesPlayer = false;
	private bool enoughMembersLeft = true;

	//Pointers to player components

	GameObject player;
	GameObject theClosestChild;
	GameObject child1, child2;

	PlayerHealth playerHealth;
	PlayerMovement playerMovement;

	private Transform childBearTrans;

	//Pointer to SpriteRenderer & sprites

	SpriteRenderer damage;
	public Sprite sprite1;
	public Sprite sprite2;

	//other things

	private int randomNumber;		//will be used for random walk of wolf closer to the child bear
	private float falling = 0f;		//necessary for dying (lets him fall to the side)
	
	NavMeshAgent agent;
	NavMeshPath playerPath,childPath,runAwayPath;

	ParticleSystem ps;
	ParticleSystem.EmissionModule em;

	private Transform endOfMap;

	GameObject bridgeCheckpoint2;
	private BridgeCheck bridgeScript;


	// Use this for initialization
	void Start () {
		
		anim = GetComponent<Animator>();

		currentHealth = startingHealth;
		enemyMovementSpeed = Random.Range (10, 16);

		player = GameObject.FindGameObjectWithTag ("Player");
		playerMovement = player.GetComponent <PlayerMovement> ();

		child1 = GameObject.Find ("ChildBear1");
		child2 = GameObject.Find ("ChildBear2");
		theClosestChild = child1;

		damage = GetComponentInChildren<SpriteRenderer> ();
		damage.enabled = false;

		moveDistance = 22;
		randomNumber = 0;

		agent = GetComponent<NavMeshAgent>();

		ps = GetComponent<ParticleSystem> ();
		em = ps.emission;

		em.enabled = false;

		rb = GetComponent<Rigidbody> ();

		fpsTarget = player.transform;		//Transform of the player is the target

		endOfMap = GameObject.FindGameObjectWithTag ("EndOfMap").transform;

		playerPath = new NavMeshPath ();
		childPath = new NavMeshPath ();
		runAwayPath = new NavMeshPath ();

		agent.CalculatePath (player.transform.position, playerPath);
		agent.CalculatePath (theClosestChild.transform.position, childPath);
		agent.CalculatePath (endOfMap.position, runAwayPath);

		bridgeCheckpoint2 = GameObject.Find ("SecondBridgeCheckpoint");

		bridgeScript = bridgeCheckpoint2.GetComponent<BridgeCheck>();

	}
	
	// Update is called once per frame


	void Update () {

		//IF NOT DEAD YET//

		if (!isDead) {

			if (enoughMembersLeft) {

				if (!bridgeScript.DidItHappen ()) {
					theClosestChild = child1;
				} else {
					theClosestChild = child2;
				}

				if (randomNumber != 1 && !somethingInReach) 
				{
					if(bridgeScript.DidItHappen()) 
						randomNumber = Random.Range (0, 1000);
					else
						randomNumber = Random.Range (0, 350);
				}
				
				//randomNumber between 1 and 1000 gets generated every frame, if it is 1, start walking in direction of child bear
				
				if (randomNumber == 1 && !packMemberInReachOfSomething && enoughMembersLeft) 
				{	
					StartRandomWalk ();
				}
				
				//distance between players position and own position gets calculated

				float tempDistance;
				tempDistance = Vector3.Distance (player.transform.position, transform.position);
				fpsTargetDistance = tempDistance;

				if (tempDistance < moveDistance || packMemberSeesPlayer) 
				{
					fpsTarget = player.transform;
				} 
				else if (theClosestChild != null) //MOM BEAR NOT IN LOOK DISTANCE, LOOK IF A CHILD IS IN REACH
				{
					tempDistance = Vector3.Distance (theClosestChild.transform.position, transform.position); 
					fpsTarget = theClosestChild.transform;
					fpsTargetDistance = tempDistance;

				} 
				else 
				{
					fpsTarget = player.transform;
				}

				//Check if something is in Reach (moveDistance) of this wolf

				if (fpsTargetDistance < moveDistance) 
				{
					somethingInReach = true;
				} 
				else 
				{
					somethingInReach = false;
				}

				// packMemberInReachOfSomething is true as soon as one of the wolves (including this one) is in reach of a child or the player

				if (packMemberInReachOfSomething) 
				{

					if (fpsTarget == player.transform && somethingInReach) 
					{
						packMemberSeesPlayer = true;
					}

					//canAttack is only false for a few seconds after he attacked

					if (canAttack) 
					{	
						MoveTowardsPlayerOrChild ();
						running = true;
					} 
					else 
					{
						MoveAwayFromPlayerOrChild ();	//creeps away in this function
						running = false;
					} 
				} 



			}
				

		} 

		else {

			agent.enabled = false;	//Nav Mesh Agent disabled

			rb.isKinematic = true;	//Rigidbody properties
			rb.freezeRotation = true;

			//Falling to the side 'animation' until 90 degrees

			if (falling < 90f) {
				Vector3 oldRot = transform.rotation.eulerAngles;
				transform.rotation = Quaternion.Euler (oldRot.x, oldRot.y, falling);

				falling += 3.5f * Time.deltaTime *20f;

				//little boost from 65 degrees (exponential falling growth)

				if (falling > 50f) {
					falling += 7f * Time.deltaTime *20f;
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
		anim.SetBool ("running", running);
		anim.SetBool ("enoughMembers", enoughMembersLeft);	
		anim.SetBool ("playerInReach", packMemberInReachOfSomething);
		anim.SetBool ("randomWalking", randomWalking);
		anim.SetBool ("canAttack", canAttack);
	}




	////////////////////////// 				FUNCTIONS 			//////////////////////////
		
		

	void MoveTowardsPlayerOrChild()
	{	
		agent.enabled = true;
		agent.CalculatePath (fpsTarget.transform.position, playerPath);
		Quaternion rotation = Quaternion.LookRotation (fpsTarget.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime*2);
		agent.speed = enemyMovementSpeed / 2;
		agent.path = playerPath;
	}

	public void RunAwayFromPlayer() 
	{	
		enoughMembersLeft = false;
		running = true;
		agent.enabled = true;
		agent.CalculatePath (endOfMap.position, runAwayPath);
		agent.speed = enemyMovementSpeed / 2;
		agent.path = runAwayPath;

	}

	void MoveAwayFromPlayerOrChild()
	{	
		Quaternion rotation = Quaternion.LookRotation (fpsTarget.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime*2);

		Vector3 oldRot = transform.rotation.eulerAngles;
		transform.rotation = Quaternion.Euler (0, oldRot.y, oldRot.z);

		agent.enabled = false;
		transform.position -= transform.forward * enemyMovementSpeed * 0.0025f;
	}

	void AttackPlayer(GameObject attackedObject)
	{
		if (attackedObject.tag == "ChildBear" && packMemberSeesPlayer) {
			//do not attack the child if some member of the pack sees the player
		} else {
			running = false;
			canAttack = false;
			playerHealth = attackedObject.GetComponent<PlayerHealth> ();
			playerHealth.TakeDamage (10);


			//set Timeout for attack
			StartCoroutine (setCanAttack ());
		}

	}

	void StartRandomWalk() 
	{
		randomWalking = true;

		agent.enabled = true;
		agent.CalculatePath (fpsTarget.transform.position, childPath);
		agent.speed = enemyMovementSpeed / 9;
		agent.path = childPath;

		StartCoroutine (stopRandomWalk());
	}



	public void TakeDamage (int amount)
	{
		currentHealth -= amount;

		//healthSlider.value = currentHealth;

		//playerAudio.Play ();

		if(currentHealth <= 0 && !isDead)
		{	
			playerMovement.FillBearzerk (10);
			isDead = true;
			anim.speed = 0;
			running = false;
			canAttack = false;
		}
	}


	void OnCollisionStay(Collision other)
	{
		if (other.gameObject == player) 
		{
			Vector3 oldRot = transform.rotation.eulerAngles;
			Quaternion tempRot = Quaternion.Euler (0, oldRot.y, 0);
			transform.rotation = Quaternion.Slerp (transform.rotation, tempRot, Time.deltaTime*2);
		}



		if ((other.gameObject == player || other.gameObject == theClosestChild) && canAttack && running && enoughMembersLeft)
		{
			AttackPlayer (other.gameObject);
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
			TakeDamage (35 * playerMovement.bearzerkFactor);
			damage.enabled = true;
			damage.sprite = sprite1;
			em.enabled = true;
			em.rateOverTime = 500;

			StartCoroutine (stopEmitting());

		} 
		else if (other.gameObject == player && playerMovement.attackingStraight && playerMovement.attacking && hit.collider.gameObject == this.gameObject) 
		{
			playerMovement.attacking = false;
			TakeDamage (65 * playerMovement.bearzerkFactor);
			damage.enabled = true;
			damage.sprite = sprite2;
			em.enabled = true;
			em.rateOverTime = 1500;

			StartCoroutine (stopEmitting());
		}
		
	}

	// GETTER AND SETTER //

	public bool GetInReach()
	{
		return somethingInReach;
	}

	public bool GetInReachOfPlayer()
	{
		return packMemberSeesPlayer;
	}

	public void SetInReach(bool yourBoolean) 
	{
		packMemberInReachOfSomething = yourBoolean;
	}

	public void SetInReachOfPlayer(bool yourBoolean)
	{
		packMemberSeesPlayer = yourBoolean;
	}



	// 				TIMEOUTS			//



	IEnumerator setCanAttack() {
		yield return new WaitForSeconds(4);
		canAttack = true;

	}

	IEnumerator stopEmitting() {
		yield return new WaitForSecondsRealtime(0.4f);
		em.enabled = false;
		damage.enabled = false;
	}


	IEnumerator stopRandomWalk()
	{
		yield return new WaitForSeconds (4);

		randomNumber = 0;
		randomWalking = false;
		agent.enabled = false;

	}
		


}