﻿using UnityEngine;
using System.Collections;

public class FallCatchTrigger : MonoBehaviour {

	public GameController gameController;
	
	void OnTriggerEnter(Collider other){
		if (other.collider.tag == "Player") {
			gameController.GetComponent<GameController>().levelFailed();
		}
	}
}