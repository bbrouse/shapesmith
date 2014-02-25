using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RowHandler : MonoBehaviour {

	public float maxRowCheckRange = 100.0f;
	public LayerMask environmentMask;
	public LayerMask tetrominoMask;

	//Called whenever a tetromino stops moving to see if it completes a row
	public void checkCompletesRow(GameObject[] childCubes){
		List<RaycastHit> hitTetrominos = new List<RaycastHit>();
		List<GameObject> rowCompletingChildren = new List<GameObject>();
		List<GameObject> tetrominosToSplit = new List<GameObject>();
		bool enableGravity = false;

		//each child cube of a tetromino checks to see if it completes a row
		for (int i = 0; i < childCubes.Length; i++) {
			GameObject origin = childCubes[i];
			if(origin != null){
				int previousCount = hitTetrominos.Count;
				hitTetrominos.AddRange(getCompleteRow(origin.transform.position, new Vector3(1,0,0)));
				hitTetrominos.AddRange(getCompleteRow(origin.transform.position, new Vector3(0,1,0)));
				hitTetrominos.AddRange(getCompleteRow(origin.transform.position, new Vector3(0,0,1)));
				if(hitTetrominos.Count > previousCount){
					rowCompletingChildren.Add(origin); //if child completes row, we want to destory that too
					enableGravity = true;
				}
			}
		}

		//now we destroy all the cubes that complete a row in x, y, or z direction at one time
		if (hitTetrominos.Count != 0) {
			for (int i = 0; i < rowCompletingChildren.Count; i++) {
				DestroyImmediate(rowCompletingChildren[i]);
			}

			for (int i = 0; i < hitTetrominos.Count; i++) {
				if(hitTetrominos[i].collider != null){
					//before we destroy the cube, let the parent know that it should fall if it needs to
					GameObject tetromino = hitTetrominos[i].collider.gameObject.transform.parent.gameObject;
					DestroyImmediate(hitTetrominos[i].collider.gameObject);
					tetrominosToSplit.Add(tetromino);
				}
			}
		}

		if (enableGravity) {
			GetComponent<GravityHandler>().enableGravityCheck();
		}
		if (rowCompletingChildren.Count > 0) {
			GetComponent<SplitHandler>().checkandHandleSplit(childCubes);
		}
		if (tetrominosToSplit.Count > 0) {
			for(int i = 0; i < tetrominosToSplit.Count; i++){
				GameObject tetromino = tetrominosToSplit[i];
				GravityHandler hitTetroGravityHandler = tetromino.GetComponent<GravityHandler>();
				hitTetroGravityHandler.enableGravityCheck();
				tetromino.GetComponent<SplitHandler>().checkandHandleSplit(hitTetroGravityHandler.childCubes);
			}
		}
	}

	//logic for checking a row in a given direction from one cube
	private List<RaycastHit> getCompleteRow(Vector3 origin, Vector3 direction){
		//rowFull represents the positive and negative direction (i.e positive x and negative x from the cube)
		bool[] rowFull = new bool[2];
		List<RaycastHit> hitTetrominos = new List<RaycastHit>();

		//for positive and negative direction from the cube...
		for (int i=0; i < 2; i++) {
			rowFull[i] = false;
			Ray exploringRay = new Ray (origin, direction);
			RaycastHit hitInfo;

			//exploringRay sees if there's any environment boundaries within a given number of units
			float distanceToEnvironment = 0;
			if (Physics.Raycast (exploringRay, out hitInfo, maxRowCheckRange, environmentMask)) {
				distanceToEnvironment = Vector3.Distance(hitInfo.point, origin);
			}
			else{
				//if no boundary to hold the row, then return empty list
				return new List<RaycastHit>();
			}

			if(distanceToEnvironment < 1){
				//if directly against the environment boundary, then this direction is full
				rowFull[i] = true;
			}
			else{
				//see if the space between origin and environment boundary is full of cubes
				RaycastHit[] hits = Physics.RaycastAll(origin, direction, distanceToEnvironment, tetrominoMask);
				if(hits.Length == (int) distanceToEnvironment){
					hitTetrominos.AddRange(hits);
					rowFull[i] = true;
				}
			}

			direction = direction * -1;
		}

		//if both directions full, then return all the tetrominos
		if (rowFull [0] && rowFull [1]) {
			return hitTetrominos;
		}
		else{
			return new List<RaycastHit>();
		}
	}
}
