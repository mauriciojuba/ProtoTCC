using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMoviment : MonoBehaviour {

	public float X,Y,Z;

	[SerializeField] private bool moveTo;
	[SerializeField] private bool InScreen;
	Vector3 pos;
	[SerializeField] private Vector3 posZ;
	[SerializeField] private Vector3 Offset;
	[SerializeField] private Vector3 RotationP;
	void Start () {
		
	}
	
	void Update () {
			pos = Camera.main.WorldToViewportPoint (transform.position);
			pos.x = Mathf.Clamp01 (pos.x);
			pos.y = Mathf.Clamp01 (pos.y);
			transform.position = Camera.main.ViewportToWorldPoint (pos);
		if(moveTo) {
			if (!InScreen) {
				transform.position = Vector3.MoveTowards (transform.position, Camera.main.ViewportToWorldPoint (new Vector3 (pos.x, pos.y, 3)), 10 * Time.deltaTime);
				transform.localEulerAngles = Vector3.MoveTowards (transform.localEulerAngles, new Vector3(Camera.main.transform.localEulerAngles.x,transform.localEulerAngles.y,transform.localEulerAngles.z) , Time.deltaTime * 50);
				if (transform.position == Camera.main.ViewportToWorldPoint (new Vector3 (pos.x, pos.y, 3))  && transform.localEulerAngles == new Vector3(Camera.main.transform.localEulerAngles.x,transform.localEulerAngles.y,transform.localEulerAngles.z)) {
					Offset =Camera.main.ViewportToWorldPoint (new Vector3 (pos.x, pos.y, 3)) - posZ;
					InScreen = true;
					moveTo = false;
				}
			} else {
				transform.position = Vector3.MoveTowards (transform.position, new Vector3(transform.position.x, posZ.y,posZ.z), 5 * Time.deltaTime);
				transform.localEulerAngles = Vector3.MoveTowards (transform.localEulerAngles, RotationP , Time.deltaTime * 50);
				if (transform.position == new Vector3(transform.position.x ,posZ.y,posZ.z) && transform.eulerAngles == RotationP) {
					InScreen = false;
					moveTo = false;
				}
			}
		}
	
		if (!moveTo && Input.GetButtonDown("LB P1")) {
			if (!InScreen) {
				posZ = transform.position;
				RotationP = transform.localEulerAngles;
				transform.LookAt (Camera.main.transform);
			}
			moveTo = true;
		}
	}

	void OnEnable(){
		
	}

}
