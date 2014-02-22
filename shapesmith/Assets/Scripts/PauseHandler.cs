﻿using UnityEngine;
using System.Collections;

public class PauseHandler : MonoBehaviour {
	public HideAndLockCursor cursorController;
	public GameController gameController;

	private bool paused = false;
	
	void Update()
	{
		if (Input.GetKeyDown ("p")) {
			cursorController.toggleCursorLock ();
			paused = !paused;
		}
	}
	
	void OnGUI()
	{
		if(paused)
		{
			GUILayout.Label("Paused!");
			if(GUILayout.Button("Resume")){
				cursorController.toggleCursorLock();
				paused = !paused;
			}

			if(GUILayout.Button("Toggle Debug Mode")){
				gameController.debugMode = !gameController.debugMode;
				Debug.Log (gameController.debugMode);
			}
		}
	}
	
	bool togglePause()
	{
		if(Time.timeScale == 0f)
		{
			Time.timeScale = 1f;
			return(false);
		}
		else
		{
			Time.timeScale = 0f;
			return(true);    
		}
	}
}
