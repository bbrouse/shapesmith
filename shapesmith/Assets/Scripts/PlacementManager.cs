using UnityEngine;
using System.Collections;

public class PlacementManager : MonoBehaviour {
	
	public GameObject origin;
	public GameObject player;
	public float maxDistance;
	public LayerMask environmentMask;
	public LayerMask tetrominoMask;
	public GameController gameController;
	public PauseHandler pauseHandler;
	public GameObject[] shapesArray = new GameObject[5];
	public GameObject[] tetrominoArray = new GameObject[5];
	public int currentShape = 0;

	private bool allowPlacement = false;
	private Vector3[] startPos = new Vector3[5];
	private Quaternion[] startRot = new Quaternion[5];
	private bool randomizing = false;

	void Start(){
		for (int i=0; i<startPos.Length; i++) {
			startPos[i] = shapesArray[i].gameObject.transform.position;
			startRot[i] = shapesArray[i].gameObject.transform.rotation;
		}
	}

	void Update () {

		if (!gameController.debugMode && !randomizing) {
			randomizing = true;
			//InvokeRepeating ("randomizeTetromino", 7.5f, 7.5f);
		}

		if (gameController.debugMode) {
			randomizing = false;
			CancelInvoke("randomizeTetromino");
		}
		
		Ray placementRay = new Ray (origin.transform.position, origin.transform.forward);
		RaycastHit hitInfo;
		
		Debug.DrawRay (origin.transform.position, origin.transform.forward * maxDistance);
		Physics.Raycast (placementRay, out hitInfo, maxDistance);
		if (Physics.Raycast (placementRay, out hitInfo, maxDistance, environmentMask | tetrominoMask)) {
			Vector3 position = hitInfo.point;

			Debug.Log (hitInfo.normal);

			position.x = getCoordinateFromHit(position.x, hitInfo.collider.transform.position.x);
			position.y = getCoordinateFromHit(position.y, hitInfo.collider.transform.position.y);
			position.z = getCoordinateFromHit(position.z, hitInfo.collider.transform.position.z);

			checkPlacementAllowed();

			if((hitInfo.collider.GetType() == typeof(MeshCollider) || hitInfo.collider.GetType() == typeof(BoxCollider)) && isCornerHit(position, hitInfo.point)){
				//We don't want to place the placeholder object because we are looking at a cube's corner
				//We also want to reset the placeholder object if it gets 'stuck' in the last placed object
				resetTetromino ();
			}else if(!allowPlacement){
				//We don't want to allow placing objects over top of the player or cubes
			}else{
				position = position + (hitInfo.normal * .025f);
				shapesArray[currentShape].transform.parent.gameObject.transform.position = position;

				allowPlacement = false;

				while(!allowPlacement){
					for(int i=0; i<4; i++){
						while(!gameController.checkObjectProximity(shapesArray[currentShape].gameObject.transform.parent.GetChild(i).position)){
							shapesArray[currentShape].transform.parent.gameObject.transform.Translate(hitInfo.normal, Space.World);
						}
					}
					
					for(int i=0; i<4; i++){
						if(gameController.checkObjectProximity(shapesArray[currentShape].gameObject.transform.parent.GetChild(i).position)){
							if(i == 3) allowPlacement = true;
						}else{
							break;
						}
					}
				}
			}
		}else{
			allowPlacement = false;
			resetTetromino ();
		}
	}

	public bool placeTetromino(){
		if (allowPlacement) {
			Instantiate(tetrominoArray[currentShape], shapesArray[currentShape].transform.parent.gameObject.transform.position, shapesArray[currentShape].transform.parent.gameObject.transform.rotation);
			return true;
		}
		return false;
	}
	
	private float getCoordinateFromHit(float positionCoordinate, float hitCoordinate){
		if(hitCoordinate > 0){
			if((hitCoordinate - .5f) % 2 == 0 || hitCoordinate % 2 == 0){
				if(positionCoordinate > hitCoordinate){
					positionCoordinate = positionCoordinate + 0.1f;
				}else{
					positionCoordinate = positionCoordinate - 0.1f;
				}
			}
		}else{
			if((hitCoordinate + .5f) % 2 == 0 || hitCoordinate % 2 == 0){
				if(positionCoordinate > hitCoordinate){
					positionCoordinate = positionCoordinate + 0.1f;
				}else{
					positionCoordinate = positionCoordinate - 0.1f;
				}
			}
		}

		return Mathf.Round(positionCoordinate);
	}

	private bool isCornerHit(Vector3 placementPosition, Vector3 hitPosition){
		if(hitPosition.x - Mathf.Floor(hitPosition.x) >= .4 && hitPosition.x - Mathf.Floor(hitPosition.x) <= .6){
			hitPosition.x = Mathf.Round(hitPosition.x * 2) / 2;
		}
		if(hitPosition.y - Mathf.Floor(hitPosition.y) >= .4 && hitPosition.y - Mathf.Floor(hitPosition.y) <= .6){
			hitPosition.y = Mathf.Round(hitPosition.y * 2) / 2;
		}
		if(hitPosition.z - Mathf.Floor(hitPosition.z) >= .4 && hitPosition.z - Mathf.Floor(hitPosition.z) <= .6){
			hitPosition.z = Mathf.Round(hitPosition.z * 2) / 2;
		}

		int cornerCheck = 0;

		if((hitPosition.x - .5) % 1 == 0){
			cornerCheck++;
		}
		if((hitPosition.y - .5) % 1 == 0){
			cornerCheck++;
		}
		if((hitPosition.z - .5) % 1 == 0){
			cornerCheck++;
		}
 		return (cornerCheck > 1);
	}

	public void switchTetromino(){
		resetTetromino ();
		if (currentShape < 4) {
			currentShape++;
		} else {
			currentShape = 0;
		}
	}

	public void randomizeTetromino(){
		if(!pauseHandler.paused){
			int rand = Random.Range (0, 5);
			while (rand == currentShape) {
				rand = Random.Range (0, 5);
			}

			resetTetromino ();
			currentShape = rand;
		}
	}

	private void resetTetromino(){
		shapesArray [currentShape].transform.parent.gameObject.transform.position = startPos [currentShape];
		shapesArray [currentShape].transform.parent.gameObject.transform.rotation = startRot [currentShape];
	}

	public void checkPlacementAllowed(){
		for(int i=0; i<shapesArray[currentShape].gameObject.transform.parent.childCount; i++){
			allowPlacement = gameController.checkObjectProximity(shapesArray[currentShape].gameObject.transform.parent.GetChild(i).position, player.collider);
			if(!allowPlacement) break;
		}
	}
}
