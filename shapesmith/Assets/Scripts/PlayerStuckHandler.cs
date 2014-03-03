using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStuckHandler : MonoBehaviour {
	public float maxDistance;
	public LayerMask environmentMask;
	public GameController gameController;

	private GameObject origin;
	private List<Ray> placementRays = new List<Ray>();
	private int hits = 0;
	private RaycastHit hitInfo;
	private bool drawRays = true;

	void Update () {
		if(drawRays){
			origin = this.transform.gameObject;
			
			placementRays.Add(new Ray (origin.transform.position + new Vector3(0.0f, 1f, 0.0f), origin.transform.forward));
			placementRays.Add(new Ray (origin.transform.position + new Vector3(0.0f, 1f, 0.0f), -origin.transform.forward));
			placementRays.Add(new Ray (origin.transform.position + new Vector3(0.0f, 1f, 0.0f), -origin.transform.up));
			placementRays.Add(new Ray (origin.transform.position + new Vector3(0.0f, 1f, 0.0f), origin.transform.right));
			placementRays.Add(new Ray (origin.transform.position + new Vector3(0.0f, 1f, 0.0f), -origin.transform.right));

			Debug.DrawRay(origin.transform.position + new Vector3(0.0f, 1f, 0.0f), origin.transform.forward * maxDistance);
			Debug.DrawRay(origin.transform.position + new Vector3(0.0f, 1f, 0.0f), -origin.transform.forward * maxDistance);
			Debug.DrawRay(origin.transform.position + new Vector3(0.0f, 1f, 0.0f), -origin.transform.up * (maxDistance + 1.5f));
			Debug.DrawRay(origin.transform.position + new Vector3(0.0f, 1f, 0.0f), origin.transform.right * maxDistance);
			Debug.DrawRay(origin.transform.position + new Vector3(0.0f, 1f, 0.0f), -origin.transform.right * maxDistance);

			for(int i=0; i<5; i++){
				if(i != 2){
					if(Physics.Raycast(placementRays[i], out hitInfo, maxDistance, environmentMask)){
						hits++;
					}
				}else{
					if(Physics.Raycast(placementRays[i], out hitInfo, maxDistance + 1.5f, environmentMask)){
						hits++;
					}
				}
				Debug.Log(hits);
			}
			
			if(hits == 5){
				drawRays = false;
				gameController.CancelInvoke("tetrominoTimerCountdown");
				gameController.InvokeRepeating("finalCountdown", 0.001f, 1.0f);
			}

			placementRays.Clear();
			hits = 0;
		}
	}
}
