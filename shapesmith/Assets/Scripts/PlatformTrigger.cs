using UnityEngine;
using System.Collections;

public class PlatformTrigger : MonoBehaviour {

	public float platformSpeed;
	private bool triggered = false;

	void OnTriggerEnter(Collider other){
		Debug.Log("Trigger entrered");
		if (other.collider.tag == "Player" && !triggered) {
			Debug.Log("Start platform");
			transform.parent.animation["level_2_moving_platform"].speed = platformSpeed;
			transform.parent.animation.Play("level_2_moving_platform");
		}

		triggered = true;
	}
}
