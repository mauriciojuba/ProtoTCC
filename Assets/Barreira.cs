using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barreira : MonoBehaviour {

	[SerializeField] GameObject[] AreaEnemys;
	[SerializeField] List<GameObject> DeadEnemys;
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (DeadEnemys.Count == AreaEnemys.Length) {
			Destroy (gameObject);
		}

		for (int i = 0; i < AreaEnemys.Length; i++) {
			if (AreaEnemys [i].GetComponent<FSMMosquito> ().state == FSMMosquito.FSMStates.Die) {
				if (!DeadEnemys.Contains (AreaEnemys [i])) {
					DeadEnemys.Add (AreaEnemys [i]);
				}
			}
		}

	}
}
