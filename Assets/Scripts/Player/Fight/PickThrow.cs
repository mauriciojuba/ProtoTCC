using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickThrow : MonoBehaviour {


	[SerializeField] private float Radius;
	[SerializeField] private Collider ColliderInRange;


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		LocateObject ();

	}


	void LocateObject(){
		Ray ray = new Ray (transform.position, transform.forward * 2);
		Debug.DrawRay (transform.position, transform.forward * 2, Color.red);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider.CompareTag ("Enemy")) {
				ColliderInRange = hit.collider;
			} else {
				ColliderInRange = null;
			}
		} else {
			ColliderInRange = null;
		}
	}

	void PickObject(){
		
	}

	void ThrowObject(){
		
	}
}
