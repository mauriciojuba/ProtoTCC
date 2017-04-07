using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickThrow : MonoBehaviour {


	[SerializeField] private float Radius;
	[SerializeField] private Collider ColliderInRange;
	[SerializeField] private GameObject PickedObj;
	[SerializeField] private Transform FixPoint;
	private int PlayerNum;
	[SerializeField] private bool CanPick;
	[Range(500,2000)]
	[SerializeField] private float Force;

	void Start () {
		PlayerNum = GetComponent<Movimentacao3D> ().PlayerNumber;
		CanPick = true;
	}
	
	// Update is called once per frame
	void Update () {
		LocateObject ();
		if (Input.GetButtonDown ("B P" + PlayerNum)) {
			if (CanPick)
				PickObject ();
			else
				ThrowObject ();
		}
	}


	void LocateObject(){
		Debug.DrawRay (transform.position - (transform.up / 2), transform.forward * 2, Color.red);
		RaycastHit hit;

		if (Physics.Raycast (transform.position  - (transform.up / 2),transform.forward, out hit,2)) {
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
		if (ColliderInRange == null)
			return;

		CanPick = false;
		PickedObj = ColliderInRange.gameObject;
		PickedObj.transform.SetParent (FixPoint);
		PickedObj.transform.position = FixPoint.position;
		PickedObj.transform.rotation = FixPoint.rotation;
		PickedObj.GetComponent<Rigidbody> ().isKinematic = true;
	}

	void ThrowObject(){
		PickedObj.transform.SetParent (null);
		PickedObj.GetComponent<Rigidbody> ().isKinematic = false;
		PickedObj.GetComponent<Rigidbody> ().AddForce (PickedObj.transform.forward * Force);
		PickedObj = null;
		CanPick = true;
	}
}
