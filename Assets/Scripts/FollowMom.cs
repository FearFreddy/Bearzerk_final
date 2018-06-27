using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowMom : MonoBehaviour {
	
	private Animator anim;
	NavMeshAgent agent;
	private Transform player;
	private float theDistance;
	public float childSpeed;
	private float velo;

	PlayerMovement script;

	bool followingMom = false;


	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		agent.speed = childSpeed;
		script = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement> ();
	}
	
	// Update is called once per frame
	void Update () {

			player = GameObject.FindGameObjectWithTag ("Player").transform;

			theDistance = Vector3.Distance (player.position, transform.position);
			anim = GetComponent<Animator> ();

			velo = 0;
			theDistance = Vector3.Distance (player.position, transform.position);

			if (theDistance < 20 && !followingMom) {
				script.setFollowing (true);
				followingMom = true;
			}

			if (theDistance > 20 && followingMom) {
				script.setFollowing (false);
				followingMom = false;
			}

			if (theDistance < 20 && theDistance > 5) {
				velo = 1;
				agent.destination = player.position;

			} else if (theDistance < 5 || theDistance > 20) {
				velo = 0;
				agent.destination = transform.position;
			}
			anim.speed = 1.5f;
			anim.SetFloat ("inputV", velo);
	}

		
}
