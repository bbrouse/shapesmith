using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public float tetrominoFallDelay;
	public PlacementManager placementManager;

	void Update(){
		if (Input.GetKeyDown ("tab")) {
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

	public bool checkObjectProximity(Vector3 pos){
		var hitColliders = Physics.OverlapSphere(pos, .4f);
		if (hitColliders.Length > 0) {
			return false;
		}
		return true;
	}
}
