using UnityEngine;
using System.Collections;

public class GoalTriggerHandler : MonoBehaviour {

	public GameController gameController;

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			gameController.GetComponent<GameController>().levelCompleted();
		}
	}
}
