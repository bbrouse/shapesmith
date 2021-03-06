﻿using UnityEngine;
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
	public Camera guiCamera;

	void Start () {
		mainCameraBlur = Camera.main.GetComponent<BlurEffect> ();

		restartText.GetComponent<ClickableMenuItem> ().setMethodToRun (restartLevel);
		levelSelectText.GetComponent<ClickableMenuItem> ().setMethodToRun (loadLevelSelect);
		resumeText.GetComponent<ClickableMenuItem> ().setMethodToRun (resumeLevel);
		quitText.GetComponent<ClickableMenuItem> ().setMethodToRun (quitGame);
		guiCamera = GameObject.Find ("3D Gui Camera").camera;
	}

	public void toggleMenu(){
		guiCamera.depth = -guiCamera.depth;
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
		Screen.lockCursor = false;
		PlayerPrefs.DeleteAll ();
		Application.LoadLevel ("Main_Menu");
	}
}
