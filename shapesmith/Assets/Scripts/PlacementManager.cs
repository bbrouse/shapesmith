using UnityEngine;
using System.Collections;

public class PlacementManager : MonoBehaviour {
	
	public GameObject origin;
	public float maxDistance;
	public LayerMask layerMask;
	public GameObject placeholderObject;

	public GameObject[] shapesArray = new GameObject[6];
	public GameObject[] tetrominoArray = new GameObject[5];
	private bool allowPlacement = true;
	private Vector3 startPos;

	void Update () {
		allowPlacement = true;
		Ray placementRay = new Ray (origin.transform.position, origin.transform.forward);
		RaycastHit hitInfo;
		
		Debug.DrawRay (origin.transform.position, origin.transform.forward * maxDistance);
		Physics.Raycast (placementRay, out hitInfo, maxDistance);
		if (Physics.Raycast (placementRay, out hitInfo, maxDistance, layerMask)) {
			Vector3 position = hitInfo.point;

			position.x = getCoordinateFromHit(position.x, hitInfo.collider.transform.position.x);
			position.y = getCoordinateFromHit(position.y, hitInfo.collider.transform.position.y);
			position.z = getCoordinateFromHit(position.z, hitInfo.collider.transform.position.z);

			if(hitInfo.collider.bounds.size.Equals(new Vector3(1.0f, 1.0f, 1.0f)) && isCornerHit(position, hitInfo.collider.transform.position)){
				//We dont' want to place the placeholder object because we are looking at a cube's corner
			}else if(!allowPlacement){
				//We don't want to allow placing objects over top of the player or other cubes
			}else{
				//placeholderObject.transform.position = position;
				shapesArray[1].transform.parent.gameObject.transform.position = position;
				allowPlacement = true;
			}

			for(int i=0; i<shapesArray[1].gameObject.transform.parent.childCount; i++){
				checkObjectProximity(shapesArray[1].gameObject.transform.parent.GetChild(i).position);
			}

			//Debug.Log(position + ", " + hitInfo.transform.position);
		}

		if (Input.GetMouseButtonDown (0) && allowPlacement == true) {
			/*GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.layer = 8;
			//cube.transform.position = placeholderObject.transform.position;
			cube.transform.position = shapesArray[1].transform.parent.gameObject.transform.position;*/

			Instantiate(tetrominoArray[0], shapesArray[1].transform.parent.gameObject.transform.position, Quaternion.identity);
		}
	}

	private bool checkObjectProximity(Vector3 pos){
		var hitColliders = Physics.OverlapSphere(pos, .4f);
		if (hitColliders.Length > 0) {
			for(int i = 0; i < hitColliders.Length; i++){
				if(hitColliders[i].gameObject.tag == "Player" || hitColliders[i].gameObject.tag == "Block"){
					allowPlacement = false;
				}
			}
		}
		return allowPlacement;
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
		float x = placementPosition.x;
		float y = placementPosition.y;
		float z = placementPosition.z;
		float newX = hitObjectPosition.x;
		float newY = hitObjectPosition.y;
		float newZ = hitObjectPosition.z;
		return ((x != newX && y != newY) || (y != newY && z != newZ) || (x != newX && z != newZ));
	}

}
