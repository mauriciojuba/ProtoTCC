﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickThrow : MonoBehaviour {


	[SerializeField] private float Radius;
	[SerializeField] private Collider ColliderInRange;
	[SerializeField] private GameObject PickedObj,PickedCollider;
	[SerializeField] private Transform FixPoint, FixPoint2;
	private int PlayerNum;
	[SerializeField] private bool CanPick;
	[Range(500,2000)]
	[SerializeField] private float Force;
	[SerializeField] private bool Grabbing;

	void Start () {
		PlayerNum = transform.parent.GetComponent<Movimentacao3D> ().PlayerNumber;
		CanPick = true;
	}
	
	// Update is called once per frame
	void Update () {
		LocateObject ();
		if (PickedCollider != null) {
			if (PickedCollider.GetComponent<FSMMosquito> () != null) {
				if (PickedCollider.GetComponent<FSMMosquito> ().state == FSMMosquito.FSMStates.Die) {
					PickedCollider.transform.SetParent (null);
					PickedCollider.GetComponent<Rigidbody> ().isKinematic = false;
					PickedCollider.transform.eulerAngles = new Vector3(0,0,0);
					PickedCollider.GetComponent<Rigidbody> ().AddForce (transform.forward * Force);
					PickedCollider.GetComponent<Collider>().isTrigger = false;
					PickedObj = null;
					PickedCollider = null;
					CanPick = true;
					transform.parent.GetComponent<Movimentacao3D> ().CanMove = true;
					transform.parent.GetComponent<Movimentacao3D> ().SetGrabbedAnim (false);
				}
			}
		}
		if (PlayerNum != 0) {
			if (Input.GetButtonDown ("B P" + PlayerNum) && !Grabbing) {
				if (CanPick) {
					transform.parent.GetComponent<Movimentacao3D> ().SetGrabAnim ();
				} else {
					transform.parent.GetComponent<Movimentacao3D> ().SetGrabAnim ();
				}
			}
		}
	}


	void LocateObject(){
		Debug.DrawRay (transform.position + (transform.up / 2), transform.forward * 2, Color.red);
		RaycastHit hit;

		if (Physics.Raycast (transform.position  + (transform.up / 2),transform.forward, out hit,2)) {
			if (hit.collider.CompareTag ("Enemy") || hit.collider.CompareTag("Box")) {
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

		if (ColliderInRange.CompareTag ("Box")) {
			CanPick = false;
			PickedObj = ColliderInRange.gameObject;
			PickedObj.transform.SetParent (FixPoint);
			PickedObj.transform.position = FixPoint.position;
			PickedObj.transform.rotation = FixPoint.rotation;
			PickedObj.GetComponent<Rigidbody> ().isKinematic = true;
		} else if (ColliderInRange.CompareTag ("Enemy")) {
			transform.parent.GetComponent<Movimentacao3D> ().SetGrabbedAnim (true);
			CanPick = false;
			PickedCollider = ColliderInRange.gameObject;
			//PickedObj = ColliderInRange.gameObject.GetComponentInChildren<PointOfGrab>().gameObject;
			PickedCollider.transform.SetParent (FixPoint2);
			PickedCollider.transform.position = FixPoint2.position + PickedCollider.GetComponentInChildren<PointOfGrab>().Offset;
			PickedCollider.GetComponent<Collider>().isTrigger = true;
			PickedCollider.GetComponent<Rigidbody> ().isKinematic = true;
			if (PickedCollider.GetComponent<FSMMosquito> () != null)
				PickedCollider.GetComponent<FSMMosquito> ().state = FSMMosquito.FSMStates.Grappled;
		}
	}

	void ThrowObject(){
		if (PickedCollider.CompareTag ("Box")) {
			PickedObj.transform.SetParent (null);
			PickedObj.GetComponent<Rigidbody> ().isKinematic = false;
			PickedObj.GetComponent<Rigidbody> ().AddForce (PickedObj.transform.forward * Force);
			PickedObj = null;
			CanPick = true;
		} else if (PickedCollider.CompareTag ("Enemy")) {
			transform.parent.GetComponent<Movimentacao3D> ().SetGrabbedAnim (false);
			PickedCollider.transform.SetParent (FixPoint);
			PickedCollider.transform.position = FixPoint.position + PickedCollider.GetComponentInChildren<PointOfGrab> ().Offset2;
			PickedCollider.transform.eulerAngles = new Vector3(0,0,0);
			//PickedObj.transform.position = FixPoint.position;
			//PickedObj.transform.eulerAngles = new Vector3(0,0,0);

		}
	}

	public void ThrowEnemy(){
		if (PickedCollider.GetComponent<FSMMosquito> () != null)
			PickedCollider.GetComponent<FSMMosquito> ().state = FSMMosquito.FSMStates.Thrown;
		PickedCollider.transform.SetParent (null);
		PickedCollider.GetComponent<Rigidbody> ().isKinematic = false;
		PickedCollider.transform.eulerAngles = new Vector3(0,0,0);
		PickedCollider.GetComponent<Rigidbody> ().AddForce (transform.forward * Force);
		PickedCollider.GetComponent<Collider>().isTrigger = false;
		PickedObj = null;
		PickedCollider = null;
		CanPick = true;
	}

	public void SetGrabbing(){
		Grabbing = !Grabbing;
		transform.parent.GetComponent<Movimentacao3D> ().CanMove = !Grabbing;
		transform.parent.GetComponent<Movimentacao3D> ().SetGrabbingAnim (Grabbing);
	}

	public void SetGrabbedFalse(){
		Grabbing = !Grabbing;
		transform.parent.GetComponent<Movimentacao3D> ().CanMove = !Grabbing;
		transform.parent.GetComponent<Movimentacao3D> ().SetGrabbedFalse ();
	}

	public void CanMoveFalse(){
		transform.parent.GetComponent<Movimentacao3D>().CanMove = false;
	}

	public void CanMoveTrue(){
		transform.parent.GetComponent<Movimentacao3D>().CanMove = true;
	}
}
