using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RowHandler : MonoBehaviour {

	public float maxRowCheckRange = 100.0f;
	public LayerMask environmentMask;
	public LayerMask tetrominoMask;

	public void checkCompletesRow(GameObject[] childCubes){
		List<RaycastHit> hitTetrominos = new List<RaycastHit>();
		List<GameObject> rowCompletingChildren = new List<GameObject>();

		for (int i = 0; i < childCubes.Length; i++) {
			GameObject origin = childCubes[i];
			if(origin != null){
				int previousCount = hitTetrominos.Count;
				hitTetrominos.AddRange(getCompleteRow(origin.transform.position, new Vector3(1,0,0)));
				hitTetrominos.AddRange(getCompleteRow(origin.transform.position, new Vector3(0,1,0)));
				hitTetrominos.AddRange(getCompleteRow(origin.transform.position, new Vector3(0,0,1)));
				if(hitTetrominos.Count > previousCount){
					rowCompletingChildren.Add(origin);
				}
			}
		}

		if (hitTetrominos.Count != 0) {
			for (int i = 0; i < rowCompletingChildren.Count; i++) {
				GameObject tetromino = rowCompletingChildren[i].transform.parent.gameObject;
				tetromino.GetComponent<GravityHandler>().enableGravityCheck();
				Destroy(rowCompletingChildren[i]);
			}

			for (int i = 0; i < hitTetrominos.Count; i++) {
				GameObject tetromino = hitTetrominos[i].collider.gameObject.transform.parent.gameObject;
				tetromino.GetComponent<GravityHandler>().enableGravityCheck();
				Destroy(hitTetrominos[i].collider.gameObject);
			}
		}
	}

	private List<RaycastHit> getCompleteRow(Vector3 origin, Vector3 direction){
		bool[] rowFull = new bool[2];
		List<RaycastHit> hitTetrominos = new List<RaycastHit>();

		for (int i=0; i < 2; i++) {
			rowFull[i] = false;
			Ray exploringRay = new Ray (origin, direction);
			RaycastHit hitInfo;
			
			float distanceToEnvironment = 0;
			if (Physics.Raycast (exploringRay, out hitInfo, maxRowCheckRange, environmentMask)) {
				distanceToEnvironment = Vector3.Distance(hitInfo.point, origin);
				Debug.Log("Distance to environment: " + (int)distanceToEnvironment);
			}
			else{
				return new List<RaycastHit>();
			}

			if(distanceToEnvironment < 1){
				rowFull[i] = true;
			}
			else{
				RaycastHit[] hits = Physics.RaycastAll(origin, direction, distanceToEnvironment, tetrominoMask);
				Debug.Log("Number of tetrominos hit: " + hits.Length);
				if(hits.Length == (int) distanceToEnvironment){
					hitTetrominos.AddRange(hits);
					rowFull[i] = true;
				}
			}

			direction = direction * -1;
		}
		if (rowFull [0] && rowFull [1]) {
			return hitTetrominos;
		}
		else{
			return new List<RaycastHit>();
		}
	}
}
