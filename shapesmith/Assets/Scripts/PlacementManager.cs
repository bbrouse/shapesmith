using UnityEngine;
using System.Collections;

public class PlacementManager : MonoBehaviour {
	
	public GameObject origin;
	public float maxDistance;
	public LayerMask layerMask;
	public GameObject placeholderObject;
	private float x, y, z, newX, newY, newZ;
	private bool allowPlacement = true;
	
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

			if(hitInfo.transform.tag == "block" && isCornerHit(position, hitInfo.transform.position)){
				//We dont' want to place the placeholder object because we are looking at a cube's corner
			}else{
				placeholderObject.transform.position = position;
				allowPlacement = true;
			}

			checkPlayerProximity(position);

			//Debug.Log(position + ", " + hitInfo.transform.position);
		}

		if (Input.GetMouseButtonDown (0) && allowPlacement == true) {
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.layer = 8;
			cube.tag = "block";
			cube.transform.position = placeholderObject.transform.position;
			
			//Debug.Log(cube.layer);
		}
	}

	private void checkPlayerProximity(Vector3 pos){
		var hitColliders = Physics.OverlapSphere(pos, .7f);
		if (hitColliders.Length > 0) {
			for(int i = 0; i < hitColliders.Length; i++){
				if(hitColliders[i].gameObject.tag == "Player"){
					allowPlacement = false;
					break;
				}
			}
		}
	}
	
	private float getCoordinateFromHit(float positionCoordinate, float hitCoordinate){
		if(hitCoordinate % 2 == 0){
			if(positionCoordinate > hitCoordinate){
				positionCoordinate = positionCoordinate + 0.1f;
			}else{
				positionCoordinate = positionCoordinate - 0.1f;
			}
		}
		return Mathf.Round(positionCoordinate);
	}

	private bool isCornerHit(Vector3 placementPosition, Vector3 hitObjectPosition){
		x = placementPosition.x;
		y = placementPosition.y;
		z = placementPosition.z;
		newX = hitObjectPosition.x;
		newY = hitObjectPosition.y;
		newZ = hitObjectPosition.z;
		return ((x != newX && y != newY) || (y != newY && z != newZ) || (x != newX && z != newZ));
	}

}
