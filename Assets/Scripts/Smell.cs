// This complete script can be attached to a camera to make it
// continuously point at another object.

// The target variable shows up as a property in the inspector.
// Drag another object onto it to make the camera look at it.
using UnityEngine;
using System.Collections;

public class Smell : MonoBehaviour
{
	public Transform player;
	public Transform target1;
	public Transform target2;	
	public Transform target3;

	public GameObject bridge;
	BridgeCheck bridgeScript;

	public int direction = 0;


	void Start() 
	{
		bridgeScript = bridge.GetComponent<BridgeCheck> ();
	}
	void Update()
	{
		Direction ();

		// Rotate the camera every frame so it keeps looking at the target
		if (direction == 1)
			transform.LookAt(target1);
		else if (direction==2)
			transform.LookAt(target2);
		else if (direction == 3)
			transform.LookAt(target3);


	}

	private void Direction(){
	
		float distance1 = Vector3.Distance (player.transform.position, target1.transform.position);
		float distance2 = Vector3.Distance (player.transform.position, target2.transform.position);
		float distance3 = Vector3.Distance (player.transform.position, target3.transform.position);

		if ((distance1 > 20 && !bridgeScript.DidItHappen()) || (bridgeScript.DidItHappen() && distance1 < distance2)) {
			direction = 1;
		}
		else if (distance2 > 15)
			direction = 2;
		else if (distance3 > 15)
			direction = 3;
		else 
			transform.LookAt(player);
	}
}