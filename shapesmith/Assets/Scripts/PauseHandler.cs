using UnityEngine;
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

			if(gameController.debugMode){
				GUILayout.Label("Debug Mode: On");
			}else{
				GUILayout.Label("Debug Mode: Off");
			}

			if(GUILayout.Button("Resume")){
				cursorController.toggleCursorLock();
				paused = !paused;
			}

			if(GUILayout.Button("Toggle Debug Mode")){
				gameController.debugMode = !gameController.debugMode;
			}

			if(GUILayout.Button("Restart Level")){
				Application.LoadLevel("main");
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
