using UnityEngine;
using System.Collections;

public class GoalTriggerHandler : MonoBehaviour {

	public GameController gameController;

	void OnTriggerEnter(Collider other){
		if (other.collider.tag == "Player") {
			gameController.GetComponent<GameController>().levelCompleted();
		}
	}
}
