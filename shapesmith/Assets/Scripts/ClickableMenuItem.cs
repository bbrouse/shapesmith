using UnityEngine;
using System;
using System.Collections;

public class ClickableMenuItem : MonoBehaviour {

	public Color standardColor;
	public Color hoverColor;

	private Action method;

	void Start(){
	}

	public void setMethodToRun(Action inMethod){
		method = inMethod;
	}

	void OnMouseEnter(){
		Debug.Log("Mouse entered");
		guiText.color = hoverColor;
	}

	void OnMouseExit(){
		Debug.Log("Mouse exited");
		guiText.color = standardColor;
	}

	void OnMouseUpAsButton () {
		Debug.Log ("Text Clicked");
		method ();
	}
}
