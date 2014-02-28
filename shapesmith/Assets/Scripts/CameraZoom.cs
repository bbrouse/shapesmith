using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {

	public int cameraZoom = 20;
	public Camera mainCamera;
	private bool isZoomed = false;
	private int normal = 60;
	private float smooth = 10;
	
	void Update () {
		if(isZoomed == true){
			mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, cameraZoom, Time.deltaTime*smooth);
		}
		else{
			mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, normal, Time.deltaTime*smooth);
		}
	}

	public void toggleZoom(){
		isZoomed = !isZoomed;
	}
}
