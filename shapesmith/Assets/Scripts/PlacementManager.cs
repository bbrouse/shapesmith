﻿using UnityEngine;
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

	private Collider actualCollider = new Collider();
	private bool allowPlacement = false;
	private Vector3[] startPos = new Vector3[5];
	private Quaternion[] startRot = new Quaternion[5];
	private bool randomizing = false;
	private Material wireFrameMat;
	private Vector3 translateDirection;

	void Start(){
		for (int i=0; i<startPos.Length; i++) {
			startPos[i] = shapesArray[i].gameObject.transform.position;
			startRot[i] = shapesArray[i].gameObject.transform.rotation;
		}
		wireFrameMat = (Material)Resources.Load ("outlineMaterial", typeof(Material));
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

		allowPlacement = true;
		Ray placementRay = new Ray (origin.transform.position, origin.transform.forward);
		RaycastHit hitInfo;
		
		Debug.DrawRay (origin.transform.position, origin.transform.forward * maxDistance);
		Physics.Raycast (placementRay, out hitInfo, maxDistance);
		if (Physics.Raycast (placementRay, out hitInfo, maxDistance, environmentMask | tetrominoMask)) {
			Vector3 position = hitInfo.point;

			position.x = getCoordinateFromHit(position.x, hitInfo.collider.transform.position.x);
			position.y = getCoordinateFromHit(position.y, hitInfo.collider.transform.position.y);
			position.z = getCoordinateFromHit(position.z, hitInfo.collider.transform.position.z);

			if((hitInfo.collider.GetType() == typeof(MeshCollider) || hitInfo.collider.GetType() == typeof(BoxCollider)) && isCornerHit(position, hitInfo.point)){
				//We don't want to place the placeholder object because we are looking at a cube's corner
				//We also want to reset the placeholder object if it gets 'stuck' in the last placed object
			}else if(!allowPlacement){
				//We don't want to allow placing objects over top of the player or cubes
			}else{
				shapesArray[currentShape].transform.parent.gameObject.transform.position = position;

				checkPlacementAllowed();

				bool checkNext = false;

				for(int i=0; i<4; i++){
					if(gameController.checkObjTargetProx(shapesArray[currentShape].gameObject.transform.parent.GetChild(i).position, hitInfo.collider)){
						shapesArray[currentShape].transform.parent.gameObject.transform.Translate(hitInfo.normal, Space.World);
						i = -1;
					}
				}

				for(int i=0; i<4; i++){
					if(gameController.checkObjNonTargetProx(shapesArray[currentShape].gameObject.transform.parent.GetChild(i).position, hitInfo.collider, ref actualCollider)){
						translateDirection = actualCollider.transform.position - shapesArray[currentShape].gameObject.transform.position;
						checkNext = true;
						break;
					}
				}

				translateDirection.x = (float) System.Math.Round((translateDirection.x * 2), System.MidpointRounding.AwayFromZero) / 2;
				translateDirection.y = (float) System.Math.Round((translateDirection.y * 2), System.MidpointRounding.AwayFromZero) / 2;
				translateDirection.z = (float) System.Math.Round((translateDirection.z * 2), System.MidpointRounding.AwayFromZero) / 2;

				if(translateDirection.x % 1 == 0){
					if(shapesArray[currentShape].gameObject.transform.position.x < actualCollider.transform.position.x){
						translateDirection = new Vector3(-1.0f, 0.0f, 0.0f);
					}else{
						translateDirection = new Vector3(1.0f, 0.0f, 0.0f);
					}
				}else if(translateDirection.y % 1 == 0){
					if(shapesArray[currentShape].gameObject.transform.position.y < actualCollider.transform.position.y){
						translateDirection = new Vector3(0.0f, -1.0f, 0.0f);
					}else{
						translateDirection = new Vector3(0.0f, 1.0f, 0.0f);
					}
				}else if(translateDirection.z % 1 == 0){
					if(shapesArray[currentShape].gameObject.transform.position.z < actualCollider.transform.position.z){
						translateDirection = new Vector3(0.0f, 0.0f, -1.0f);
					}else{
						translateDirection = new Vector3(0.0f, 0.0f, 1.0f);
					}
				}

				if(checkNext){
					for(int j=0; j<3; j++){
						shapesArray[currentShape].transform.parent.gameObject.transform.Translate(translateDirection * (j+1), Space.World);
						for(int i=0; i<4; i++){
							if(gameController.checkObjNonTargetProx(shapesArray[currentShape].gameObject.transform.parent.GetChild(i).position, hitInfo.collider)){
								shapesArray[currentShape].transform.parent.gameObject.transform.Translate(translateDirection * -(j+1), Space.World);
								break;
							}

							if(i==3) checkNext = false;
						}
						if(!checkNext) break;
					}
				}

				checkPlacementAllowed();
			}

			checkPlacementAllowed();
		}else{
			allowPlacement = false;
			resetTetromino ();
		}
	}

	public bool placeTetromino(){
		checkPlacementAllowed();
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

			resetTetrominoFull ();
			currentShape = rand;
		}
	}

	private void resetTetromino(){
		shapesArray [currentShape].transform.parent.gameObject.transform.position = startPos [currentShape];
		//It was decided that resetting rotation probably isn't a good idea, unless a block is being placed.
		//shapesArray [currentShape].transform.parent.gameObject.transform.rotation = startRot [currentShap
	}

	private void resetTetrominoFull(){
		shapesArray [currentShape].transform.parent.gameObject.transform.position = startPos [currentShape];
		shapesArray [currentShape].transform.parent.gameObject.transform.rotation = startRot [currentShape];
	}

	public void checkPlacementAllowed(){
		for(int i=0; i<shapesArray[currentShape].gameObject.transform.parent.childCount; i++){
			allowPlacement = !gameController.checkObjectProximity(shapesArray[currentShape].gameObject.transform.parent.GetChild(i).position);
			if(!allowPlacement){
				wireFrameMat.color = Color.red;
				return;
			}
		}

		wireFrameMat.color = Color.black;
	}
}
