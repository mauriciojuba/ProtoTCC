using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	public Transform Target;
	public Vector3 Offset;


	Vector3 Distan;
	public Transform P1,P2;

	float PosX,PosZ;
	Vector3 DistanMax;
	void Start () {
		Offset = transform.position - Target.position;
	}
	
	// Update is called once per frame
	void Update () {
		Distan = Dist (P1.position, P2.position);

		if (Distan.x < 50)
			PosX = Target.position.x + Offset.x;
		if (Distan.z < 30)
			PosZ = Target.position.z + Offset.z;
			
		DistanMax = new Vector3 (PosX, transform.position.y, PosZ);

		transform.position = Vector3.MoveTowards (transform.position, DistanMax, 0.5f);
	}

	Vector3 Dist(Vector3 A,Vector3 B){
		float DistX = A.x - B.x;
		float DistZ = A.z - B.z;

		float CalcX = Mathf.Sqrt (DistX * DistX);
		float CalcZ = Mathf.Sqrt (DistZ * DistZ);

		Vector3 Result = new Vector3 (CalcX, 0, CalcZ);
		return Result;
	}
}
