using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barreira : MonoBehaviour {

	[SerializeField] GameObject[] AreaEnemys;
	[SerializeField] Rigidbody dominoCenterLeft,dominoCenterRight;
	[SerializeField] Vector3 forceDir;
	[SerializeField] List<GameObject> DeadEnemys;
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (DeadEnemys.Count == AreaEnemys.Length) {
			dominoCenterLeft.AddForce(forceDir*2000f);
			dominoCenterRight.AddForce(-forceDir*2000f);
			this.GetComponent<BoxCollider>().enabled = false;
			Destroy (this,10);
		}

		for (int i = 0; i < AreaEnemys.Length; i++) {
			if (AreaEnemys [i].GetComponent<FSMMosquito> () != null && AreaEnemys [i].GetComponent<FSMMosquito> ().state == FSMMosquito.FSMStates.Die) {
				if (!DeadEnemys.Contains (AreaEnemys [i])) {
					DeadEnemys.Add (AreaEnemys [i]);
				}
			} else if (AreaEnemys [i].GetComponent<FSMAranha> () != null && AreaEnemys [i].GetComponent<FSMAranha> ().state == FSMAranha.FSMStates.Die) {
				if (!DeadEnemys.Contains (AreaEnemys [i])) {
					DeadEnemys.Add (AreaEnemys [i]);
				}
			}
		}

	}
}
