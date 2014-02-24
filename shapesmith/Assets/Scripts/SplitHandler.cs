using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplitHandler : MonoBehaviour {

	public LayerMask tetrominoMask;

	public void checkandHandleSplit(GameObject[] childCubes){
		for(int i = 0; i < childCubes.Length; i++){
			GameObject cube = childCubes[i];
			bool isOrphanCube = true;

			if(cube != null){
				Collider[] neighbors = Physics.OverlapSphere (cube.transform.position, 1.0f, tetrominoMask);
				for (int j = 0; j < neighbors.Length; j++) {
					GameObject neighborCube = neighbors[j].gameObject;
					GameObject neighborTetromino = neighborCube.transform.parent.gameObject;
					float distance = Vector3.Distance(cube.transform.position, neighborCube.transform.position);

					if(gameObject == neighborTetromino && distance == 1.0f){
						isOrphanCube = false;
						break;
					}
				}

				if(isOrphanCube){
					splitFromParent(cube);
				}
			}
		}
	}

	private void splitFromParent(GameObject orphanCube){
		GameObject tetromino = orphanCube.transform.parent.gameObject;
		GameObject cloneTetromino = (GameObject)Object.Instantiate(tetromino);
		List<GameObject> cubesToDestroy = new List<GameObject>();

		foreach (Transform child in cloneTetromino.transform)
		{
			if(child.position != orphanCube.transform.position){
				cubesToDestroy.Add(child.gameObject);
			}
		}

		foreach (Transform child in tetromino.transform)
		{
			if(child.position == orphanCube.transform.position){
				cubesToDestroy.Add(child.gameObject);
				break;
			}
		}

		for (int i = 0; i < cubesToDestroy.Count; i++) {
			DestroyImmediate(cubesToDestroy[i]);	
		}
	}
}
