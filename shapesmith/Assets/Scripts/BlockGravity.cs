using UnityEngine;
using System.Collections;

public class BlockGravity : MonoBehaviour {
	public GameObject tetromino;
	public GameController gameController;

	private bool gravityOn = true;
	private int invoke = 0;
	private bool allowTranslate = false;
	private Vector3[] tetBlocks = new Vector3[4];
	private bool[] lowestBlock = new bool[4];
	private float lowestPos = 100000;

	void Start(){
		for (int i=0; i<4; i++) {
			tetBlocks[i] = new Vector3(1000, 1000, 1000);
			
			tetBlocks[i] = tetromino.transform.GetChild(i).position;

			if(tetBlocks[i].y < lowestPos) lowestPos = tetBlocks[i].y;
		}
	}

	void Update () {
		if (gravityOn == true) {
			for (int i=0; i<4; i++) {
				if (tetBlocks[i].y > 0) invoke++;
				if(tetBlocks[i].y == lowestPos) lowestBlock[i] = true;
			}

			if(invoke == 4){
				invoke = 0;
				gravityOn = false;
				InvokeRepeating("tetrominoGravity", .5f, .25f);
			}
		}
	}

	void tetrominoGravity(){
		for (int i=0; i<4; i++) {
			if(lowestBlock[i] == true){
				tetBlocks[i].y -= 1;
				if(gameController.checkObjectProximity(tetBlocks[i]) && tetBlocks[i].y >= 0){
					allowTranslate = true;
				}else{
					allowTranslate = false;
					break;
				}
			}
		}

		if (allowTranslate == true) {
			tetromino.transform.Translate (Vector3.up * -1, Space.World);
			allowTranslate = false;
		} else {
			CancelInvoke();
		}
	}
}
