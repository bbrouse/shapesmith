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
			
			
			if(hitInfo.transform.position.x % 2 == 0){
				if(position.x > hitInfo.transform.position.x){
					if(position.x < 0){
						position.x = (Mathf.Abs(position.x) + .1f) * -1;
					}else{
						position.x = position.x + .1f;
					}
				}else{
					if(position.x < 0){
						position.x = (Mathf.Abs(position.x) - .1f) * -1;
					}else{
						position.x = position.x - .1f;
					}
				}
			}
			position.x = Mathf.Round(position.x);
			
			if(hitInfo.transform.position.y % 2 == 0){
				if(position.y > hitInfo.transform.position.y){
					if(position.y < 0){
						position.y = (Mathf.Abs(position.y) + .1f) * -1;
					}else{
						position.y = position.y + .1f;
					}
				}else{
					if(position.y < 0){
						position.y = (Mathf.Abs(position.y) - .1f) * -1;
					}else{
						position.y = position.y - .1f;
					}
				}
			}
			position.y = Mathf.Round(position.y);
			
			if(hitInfo.transform.position.z % 2 == 0){
				if(position.z > hitInfo.transform.position.z){
					if(position.z < 0){
						position.z = (Mathf.Abs(position.z) + .1f) * -1;
					}else{
						position.z = position.z + .1f;
					}
				}else{
					if(position.z < 0){
						position.z = (Mathf.Abs(position.z) - .1f) * -1;
					}else{
						position.z = position.z - .1f;
					}
				}
			}
			position.z = Mathf.Round(position.z);
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
}
