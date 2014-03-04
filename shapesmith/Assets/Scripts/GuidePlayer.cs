using UnityEngine;
using System.Collections;

public class GuidePlayer : MonoBehaviour {

	public GameObject target;
	public GameObject reference;
	public GameObject playerCamera;
		
	void Update () {
		reference.transform.LookAt (target.transform.position);
		//transform.rotation = Quaternion.FromToRotation(playerCamera.transform.forward, reference.transform.forward);
		transform.rotation = Quaternion.Inverse (playerCamera.transform.rotation) * reference.transform.rotation;
	}
}
