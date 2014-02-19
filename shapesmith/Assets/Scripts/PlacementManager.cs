using UnityEngine;
using System.Collections;

public class PlacementManager : MonoBehaviour {
	
	public GameObject origin;
	public float maxDistance;
	public LayerMask layerMask;

	public GameController gameController;
	public GameObject[] shapesArray = new GameObject[5];
	public GameObject[] tetrominoArray = new GameObject[5];

	private int currentShape = 0;
	private bool allowPlacement = true;
	private Vector3[] startPos = new Vector3[5];

	void Start(){
		for (int i=0; i<startPos.Length; i++) {
			startPos[i] = shapesArray[i].gameObject.transform.position;
		}
	}

	void Update () {
		if (Input.GetKeyDown ("tab")) {
			switchTetromino();
		}

		if (Input.GetKeyDown ("1")) {
			shapesArray[currentShape].gameObject.transform.parent.transform.Rotate(0, 90, 0);
		}

		if (Input.GetKeyDown ("2")) {
			shapesArray[currentShape].gameObject.transform.parent.transform.Rotate(90, 0, 0);
		}

		if (Input.GetKeyDown ("3")) {
			shapesArray[currentShape].gameObject.transform.parent.transform.Rotate(0, 0, 90);
		}

		allowPlacement = true;
		Ray placementRay = new Ray (origin.transform.position, origin.transform.forward);
		RaycastHit hitInfo;
		
		Debug.DrawRay (origin.transform.position, origin.transform.forward * maxDistance);
		Physics.Raycast (placementRay, out hitInfo, maxDistance);
		if (Physics.Raycast (placementRay, out hitInfo, maxDistance, layerMask)) {
			Vector3 position = hitInfo.point;

			position.x = getCoordinateFromHit(position.x, hitInfo.collider.transform.position.x);
			position.y = getCoordinateFromHit(position.y, hitInfo.collider.transform.position.y);
			position.z = getCoordinateFromHit(position.z, hitInfo.collider.transform.position.z);

			if(hitInfo.collider.GetType() == typeof(BoxCollider) && isCornerHit(position, hitInfo.collider.transform.position)){
				//We dont' want to place the placeholder object because we are looking at a cube's corner
			}else if(!allowPlacement){
				//We don't want to allow placing objects over top of the player or other cubes
			}else{
				shapesArray[currentShape].transform.parent.gameObject.transform.position = position;

				allowPlacement = false;

				while(!allowPlacement){
					for(int i=0; i<4; i++){
						while(!gameController.checkObjectProximity(shapesArray[currentShape].gameObject.transform.parent.GetChild(i).position) || Mathf.Round(shapesArray[currentShape].gameObject.transform.parent.GetChild(i).position.y) < 0){
							shapesArray[currentShape].transform.parent.gameObject.transform.Translate(Vector3.up * 1, Space.World);
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

			for(int i=0; i<shapesArray[currentShape].gameObject.transform.parent.childCount; i++){
				allowPlacement = gameController.checkObjectProximity(shapesArray[currentShape].gameObject.transform.parent.GetChild(i).position);
				if(!allowPlacement) break;
			}
		}

		if (Input.GetMouseButtonDown (0) && allowPlacement == true) {
			Instantiate(tetrominoArray[currentShape], shapesArray[currentShape].transform.parent.gameObject.transform.position, shapesArray[currentShape].transform.parent.gameObject.transform.rotation);
		}
	}
	
	private float getCoordinateFromHit(float positionCoordinate, float hitCoordinate){

		if(hitCoordinate % 2 == 0){
			if(positionCoordinate > hitCoordinate){
				positionCoordinate = positionCoordinate + 0.1f;
			}else{
				positionCoordinate = positionCoordinate - 0.1f;
			}
		}
		return Mathf.Round(positionCoordinate);
	}

	private bool isCornerHit(Vector3 placementPosition, Vector3 hitObjectPosition){
		float x = placementPosition.x;
		float y = placementPosition.y;
		float z = placementPosition.z;
		float newX = Mathf.Round(hitObjectPosition.x);
		float newY = Mathf.Round(hitObjectPosition.y);
		float newZ = Mathf.Round(hitObjectPosition.z);
		return ((x != newX && y != newY) || (y != newY && z != newZ) || (x != newX && z != newZ));
	}

	private void switchTetromino(){
		shapesArray[currentShape].gameObject.transform.parent.transform.position = startPos[currentShape];
		if (currentShape < 4) {
			currentShape++;
		} else {
			currentShape = 0;
		}
	}
}
