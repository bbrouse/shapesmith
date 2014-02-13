using UnityEngine;
using System.Collections;

public class PlacementManager : MonoBehaviour {

	public GameObject origin;
	public float maxDistance;
	public LayerMask layerMask;
	public GameObject placeholderObject;

	void Update () {
		Ray placementRay = new Ray (origin.transform.position, origin.transform.forward);
		RaycastHit hitInfo;

		Debug.DrawRay (origin.transform.position, origin.transform.forward * maxDistance);
		Physics.Raycast (placementRay, out hitInfo, maxDistance);
		if (Physics.Raycast (placementRay, out hitInfo, maxDistance, layerMask)) {
			Vector3 position = hitInfo.point;
			
			position.x = Mathf.Round(position.x);
			position.y = Mathf.Round(position.y);
			position.z = Mathf.Round(position.z);
			//Debug.Log(position + ", " + hitInfo.normal);
			
			placeholderObject.transform.position = position;
		}

		if (Input.GetMouseButtonDown (0)) {
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.layer = 8;
			cube.transform.position = placeholderObject.transform.position;
			Debug.Log(cube.layer);
		}
	}
}
