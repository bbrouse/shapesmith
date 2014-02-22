using UnityEngine;
using System.Collections;

public class PauseHandler : MonoBehaviour {
	public HideAndLockCursor cursorController;

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
			GUILayout.Label("Game is paused!");
			if(GUILayout.Button("Click me to unpause")){
				cursorController.toggleCursorLock();
				paused = !paused;
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
