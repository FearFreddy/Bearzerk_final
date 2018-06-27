using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour {

	public Canvas pauseMenu;
	public Canvas quitMenu;
	public Canvas quitMainMenu;
	public Canvas controls;
	public Button mainMenuText;
	public Button settingsText;
	public Button controlText;
	public Button quitText;

	private AudioSource[] allAudioSources;

	// Use this for initialization
	void Start () {

		pauseMenu = pauseMenu.GetComponent<Canvas> ();
		quitMenu = quitMenu.GetComponent<Canvas> ();
		quitMainMenu = quitMainMenu.GetComponent<Canvas> ();
		controls = controls.GetComponent<Canvas> ();
		mainMenuText = mainMenuText.GetComponent<Button> ();
		settingsText = settingsText.GetComponent<Button> ();
		controlText = controlText.GetComponent<Button> ();
		quitText = quitText.GetComponent<Button> ();
		quitMenu.enabled = false;
		quitMainMenu.enabled = false;
		pauseMenu.enabled = false;
		controls.enabled = false;

	}

	// Update is called once per frame
	void Update()
	{


		if (Input.GetKeyDown(KeyCode.Escape))
		{
			pauseMenu.enabled = !pauseMenu.enabled;

			if (!pauseMenu.enabled) {
				quitMenu.enabled = false;
				quitMainMenu.enabled = false;
				controls.enabled = false;
			} else {
				mainMenuText.enabled = true;
				settingsText.enabled = true;
				controlText.enabled = true;
				quitText.enabled = true;
			}

			Pause();
		}
	}
	public void QuitMenuPress(){

		quitMenu.enabled = true;
		controls.enabled = false;
		quitMainMenu.enabled = false;
		mainMenuText.enabled = false;
		settingsText.enabled = false;
		controlText.enabled = false;
		quitText.enabled = false;
	}

	public void QuitMainMenuPress(){

		quitMainMenu.enabled = true;
		quitMenu.enabled = false;
		controls.enabled = false;
		mainMenuText.enabled = false;
		settingsText.enabled = false;
		controlText.enabled = false;
		quitText.enabled = false;
	}

	public void NoPress(){

		quitMenu.enabled = false;
		quitMainMenu.enabled = false;
		controls.enabled = false;
		mainMenuText.enabled = true;
		settingsText.enabled = true;
		controlText.enabled = true;
		quitText.enabled = true;
	}

	public void controlsPress(){

		quitMenu.enabled = false;
		quitMainMenu.enabled = false;
		controls.enabled = true;
		mainMenuText.enabled = false;
		settingsText.enabled = false;
		controlText.enabled = false;
		quitText.enabled = false;
	}

	public void StartMain(){

		SceneManager.LoadScene ("StartMenu");
		SceneManager.UnloadSceneAsync ("forest");
	}

	public void ExitGame(){

		Application.Quit ();

	}

	public void Pause()
	{
		Time.timeScale = Time.timeScale == 0 ? 1 : 0;
		PauseAllAudio ();
	}

	void PauseAllAudio() {
		allAudioSources = FindObjectsOfType (typeof(AudioSource)) as AudioSource[];

		foreach (AudioSource audioS in allAudioSources) {
			audioS.mute = !audioS.mute;
		}
	}
}
 