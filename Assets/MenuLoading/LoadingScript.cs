using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScript : MonoBehaviour {

	AsyncOperation ao;
	public GameObject loadingScreenBG;
	public Slider progBar;
	public Text loadingText;
	public GameObject enterForest;

	public bool isFakeLoadingBar = false;
	public float fakeIncrement = 0f;
	public float fakeTiming = 0f;



	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadLevel01 () {

		loadingScreenBG.SetActive (true);
		progBar.gameObject.SetActive (true);
		loadingText.gameObject.SetActive (true);
		enterForest.SetActive (false);

		if (!isFakeLoadingBar) 
		{
			StartCoroutine (LoadLevelWithRealProgress ());

		} else {
		
			StartCoroutine (LoadLevelWithFakeProgress ());
		}
	}

	IEnumerator LoadLevelWithRealProgress()
	{
		yield return new WaitForSeconds (1);

		ao = SceneManager.LoadSceneAsync ("forest");
		ao.allowSceneActivation = false;

		while (!ao.isDone) {
			progBar.value = ao.progress;

			if (ao.progress == 0.9f) {

				progBar.value = 1f;
				loadingText.text = "Press 'Return' to continue";
				if (Input.GetKeyDown (KeyCode.Return)) {
					ao.allowSceneActivation = true;
				}
			}

			Debug.Log (ao.progress);
			yield return null;
	
		}
	}

	IEnumerator LoadLevelWithFakeProgress()
	{
		yield return new WaitForSeconds (1);

		while (progBar.value != 1f) 
		{
			progBar.value += fakeIncrement;
			yield return new WaitForSeconds (fakeTiming);
		}

		while (progBar.value == 1f)
		{
			loadingText.text = "Press 'Return' to continue";
			if (Input.GetKeyDown (KeyCode.Return)) 
			{
				SceneManager.LoadScene ("forest");
			}
			yield return null;
		}
	}

}
