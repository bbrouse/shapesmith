using UnityEngine;
using System.Collections;
using System.Linq;

public class GravityHandler : MonoBehaviour {
	
	public GameObject[] childCubes = new GameObject[4];
	public LayerMask environmentMask;
	public LayerMask tetrominoMask;
	public bool isMoving = false;
	public RowHandler rowHandler;
	
	private GameController gameController;
	private float raycastDistance = 0.6f; //ray that looks under a cube
	private bool hitEnvironment = false;
	private bool gravityCheckEnabled = true;
	private float fallDelay = 0.5f; //how often the object will drop down
	
	void Start(){
		//using find here should be ok since it's only on instantiation
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
		fallDelay = gameController.tetrominoFallDelay;
		
		//update position once every fall delay
		InvokeRepeating("gravity", fallDelay, fallDelay);
	}
	
	void gravity(){
		if(gravityCheckEnabled){
			for (int i = 0; i < 4; i++) {
				//all child cubes check if there's anything directly under them
				GameObject origin = childCubes[i];
				if(origin != null){
					Ray gravityRay = new Ray (origin.transform.position, -Vector3.up);
					RaycastHit hitInfo;
					if (Physics.Raycast (gravityRay, out hitInfo, raycastDistance, environmentMask | tetrominoMask)) {
						Debug.Log(hitInfo.transform.gameObject.name);
						//check that the discovered object is not a sibling cube
						if(!childCubes.Contains(hitInfo.transform.gameObject)){
							//stop moving now
							hitEnvironment = true;
							isMoving = false;
							
							//since we're stopped, let the rowHandler handle row completions
							rowHandler.checkCompletesRow(childCubes);
							gravityCheckEnabled = false;
							break;
						}
					}
				}
				if(i == 3){
					hitEnvironment = false;
				}
			}
		
			//if we didn't hit anything, move position down
			if (!hitEnvironment) {
				isMoving = true;
				transform.position += new Vector3 (0, -1, 0);
				alertNeighbors();
			}
		}
	}

	//let neighboring tetros know that we're moving so they can start checking again
	private void alertNeighbors(){
		Collider[] neighbors = Physics.OverlapSphere (transform.position, 3.0f, tetrominoMask);
		for (int i = 0; i < neighbors.Length; i++) {
			GameObject neighbor_tetromino = neighbors[i].gameObject.transform.parent.gameObject;
			neighbor_tetromino.GetComponent<GravityHandler>().enableGravityCheck();
		}
	}

	public void enableGravityCheck(){
		gravityCheckEnabled = true;
	}
}
