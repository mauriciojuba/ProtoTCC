using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMoviment : MonoBehaviour {

	public float X,Y,Z;

	void Start () {
		
	}
	
	void Update () {
		Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
		transform.position = Camera.main.ScreenToWorldPoint(pos);
	}
}
