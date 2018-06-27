using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fadeAttackBig : MonoBehaviour {

	public GameObject player;
	PlayerMovement playerMovement;
	//public Sprite sprite1, sprite2;

	// Use this for initialization
	void Start () {
		playerMovement = player.GetComponent <PlayerMovement> ();
	}

	// Update is called once per frame
	void Update () {
		Image sprite = GetComponent<Image> ();

		if (playerMovement.attackingStraight) {
			sprite.color = new Color32 (255, 255, 255, 50);
			//sprite.sprite = sprite2;
		} else {
			sprite.color = new Color32 (255, 255, 255, 255);
			//sprite.sprite = sprite1;
		}
	}
}
