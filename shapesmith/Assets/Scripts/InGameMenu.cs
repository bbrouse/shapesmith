using UnityEngine;
using System.Collections;

public class InGameMenu : MonoBehaviour {

	public BlurEffect mainCameraBlur;
	public GameObject guiBackground;
	private bool paused = false;

	void Start () {
		mainCameraBlur = Camera.main.GetComponent<BlurEffect> ();
	}

	void Update(){
		if (Input.GetMouseButtonDown (0) && !paused) {
			Screen.lockCursor = true;		
		}
	}

	void OnGUI(){
		if (paused) {
			if(GUI.Button(new Rect(10, 10, 100, 60), "Quit Game")){
				Application.Quit();
			}
		}
	}

	public void toggleMenu(){
		paused = !paused;
		Time.timeScale = (Time.timeScale == 1f) ? 0f : 1f;
		mainCameraBlur.enabled = !mainCameraBlur.enabled;
		guiBackground.guiTexture.enabled = !guiBackground.guiTexture.enabled;
		//show/hide menu
	}
}
