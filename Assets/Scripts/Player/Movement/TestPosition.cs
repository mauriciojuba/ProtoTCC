using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPosition : MonoBehaviour {

	public Camera CameraMain;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var DistanceZ = (transform.position - CameraMain.transform.position).z;

		var Leftborder = CameraMain.ViewportToWorldPoint (new Vector3 (0, 0, DistanceZ)).x;

		var Rightborder = CameraMain.ViewportToWorldPoint (new Vector3 (1, 0, DistanceZ)).x;

		var Topborder = CameraMain.ViewportToWorldPoint (new Vector3 (0, 0, DistanceZ)).y;

		var Bottomborder = CameraMain.ViewportToWorldPoint (new Vector3 (0, 1.2f, DistanceZ)).y;

		transform.position = new Vector3 (
			Mathf.Clamp (transform.position.x, Leftborder, Rightborder),
			Mathf.Clamp (transform.position.y, Topborder, Bottomborder), 
			transform.position.z);
		
	}
}
