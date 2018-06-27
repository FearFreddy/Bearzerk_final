using UnityEngine;
using System.Collections;

public class ThirdPersonCamera1 : MonoBehaviour
{

	private const float Y_ANGLE_MIN = 30.0f;
	private const float Y_ANGLE_MAX = 50.0f;
	private const float X_ANGLE_MIN = -10.0f;
	private const float X_ANGLE_MAX = 10.0f;

	public Transform lookAt;
	public Transform camTransform;

	private Camera cam;

	private float distance = 8.0f;
	private float currentX = 0.0f;
	private float currentY = 0.0f;

	private float scroll;

	public GameObject baum;
	Lerp baumScript;

	bool startShaking = false;

	GameObject player;

	PlayerMovement playerM;

	private void Start()
	{
		camTransform = transform;
		cam = Camera.main;
		baumScript = baum.GetComponent<Lerp> ();

		player = GameObject.FindGameObjectWithTag ("Player");
		playerM = player.GetComponent<PlayerMovement> ();

	}

	private void Update()
	{
		currentX += Input.GetAxis("Mouse X");
		currentY += Input.GetAxis("Mouse Y");
		scroll = Input.GetAxis ("Mouse ScrollWheel");

		if (scroll > 0 && distance > 5f) 
		{
			distance -= 3;
		} 
		else if (scroll < 0 && distance < 10f) 
		{
			distance += 3;
		}
			

		if (playerM.bearzerkOn)
		{
			currentX += Random.Range (-0.3f, 0.3f);
			currentY += Random.Range (-1.0f, 1.0f);
		}

		if (baumScript.currentLerpTime > 3.9 && !startShaking ) {
			StartCoroutine (stopShaking ());
			currentX += Random.Range (-2f, 2f);
			currentY += Random.Range (-2f, 2f);
		}

		scroll = 0;

		currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
		currentX = Mathf.Clamp(currentX, X_ANGLE_MIN, X_ANGLE_MAX);
	}

	private void LateUpdate()
	{
		Vector3 dir = new Vector3 (0,0f,-distance);

		float angle = 0;

		if (lookAt.eulerAngles.x < 100) 
		{
			angle = lookAt.eulerAngles.x / 2;
		}
			
		Quaternion rotation = Quaternion.Euler(currentY + angle, lookAt.eulerAngles.y + currentX, 0);
		camTransform.position = lookAt.position + rotation * dir;
		Vector3 tempLookAt = lookAt.position + new Vector3 (0, 2f, 0);
		camTransform.LookAt(tempLookAt);
	}

	IEnumerator stopShaking() {
		yield return new WaitForSecondsRealtime (0.6f);
		startShaking = true;
	}

}

