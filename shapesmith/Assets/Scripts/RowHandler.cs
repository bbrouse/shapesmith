using UnityEngine;
using System.Collections;

public class RowHandler : MonoBehaviour {
	
	public void checkCompletesRow(GameObject[] childCubes){
		for (int i = 0; i < childCubes.Length; i++) {
			//all child cubes check if there's anything directly under them
			GameObject origin = childCubes[i];
			
			//loop through each cube
			//check the x,y,z directions
			//fire a raycastAll in that direction until it hits an environment gameobject
			//see if the distance matches the number of objects hit
			//delete all gameobjects hit
	}
}
