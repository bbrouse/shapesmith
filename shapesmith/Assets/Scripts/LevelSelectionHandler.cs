using UnityEngine;
using System.Collections;

public class LevelSelectionHandler : MonoBehaviour {

	public string levelName;

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			Invoke("loadLevel", 0.5f);
		}
	}

	private void loadLevel(){
		Application.LoadLevel(levelName);
	}
}
