using UnityEngine;
using System.Collections;

public class HideAndLockCursor : MonoBehaviour {
	public GameObject player;
	public GameObject gameControllerObj;

	private CharacterMotor playerMotor;
	private MouseLook playerMouse;
	private MouseLook cameraMouse;
	private PlacementManager placementManager;
	private GameController gameController;

	//Starts with the cursor locked and grabs the components for player movement and player/camera mouse movement.
	void Start() {
		Screen.lockCursor = true;
		playerMotor = player.GetComponent<CharacterMotor> ();
		playerMouse = player.GetComponent<MouseLook> ();
		cameraMouse = Camera.main.GetComponent<MouseLook> ();
		placementManager = gameControllerObj.GetComponent<PlacementManager> ();
		gameController = gameControllerObj.GetComponent<GameController>();
	}

	//Sets all movement and mouse locks to false if true and true if false on pressing the p key.
	public void toggleCursorLock() {
		Screen.lockCursor = !Screen.lockCursor;
		playerMotor.enabled = !playerMotor.enabled;
		playerMouse.enabled = !playerMouse.enabled;
		cameraMouse.enabled = !cameraMouse.enabled;
		placementManager.enabled = !placementManager.enabled;
	}
}
