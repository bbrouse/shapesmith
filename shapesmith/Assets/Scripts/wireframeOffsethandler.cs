using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class wireframeOffsethandler : MonoBehaviour {
	public float maxDistance;
	public LayerMask environmentMask;

	private GameObject origin;
	private List<Ray> placementRays = new List<Ray>();
	private RaycastHit hitInfo;

	void Update () {
		origin = this.transform.parent.gameObject;

		placementRays.Add(new Ray (origin.transform.position, origin.transform.forward));
		placementRays.Add(new Ray (origin.transform.position, -origin.transform.forward));
		placementRays.Add(new Ray (origin.transform.position, origin.transform.up));
		placementRays.Add(new Ray (origin.transform.position, -origin.transform.up));
		placementRays.Add(new Ray (origin.transform.position, origin.transform.right));
		placementRays.Add(new Ray (origin.transform.position, -origin.transform.right));

		for(int i=0; i<6; i++){
			if(Physics.Raycast(placementRays[i], out hitInfo, maxDistance, environmentMask)){
				origin.transform.Translate((hitInfo.normal * .01f), Space.World);
			}
		}
	}
}
