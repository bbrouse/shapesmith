using UnityEngine;
using System;
using System.Collections;

public class InGameMenu : MonoBehaviour {

	public BlurEffect mainCameraBlur;
	public GameObject guiBackground;
	public PauseHandler pauseHandler;
	public GameController gameController;

	public GameObject restartText;
	public GameObject levelSelectText;
	public GameObject resumeText;
	public GameObject quitText;

	void Start () {
		mainCameraBlur = Camera.main.GetComponent<BlurEffect> ();

		restartText.GetComponent<ClickableMenuItem> ().setMethodToRun (restartLevel);
		levelSelectText.GetComponent<ClickableMenuItem> ().setMethodToRun (loadLevelSelect);
		resumeText.GetComponent<ClickableMenuItem> ().setMethodToRun (resumeLevel);
		quitText.GetComponent<ClickableMenuItem> ().setMethodToRun (quitGame);
	}

	public void toggleMenu(){
		gameController.GetComponent<GameController> ().toggleTimer ();
		pauseHandler.togglePause ();
		mainCameraBlur.enabled = !mainCameraBlur.enabled;
		guiBackground.guiTexture.enabled = !guiBackground.guiTexture.enabled;
		restartText.guiText.enabled = !restartText.guiText.enabled;
		levelSelectText.guiText.enabled = !levelSelectText.guiText.enabled;
		resumeText.guiText.enabled = !resumeText.guiText.enabled;
		quitText.guiText.enabled = !quitText.guiText.enabled;
		//show/hide menu
	}

	public void resumeLevel(){
		toggleMenu ();
	}

	public void restartLevel(){
		toggleMenu ();
		Application.LoadLevel (Application.loadedLevel);
	}

	public void loadLevelSelect(){
		toggleMenu ();
		gameController.GetComponent<GameController> ().loadMainMenu ();
	}

	public void quitGame(){
		pauseHandler.togglePause ();
		Application.LoadLevel ("Main_Menu");
		Application.ExternalEval("window.open('','_self').close()");
	}
}
