using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEmission : MonoBehaviour {
	
	ParticleSystem ps;
	Rigidbody rb;
	MeshRenderer mr;
	ParticleSystem.EmissionModule em;
	private GameObject terrain;

	AudioSource playerAudio;

	// Use this for initialization
	void Start () {
		ps = GetComponent<ParticleSystem> ();
		rb = GetComponent<Rigidbody> ();
		mr = GetComponent<MeshRenderer> ();
		em = ps.emission;
		em.enabled = true;
		terrain = GameObject.FindGameObjectWithTag("Terrain");
		Physics.IgnoreCollision (terrain.GetComponent<TerrainCollider> (), GetComponent<CapsuleCollider>());
		Physics.IgnoreCollision (terrain.GetComponent<TerrainCollider> (), GetComponent<SphereCollider>());

		playerAudio = GetComponent <AudioSource> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if (rb.useGravity == true && transform.position.y > 12) 
		{
			if (!playerAudio.isPlaying) {
				playerAudio.Play ();
				StartCoroutine (Destroy ());
			}
			em.rateOverTime = 500;

		}

		if (transform.position.y < 12) 
		{
			Physics.IgnoreCollision (terrain.GetComponent<TerrainCollider> (), GetComponent<SphereCollider>(), false);
		}
	}

	void OnCollisionEnter() 
	{
		if (transform.position.y < 12) {
			mr.enabled = false;
			em.enabled = false;
			em.rateOverTime = 0;
		}
	}

	IEnumerator Destroy() 
	{
		yield return new WaitForSeconds (10);
		Destroy (this.gameObject);
	}
}
