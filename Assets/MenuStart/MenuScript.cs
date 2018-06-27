using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	public Canvas quitMenu;
	public Button startText;
	public Button settingsText;
	public Button controlText;
	public Button quitText;
	public Canvas controls;

	// Use this for initialization
	void Start () {

		quitMenu = quitMenu.GetComponent<Canvas> ();
		startText = startText.GetComponent<Button> ();
		settingsText = settingsText.GetComponent<Button> ();
		controlText = controlText.GetComponent<Button> ();
		quitText = quitText.GetComponent<Button> ();
		controls = controls.GetComponent<Canvas> ();
		quitMenu.enabled = false;
		controls.enabled = false;


	}
	
	public void ExitPress(){

		quitMenu.enabled = true;
		startText.enabled = false;
		settingsText.enabled = false;
		controlText.enabled = false;
		quitText.enabled = false;
		controls.enabled = false;
	}

	public void NoPress(){

		quitMenu.enabled = false;
		startText.enabled = true;
		settingsText.enabled = true;
		controlText.enabled = true;
		quitText.enabled = true;
		controls.enabled = false;
	}

	public void controlsPress(){

		quitMenu.enabled = false;
		controls.enabled = true;
		settingsText.enabled = false;
		controlText.enabled = false;
		quitText.enabled = false;
	}

	public void StartLevel(){
	
		SceneManager.LoadScene ("LoadingScreen");
//		SceneManager.LoadScene ("forest");
	}

	public void ExitGame(){

		Application.Quit ();

	}
}
