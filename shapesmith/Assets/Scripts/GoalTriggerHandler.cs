using UnityEngine;
using System.Collections;

public class GoalTriggerHandler : MonoBehaviour {
	public GameController gameController;
	public int numLevels;

	void OnTriggerEnter(Collider other){
		if (other.collider.tag == "Player") {
			if(Application.loadedLevelName != "Main_Menu"){
				unlockLevel();
				gameController.GetComponent<GameController>().levelCompleted();
			}
		}
	}

	void unlockLevel(){
		PlayerPrefs.SetInt(Application.loadedLevelName, 1);
	}
}
