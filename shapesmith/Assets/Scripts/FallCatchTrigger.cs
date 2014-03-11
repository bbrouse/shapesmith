using UnityEngine;
using System.Collections;

public class FallCatchTrigger : MonoBehaviour {

	public GameController gameController;
	
	void OnTriggerEnter(Collider other){
		Debug.Log (other.collider.tag);
		if (other.collider.tag == "Player") {
			gameController.GetComponent<GameController>().levelFailed();
		}
		else if(other.collider.tag == "Block"){
			Destroy(other.gameObject);
		}
	}
}
