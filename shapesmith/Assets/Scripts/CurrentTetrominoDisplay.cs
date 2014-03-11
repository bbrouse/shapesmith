using UnityEngine;
using System.Collections;

public class CurrentTetrominoDisplay : MonoBehaviour {

	public TextMesh tetrominoCountText;
	public TextMesh tetrominoWarningText;

	private PlacementManager placementManager;
	private GameObject playerCamera;
	private GameController gameController;
	private GameObject currentTetromino;
	private int currentShape = -1;

	// Use this for initialization
	void Start () {
		placementManager = GameObject.Find ("GameController").GetComponent<PlacementManager> ();
		playerCamera = GameObject.Find ("Main Camera");
		gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (currentShape != placementManager.currentShape) {
			switchTetromino();
		}
	}

	private void switchTetromino(){
		currentShape = placementManager.currentShape;

		Destroy (currentTetromino);
		currentTetromino = Instantiate (placementManager.tetrominoArray [currentShape], transform.position, transform.rotation) as GameObject;
		Destroy (currentTetromino.GetComponent<GravityHandler> ());
		currentTetromino.transform.parent = transform;
		currentTetromino.transform.localScale = new Vector3 (1, 1, 1);
		currentTetromino.transform.localPosition = new Vector3 (0, 0, 0);

		foreach (Transform child in currentTetromino.transform)
		{
			child.gameObject.layer = LayerMask.NameToLayer("3DGui");
		}

		Vector3 startingPosition = currentTetromino.transform.localPosition;
		Vector3 adjustment = new Vector3();
		if (currentTetromino.name == "Z-Tetromino(Clone)") {
			adjustment = new Vector3(1,0.5f,0);
		}
		else if(currentTetromino.name == "Reverse-Z-Tetromino(Clone)"){
			adjustment = new Vector3(-0.8f,0.6f,0);
		}
		else if(currentTetromino.name == "L-Tetromino(Clone)"){
			adjustment = new Vector3(0.5f,0.2f,0);
		}
		else if(currentTetromino.name == "Reverse-L-Tetromino(Clone)"){
			adjustment = new Vector3(-0.2f,0.1f,0);
		}
		else if(currentTetromino.name == "T-Tetromino(Clone)"){
			adjustment = new Vector3(0.05f,0.5f,0);
		}
		else if(currentTetromino.name == "O-Tetromino(Clone)"){
			adjustment = new Vector3(0.6f,0.6f,0);
		}
		else if(currentTetromino.name == "I-Tetromino(Clone)"){
			adjustment = new Vector3(0,-.4f,0);
		}
		currentTetromino.transform.localPosition = startingPosition + adjustment;
		tetrominoCountText.text = gameController.tetrominosLeft.ToString();
		if (gameController.tetrominosLeft == 0) {
			tetrominoWarningText.text = "Out of\nTetrominos";
			Destroy(currentTetromino);
		}
	}
}
