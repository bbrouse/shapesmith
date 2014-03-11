using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {
	
	void Start () {
		gameObject.GetComponent<ClickableMenuItem> ().setMethodToRun (startGame);
	}

	void startGame(){
		Application.LoadLevel("Beta_Level_0");
	}
}
