using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickThrow : MonoBehaviour {


	[SerializeField] private float Radius;
	[SerializeField] private Collider ColliderInRange;
	[SerializeField] private GameObject PickedObj;
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
		if (Input.GetButtonDown ("B P" + PlayerNum) && !Grabbing) {
			if (CanPick) {
				transform.parent.GetComponent<Movimentacao3D> ().SetGrabAnim ();
			} else {
				transform.parent.GetComponent<Movimentacao3D> ().SetGrabAnim ();
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
			PickedObj = ColliderInRange.gameObject;
			PickedObj.transform.SetParent (FixPoint2);
			PickedObj.transform.position = FixPoint2.position;
			PickedObj.transform.eulerAngles = new Vector3(0,0,0);
			PickedObj.GetComponent<Collider>().enabled = false;
			PickedObj.GetComponent<Rigidbody> ().isKinematic = true;
		}
	}

	void ThrowObject(){
		if (PickedObj.CompareTag ("Box")) {
			PickedObj.transform.SetParent (null);
			PickedObj.GetComponent<Rigidbody> ().isKinematic = false;
			PickedObj.GetComponent<Rigidbody> ().AddForce (PickedObj.transform.forward * Force);
			PickedObj = null;
			CanPick = true;
		} else if (PickedObj.CompareTag ("Enemy")) {
			transform.parent.GetComponent<Movimentacao3D> ().SetGrabbedAnim (false);
			PickedObj.transform.SetParent (FixPoint);
			PickedObj.transform.position = FixPoint.position;
			PickedObj.transform.eulerAngles = new Vector3(0,0,0);
		}
	}

	public void ThrowEnemy(){
		PickedObj.transform.SetParent (null);
		PickedObj.GetComponent<Rigidbody> ().isKinematic = false;
		PickedObj.GetComponent<Rigidbody> ().AddForce (transform.forward * Force);
		PickedObj.GetComponent<Collider>().enabled = true;
		PickedObj = null;
		CanPick = true;
	}

	public void SetGrabbing(){
		Grabbing = !Grabbing;
		transform.parent.GetComponent<Movimentacao3D> ().CanMove = !Grabbing;
		transform.parent.GetComponent<Movimentacao3D> ().SetGrabbingAnim (Grabbing);
	}

	public void SetGrabbedFalse(){
		transform.parent.GetComponent<Movimentacao3D> ().SetGrabbedFalse ();
	}
}
