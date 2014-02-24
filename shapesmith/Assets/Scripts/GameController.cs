using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public bool debugMode;
	public float tetrominoFallDelay;
	public PlacementManager placementManager;
	public int tetrominosLeft;
	public int tetrominoTimeLimit;
	public int finalTimeLimit;
	public GUIText guiText;

	public int tetrominoTimeLeft;
	public int finalTimeLeft;

	void Start(){
		tetrominoTimeLeft = tetrominoTimeLimit;
		finalTimeLeft = finalTimeLimit;
		if(!debugMode){
			InvokeRepeating("tetrominoTimerCountdown", 1.0f, 1.0f);
		}
	}

	void Update(){
		if (Input.GetMouseButtonDown (0)){
			if(tetrominosLeft > 0){
				placeTetromino(false);
			}
		}

		if (Input.GetKeyDown ("tab") && debugMode) {
			placementManager.switchTetromino();
		}
		
		if (Input.GetKeyDown ("1")) {
			placementManager.shapesArray[placementManager.currentShape].gameObject.transform.parent.transform.Rotate(0, 90, 0);
		}
		
		if (Input.GetKeyDown ("2")) {
			placementManager.shapesArray[placementManager.currentShape].gameObject.transform.parent.transform.Rotate(90, 0, 0);
		}
		
		if (Input.GetKeyDown ("3")) {
			placementManager.shapesArray[placementManager.currentShape].gameObject.transform.parent.transform.Rotate(0, 0, 90);
		}
	}

	private void placeTetromino(bool forced){
		if (forced) {
			placementManager.placeTetromino();
		}
		else{
			placementManager.placeTetromino();
			if(debugMode)
				return;
		}
		guiText.text = tetrominoTimeLeft.ToString ();
		tetrominoTimeLeft = tetrominoTimeLimit;
		tetrominosLeft--;
		if (tetrominosLeft == 0) {
			CancelInvoke("tetrominoTimerCountdown");
			InvokeRepeating("finalCountdown", 1.0f, 1.0f);
		}
		placementManager.randomizeTetromino ();
	}

	private void tetrominoTimerCountdown(){
		if (tetrominoTimeLeft == 0) {
			placeTetromino(true);
			tetrominoTimeLeft = tetrominoTimeLimit;
		}
		else{
			tetrominoTimeLeft--;
		}
		guiText.text = tetrominoTimeLeft.ToString (); 
	}

	private void finalCountdown(){
		if (finalTimeLeft == 0) {
			CancelInvoke("finalCountdown");
			guiText.text = "Level Failed";
			Debug.Log ("Level Failed");
			return;
		}
		else{
			finalTimeLeft--;
		}
		guiText.text = finalTimeLeft.ToString ();
	}

	public void levelCompleted(){
		CancelInvoke("finalCountdown");
		CancelInvoke("tetrominoTimerCountdown");
		guiText.text = "Level Completed";
	}

	public bool checkObjectProximity(Vector3 pos){
		var hitColliders = Physics.OverlapSphere(pos, .4f);
		if (hitColliders.Length > 0) {
			return false;
		}
		return true;
	}

	public bool checkObjectProximity(Vector3 pos, Collider targetCol){
		var hitColliders = Physics.OverlapSphere(pos, .4f);
		for(int i=0; i<hitColliders.Length; i++){
			Debug.Log(hitColliders[i].tag);
			if(hitColliders[i].tag == "Player")
				return false;
		}
		return true;
	}
}
