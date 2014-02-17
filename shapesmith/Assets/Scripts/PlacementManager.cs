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

			position.x = getCoordinateFromHit(position.x, hitInfo.transform.position.x);
			position.y = getCoordinateFromHit(position.y, hitInfo.transform.position.y);
			position.z = getCoordinateFromHit(position.z, hitInfo.transform.position.z);

			//Debug.Log(position + ", " + hitInfo.normal);
			
			placeholderObject.transform.position = position;
		}
		
		if (Input.GetMouseButtonDown (0)) {
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.layer = 8;
			cube.transform.position = placeholderObject.transform.position;
			
			//Debug.Log(cube.layer);
		}
	}

	float getCoordinateFromHit(float positionCoordinate, float hitCoordinate){
		if(hitCoordinate % 2 == 0){
			if(positionCoordinate > hitCoordinate){
				positionCoordinate = positionCoordinate + 0.1f;
			}else{
				positionCoordinate = positionCoordinate - 0.1f;
			}
		}
		return Mathf.Round(positionCoordinate);
	}
}
