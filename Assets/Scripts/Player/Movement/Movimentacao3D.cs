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

	void Start () {
		ActualDirection = 3;
	}
	
	void FixedUpdate () {
		DirectionDefinition ();
	}


	//Define a direção que o player olha de acordo com os botoes que ele aperta.
	void DirectionDefinition(){
		if (Input.GetAxisRaw("Vertical") > 0) {
			InMovement = true;
			//Define a direção para Cima-Esquerda.
			if (Input.GetAxisRaw("Horizontal") < 0) {
				ActualDirection = 4;
			} 
			//Define a direção para Cima-Direita.
			else if (Input.GetAxisRaw("Horizontal") > 0) {
				ActualDirection = 5;
			} 
			//Define a direção para Cima.
			else {
				ActualDirection = 0;
			}
		} 
		else if (Input.GetAxisRaw("Vertical") < 0) {
			InMovement = true;
			//Define a direção para Baixo-Esquerda
			if (Input.GetAxisRaw("Horizontal") < 0) {
				ActualDirection = 6;
			} 
			//Define a direção para Baixo-Direita
			else if (Input.GetAxisRaw("Horizontal") > 0) {
				ActualDirection = 7;
			} 
			//Define a direção para Baixo.
			else {
				ActualDirection = 1;
			}
		} 
		//Define a direção para Esquerda
		else if (Input.GetAxisRaw("Horizontal") < 0) {
			InMovement = true;
			ActualDirection = 2;
		} 
		//Define a direção para Direita
		else if (Input.GetAxisRaw("Horizontal") > 0) {
			InMovement = true;
			ActualDirection = 3;
		} else {
			InMovement = false;
		}
		transform.LookAt(new Vector3 (Directions[ActualDirection].position.x,transform.position.y, Directions[ActualDirection].position.z));

		//Define se o Player esta em movimento ou não.
		if (InMovement)
			gameObject.GetComponent<Rigidbody> ().velocity = transform.forward * Speed;
		else
			gameObject.GetComponent<Rigidbody> ().velocity = transform.forward * 0;
		
	}
}
