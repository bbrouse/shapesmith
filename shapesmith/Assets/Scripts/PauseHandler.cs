using UnityEngine;
using System.Collections;

public class PauseHandler : MonoBehaviour {
	public HideAndLockCursor cursorController;
	public GameController gameController;
	public bool paused = false;
	
	void Update()
	{
		if (Input.GetMouseButtonDown (0) && !paused)
			Screen.lockCursor = true;
	}
	
	public void togglePause()
	{	
		Debug.Log("Toggle pause");
		if(Time.timeScale == 0f)
		{
			Time.timeScale = 1f;
			paused = false;
		}
		else
		{
			Time.timeScale = 0f;   
			paused = true;
		}
		cursorController.toggleCursorLock ();
	}
}
