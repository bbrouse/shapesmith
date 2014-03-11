using UnityEngine;
using System.Collections;

public class TimerTetromino : MonoBehaviour {

	public GameObject[][] levels = new GameObject[4][];
	public GameObject[] level0 = new GameObject[2];
	public GameObject[] level1 = new GameObject[2];
	public GameObject[] level2 = new GameObject[2];
	public GameObject[] level3 = new GameObject[2];

	private GameController gameController;
	private int currentTime = -1;
	private int timeLimit = -1;
	private int interval = 0;
	private int currentTimeLevel = 0;
	private int finalTimeLimit = -1;

	void Start () {
		gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
		timeLimit = gameController.tetrominoTimeLimit;
		finalTimeLimit = gameController.finalTimeLimit;
		interval = timeLimit / 4;

		levels [0] = level0;
		levels [1] = level1;
		levels [2] = level2;
		levels [3] = level3;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentTime != gameController.tetrominoTimeLeft && gameController.tetrominosLeft > 0) {
			currentTime = gameController.tetrominoTimeLeft;
			updateTimer();
		}
		else if (currentTime != gameController.finalTimeLeft &&gameController.tetrominosLeft == 0) {
			timeLimit = gameController.finalTimeLimit;
			currentTime = gameController.finalTimeLeft;
			interval = finalTimeLimit / 4;
			updateTimer();
		}
	}

	private void updateTimer(){
		if (currentTime == timeLimit) {
			resetTimerTetromino();
		}
		else if (currentTime % interval == 0 && currentTime != timeLimit) {
			hideTimerLevel();
		}
	}

	private void resetTimerTetromino(){
		for (int i = 0; i < levels.Length; i++) {
			for(int j = 0; j < levels[i].Length; j++){
				levels[i][j].gameObject.renderer.enabled = true;
			}
		}
		currentTimeLevel = 0;
	}

	private void hideTimerLevel(){
		for (int i = 0; i < levels[currentTimeLevel].Length; i++) {
			GameObject objectToHide = levels[currentTimeLevel][i].gameObject;
			objectToHide.renderer.enabled = false;
		}
		currentTimeLevel += 1;
		if (currentTimeLevel == 4) {
			currentTimeLevel = 0;
		}
	}
}
