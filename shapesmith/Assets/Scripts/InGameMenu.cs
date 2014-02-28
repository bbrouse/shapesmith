using UnityEngine;
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
	}

	void Update(){
		if (Input.GetKeyDown ("p") && gameController.debugMode) {
			toggleMenu();
		}

	}

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
				Application.LoadLevel(Application.loadedLevel);
			}
		}
	}

	public void toggleMenu(){
		pauseHandler.togglePause ();
		mainCameraBlur.enabled = !mainCameraBlur.enabled;
		guiBackground.guiTexture.enabled = !guiBackground.guiTexture.enabled;
		restartText.guiText.enabled = !restartText.guiText.enabled;
		levelSelectText.guiText.enabled = !levelSelectText.guiText.enabled;
		resumeText.guiText.enabled = !resumeText.guiText.enabled;
		quitText.guiText.enabled = !quitText.guiText.enabled;
		//show/hide menu
	}
}
