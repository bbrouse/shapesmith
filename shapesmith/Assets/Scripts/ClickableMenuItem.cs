using UnityEngine;
using System;
using System.Collections;

public class ClickableMenuItem : MonoBehaviour {

	public Color standardColor;
	public Color hoverColor;

	private Action method;

	public void setMethodToRun(Action inMethod){
		method = inMethod;
	}

	void OnMouseEnter(){
		guiText.color = hoverColor;
	}

	void OnMouseExit(){
		guiText.color = standardColor;
	}

	void OnMouseUpAsButton () {
		method ();
	}
}
