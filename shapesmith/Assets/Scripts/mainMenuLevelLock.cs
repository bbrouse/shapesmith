﻿using UnityEngine;
using System.Collections;

public class mainMenuLevelLock : MonoBehaviour {
	public int numLevels;

	private int[] levels;

	void Start () {
		levels = new int[numLevels];

		for (int i=0; i<levels.Length; i++) {
			if(PlayerPrefs.GetInt("Level_" + (i+1), -1) == -1){
				PlayerPrefs.SetInt("Level_" + (i+1), 0);
			}else if(PlayerPrefs.GetInt("Level_" + (i+1)) == 1){
				var level = GameObject.Find("Level_" + (i+2));
				level.collider.isTrigger = true;
				level.renderer.material = (Material)Resources.Load("goal", typeof(Material));
			}
		}
	}
}