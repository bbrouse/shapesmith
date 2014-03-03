using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SkyTetrominoSpawning : MonoBehaviour {

	List<Transform> spawns = new List<Transform>();
	public GameObject[] tetrominoArray = new GameObject[7];

	private int lastSpawn = 0;
	
	void Start () {
		foreach (Transform child in transform) {
			spawns.Add(child);
		}
		InvokeRepeating ("spawnRandomTetromino", 0.0f, 1.0f);
	}
	
	void spawnRandomTetromino(){
		int spawnIndex = Random.Range (0, spawns.Count - 1);
		while (spawnIndex == lastSpawn) {
			spawnIndex = Random.Range (0, spawns.Count - 1);
		}
		int shapeIndex = Random.Range (0, tetrominoArray.Length - 1);
		Instantiate(tetrominoArray[shapeIndex], spawns[spawnIndex].position, spawns[spawnIndex].rotation);
		lastSpawn = spawnIndex;
	}
}
