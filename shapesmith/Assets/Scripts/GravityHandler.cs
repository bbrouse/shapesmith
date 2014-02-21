using UnityEngine;
using System.Collections;
using System.Linq;

public class GravityHandler : MonoBehaviour {
	
	public GameObject[] childCubes = new GameObject[4];
	public LayerMask environmentMask;
	
	private GameController gameController;
	private float raycastDistance = 0.6f; //ray that looks under a cube
	private bool hitEnvironment = false;
	private float fallDelay = 0.5f; //how often the object will drop down
	
	void Start(){
		//using find here should be ok since it's only on instantiation
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
		fallDelay = gameController.tetrominoFallDelay;
		
		//update position once every fall delay
		//InvokeRepeating("updatePosition", fallDelay, fallDelay);
		InvokeRepeating("gravity", fallDelay, fallDelay);
	}
	
	/*void updatePosition(){
		if (!hitEnvironment) {
			Invoke ("gravity", .001f);
		} else {
			InvokeRepeating("gravity", 2f, 5);
		}
	}*/
	
	void gravity(){
		for (int i = 0; i < 4; i++) {
			//all child cubes check if there's anything directly under them
			GameObject origin = childCubes[i];
			
			Ray gravityRay = new Ray (origin.transform.position, -Vector3.up);
			RaycastHit hitInfo;
			if (Physics.Raycast (gravityRay, out hitInfo, raycastDistance, environmentMask)) {
				//check that the discovered obejct is not a sibling cube
				if(!childCubes.Contains(hitInfo.transform.gameObject)){
					//stop moving now
					hitEnvironment = true;
					break;
				}
			}
			
			if(i == 3){
				hitEnvironment = false;
			}
		}
		
		//if we didn't hit anything, move position down
		if (!hitEnvironment) {
			transform.position += new Vector3 (0, -1, 0);
		}
	}
	
	/*
	 * Until I think of a better way to start checking again
	 * once some cubes were deleted as a result of a row completion,
	 * I'll go with this thundering herd solution.
	*/
	public void someRowDeleted(){
		hitEnvironment = false;
	}
}
