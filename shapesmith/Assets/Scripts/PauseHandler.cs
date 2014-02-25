using UnityEngine;
using System.Collections;

public class PauseHandler : MonoBehaviour {
	public HideAndLockCursor cursorController;
	public GameController gameController;
	public bool paused = false;
	
	void Update()
	{
		if (Input.GetKeyDown ("p")) {
			cursorController.toggleCursorLock ();
			paused = !paused;
		}
		
		if (Input.GetMouseButtonDown (0) && !paused)
			Screen.lockCursor = true;
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
				Application.LoadLevel(Application.loadedLevel);
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
