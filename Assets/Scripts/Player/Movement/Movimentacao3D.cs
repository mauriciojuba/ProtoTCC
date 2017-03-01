using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimentacao3D : MonoBehaviour {


	//Array de direções que o player pode olhar
	public Transform[] Directions;
	//Direção atual que o player esta olhando
	public int ActualDirection;

	//define a velocidade de movimentação do player
	public float Speed;

	//define se o player esta em movimento ou não.
	public bool InMovement;

	//Define o numero do Player.
	public int PlayerNumber;

	//GO do personagem que sera movimentado.
	public GameObject Player;

	public Camera CameraMain;
	void Start () {
		ActualDirection = 3;
	}

	void Update(){
		TestPosition ();
	}

	void FixedUpdate () {
		DirectionDefinition ();
	}


	//Define a direção que o player olha de acordo com os botoes que ele aperta.
	void DirectionDefinition(){
			if (Input.GetAxisRaw ("Vertical P" + PlayerNumber) > 0) {
				InMovement = true;
				//Define a direção para Cima-Esquerda.
				if (Input.GetAxisRaw ("Horizontal P" + PlayerNumber) < 0) {
					ActualDirection = 4;
				} 
			//Define a direção para Cima-Direita.
			else if (Input.GetAxisRaw ("Horizontal P" + PlayerNumber) > 0) {
					ActualDirection = 5;
				} 
			//Define a direção para Cima.
			else {
					ActualDirection = 0;
				}
			} else if (Input.GetAxisRaw ("Vertical P" + PlayerNumber) < 0) {
				InMovement = true;
				//Define a direção para Baixo-Esquerda
				if (Input.GetAxisRaw ("Horizontal P" + PlayerNumber) < 0) {
					ActualDirection = 6;
				} 
			//Define a direção para Baixo-Direita
			else if (Input.GetAxisRaw ("Horizontal P" + PlayerNumber) > 0) {
					ActualDirection = 7;
				} 
			//Define a direção para Baixo.
			else {
					ActualDirection = 1;
				}
			} 
		//Define a direção para Esquerda
		else if (Input.GetAxisRaw ("Horizontal P" + PlayerNumber) < 0) {
				InMovement = true;
				ActualDirection = 2;
			} 
		//Define a direção para Direita
		else if (Input.GetAxisRaw ("Horizontal P" + PlayerNumber) > 0) {
				InMovement = true;
				ActualDirection = 3;
			} else {
				InMovement = false;
			}
			transform.LookAt (new Vector3 (Directions [ActualDirection].position.x, transform.position.y, Directions [ActualDirection].position.z));

			//Define se o Player esta em movimento ou não.
			if (InMovement)
				Player.GetComponent<Rigidbody> ().velocity = transform.forward * Speed;
			else
				Player.GetComponent<Rigidbody> ().velocity = transform.forward * 0;
	}
	void TestPosition(){
		//Calcula a distancia entre o personagem e a camera.
		var DistanceZ = (transform.position - CameraMain.transform.position).z;

		//Calcula o ponto maximo de movimentação pra esquerda.
		var Leftborder = CameraMain.ViewportToWorldPoint (new Vector3 (0.01f, 0, DistanceZ)).x;

		//Calcula o ponto maximo de movimentação pra direita.
		var Rightborder = CameraMain.ViewportToWorldPoint (new Vector3 (0.99f, 0, DistanceZ)).x;

		//Calcula o ponto maximo de movimentação pra Baixo.
		var Bottomborder = CameraMain.ViewportToWorldPoint (new Vector3 (0, 0, 3)).z;

		//Calcula o ponto maximo de movimentação pra Cima.
		var Topborder = CameraMain.ViewportToWorldPoint (new Vector3 (0, 0, 48)).z;

		//Mantem o personagem sempre dentro do espaço da camera.
		transform.position = new Vector3 (
			Mathf.Clamp (transform.position.x, Leftborder, Rightborder),
			/*Mathf.Clamp (*/transform.position.y/*, Bottomborder, Topborder)*/,
			Mathf.Clamp (transform.position.z,Bottomborder,Topborder ));
	}
}

