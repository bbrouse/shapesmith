using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public float tetrominoFallDelay;

	public bool checkObjectProximity(Vector3 pos){
		var hitColliders = Physics.OverlapSphere(pos, .4f);
		if (hitColliders.Length > 0) {
			return false;
		}
		return true;
	}
}
