using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	//Alvo que a camera centraliza.
	public Transform Target;
	//Vector para registrar a distancia entre a camera e o player
	[SerializeField] private Vector3 Offset;

	private float OffsetY,OffsetZ;

	//Vector para registrar a distancia entre os players
	Vector3 Distan;
	//Transforms dos players.
	public Transform P1,P2;

	//Float para registrar a distancia da camera em X e em Z
	float PosX,PosZ;
	//Vector para registrar a distancia maxima que a camera pode chegar.
	Vector3 DistanMax;


	void Start () {
		//marca a diferença entre a camera e o ponto de foco.
		OffsetY = transform.position.y - Target.position.y;
		OffsetZ = transform.position.z - Target.position.z;

		Offset = new Vector3 (0, OffsetY, OffsetZ);
	}

	void Update () {
		//marca a distancia entre os 2 players.
		Distan = Dist (P1.position, P2.position);

		//marca a Posição em X da camera.
		if (Distan.x < 50)
			PosX = Target.position.x + Offset.x;
		//marca a posição em Z da camera.
		if (Distan.z < 30)
			PosZ = Target.position.z + Offset.z;

		//Marca a distancia maxima que a camera pode ir.
		DistanMax = new Vector3 (PosX, transform.position.y, PosZ);

		//Move a camera
		transform.position = Vector3.MoveTowards (transform.position, DistanMax, 0.5f);
	}


	//Cálculo para encontrar o meio entre 2 pontos.
	Vector3 Dist(Vector3 A,Vector3 B){
		float DistX = A.x - B.x;
		float DistZ = A.z - B.z;

		float CalcX = Mathf.Sqrt (DistX * DistX);
		float CalcZ = Mathf.Sqrt (DistZ * DistZ);

		Vector3 Result = new Vector3 (CalcX, 0, CalcZ);
		return Result;
	}
}
