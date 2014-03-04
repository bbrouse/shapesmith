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

	void Update(){
		/*
		if (Input.GetKeyDown ("p") && gameController.debugMode) {
			toggleMenu();
		}
		*/
	}

	/*
	void OnGUI(){
		if (pauseHandler.paused) {
			//show menu items
		}

		if (gameController.debugMode && pauseHandler.paused) {
			GUILayout.Label("Paused!");
			
			if(gameController.debugMode){
				GUILayout.Label("Debug Mode: On");
			}else{
				GUILayout.Label("Debug Mode: Off");
			}
			
			if(GUILayout.Button("Resume")){
				toggleMenu();
			}
			
			if(GUILayout.Button("Toggle Debug Mode")){
				gameController.debugMode = !gameController.debugMode;
			}
			
			if(GUILayout.Button("Restart Level")){
				restartLevel();
			}
		}
	}*/

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
		Application.ExternalEval("window.open('','_self').close()");
	}
}
