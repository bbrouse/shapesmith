using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public bool tutorialMode;
	public float tetrominoFallDelay;
	public PlacementManager placementManager;
	public int tetrominosLeft;
	public int tetrominoTimeLimit;
	public int finalTimeLimit;
	public GUIText alertText;
	public GUIText tetrominosLeftText;
	public GUIText timerText;

	private int tetrominoTimeLeft;
	private int finalTimeLeft;
	private bool isLevelCompleted = false;
	private bool timerContinue = true;

	void Start(){
		tetrominoTimeLeft = tetrominoTimeLimit;
		finalTimeLeft = finalTimeLimit;
		tetrominosLeftText.text = "Tetrominos: " + tetrominosLeft;
		if(!tutorialMode){
			InvokeRepeating("tetrominoTimerCountdown", 2.0f, 1.0f);
		}
	}

	void Update(){
		if (timerContinue) {
			if(tetrominosLeft == 0){
				placementManager.resetTetrominoFull();
				placementManager.enabled = false;
			}
			
			if (Input.GetMouseButtonDown (0)){
				if(tetrominosLeft > 0){
					placeTetromino(false);
				}
			}
			
			if (Input.GetKeyDown (KeyCode.LeftShift)) {
				placementManager.shapesArray[placementManager.currentShape].gameObject.transform.parent.transform.Rotate(0, 0, -90);
			}
			
			if(Input.GetAxis("Mouse ScrollWheel") > 0){
				placementManager.shapesArray[placementManager.currentShape].gameObject.transform.parent.transform.Rotate(0, -90, 0);
			}
			
			if(Input.GetAxis("Mouse ScrollWheel") < 0){
				placementManager.shapesArray[placementManager.currentShape].gameObject.transform.parent.transform.Rotate(0, 90, 0);
			}
			
			if (Input.GetKeyDown (KeyCode.Z)) {
				GetComponent<CameraZoom>().toggleZoom();
			}
		}

		if (Input.GetKeyDown (KeyCode.Tab)) {
			GetComponent<InGameMenu>().toggleMenu();
		}
	}

	private void placeTetromino(bool forced){
		if (forced) {
			placementManager.placeTetromino();
		}
		else{
			placementManager.placeTetromino();
			if(tutorialMode)
				return;
		}
		timerText.text = (tetrominoTimeLeft).ToString ();
		tetrominoTimeLeft = tetrominoTimeLimit;
		tetrominosLeft--;
		tetrominosLeftText.text = "Tetrominos: " + tetrominosLeft;
		if (tetrominosLeft == 0) {
			CancelInvoke("tetrominoTimerCountdown");
			InvokeRepeating("finalCountdown", 0.0f, 1.0f);
		}
		placementManager.randomizeTetromino ();
	}

	private void tetrominoTimerCountdown(){
		if (timerContinue) {
			if (tetrominoTimeLeft == 0) {
				placeTetromino(true);
				tetrominoTimeLeft = tetrominoTimeLimit;
			}
			else{
				tetrominoTimeLeft--;
			}
			timerText.text = tetrominoTimeLeft.ToString (); 
		}
	}

	private void finalCountdown(){
		if (timerContinue) {
			if (finalTimeLeft == 0) {
				timerText.text = (finalTimeLeft).ToString ();
				levelFailed();
				return;
			}
			else{
				finalTimeLeft--;
			}
			timerText.color = Color.red;
			timerText.text = (finalTimeLeft).ToString ();
		}
	}

	public void levelCompleted(){
		CancelInvoke("finalCountdown");
		CancelInvoke("tetrominoTimerCountdown");
		alertText.color = Color.green;
		alertText.text = "Level Completed";
		Invoke ("loadMainMenu", 2.0f);
	}

	public void levelFailed(){
		CancelInvoke("finalCountdown");
		CancelInvoke("tetrominoTimerCountdown");
		alertText.color = Color.red;
		alertText.text = "Level Failed";
		Invoke ("loadMainMenu", 2.0f);
	}

	public bool checkObjectProximity(Vector3 pos){
		var hitColliders = Physics.OverlapSphere(pos, .4f);
		if (hitColliders.Length > 0) {
			return true;
		}
		return false;
	}

	public bool checkObjTargetProx(Vector3 pos, Collider targetCol){
		var hitColliders = Physics.OverlapSphere(pos, .4f);
		for(int i=0; i<hitColliders.Length; i++){
			if(hitColliders[i] == targetCol)
				return true;
		}
		return false;
	}

	public bool checkObjNonTargetProx(Vector3 pos, Collider targetCol){
		var hitColliders = Physics.OverlapSphere(pos, .4f);
		for(int i=0; i<hitColliders.Length; i++){
			if(hitColliders[i] != targetCol)
				return true;
		}
		return false;
	}

	public bool checkObjNonTargetProx(GameObject colliding, Collider targetCol, ref List<GameObject> actualHit){
		var hitColliders = Physics.OverlapSphere(colliding.transform.position, .4f);
		for(int i=0; i<hitColliders.Length; i++){
			if(hitColliders[i] != targetCol){
				actualHit.Add(colliding);
			}
		}
		if(actualHit.Count > 0) return true;
		return false;
	}

	public void loadMainMenu(){
		Application.LoadLevel("Level_Select");
	}

	public void toggleTimer(){
		timerContinue = !timerContinue;
	}
}
