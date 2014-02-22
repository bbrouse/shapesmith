using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RowHandler : MonoBehaviour {

	public float maxRowCheckRange = 100.0f;
	public LayerMask environmentMask;
	public LayerMask tetrominoMask;

	public void checkCompletesRow(GameObject[] childCubes){
		for (int i = 0; i < childCubes.Length; i++) {
			//all child cubes check if there's anything directly under them
			GameObject origin = childCubes[i];
			handleRow(origin.transform.position, new Vector3(1,0,0));
			handleRow(origin.transform.position, new Vector3(0,1,0));
			handleRow(origin.transform.position, new Vector3(0,0,1));
		}
	}

	private void handleRow(Vector3 origin, Vector3 direction){
		bool[] rowFull = new bool[2];
		List<RaycastHit> hitTetrominos = new List<RaycastHit>();

		for (int i=0; i < 2; i++) {
			rowFull[i] = false;
			Ray exploringRay = new Ray (origin, direction);
			RaycastHit hitInfo;
			
			float distanceToEnvironment = 0;
			if (Physics.Raycast (exploringRay, out hitInfo, maxRowCheckRange, environmentMask)) {
				distanceToEnvironment = Vector3.Distance(hitInfo.transform.position, origin);
				Debug.Log("Distance to environment: " + (int)distanceToEnvironment);
			}
			else{
				break;
			}

			if(distanceToEnvironment == 1){
				rowFull[i] = true;
			}
			else{
				RaycastHit[] hits = Physics.RaycastAll(origin, direction, distanceToEnvironment, tetrominoMask);
				Debug.Log("Number of tetrominos hit: " + hits.Length);
				if(hits.Length == (int) distanceToEnvironment-1){
					hitTetrominos.AddRange(hits);
					rowFull[i] = true;
				}
			}

			direction = direction * -1;
		}
		Debug.Log (rowFull [0] + ", " + rowFull [1]);
		if (rowFull [0] && rowFull [1]) {
			for(int i = 0; i < hitTetrominos.Count; i++){
				Destroy(hitTetrominos[i].collider.gameObject);
			}
		}
	}
}
