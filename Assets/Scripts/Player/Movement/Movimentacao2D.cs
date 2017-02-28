using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimentacao2D : MonoBehaviour {

	//Define a velocidade de movimento
	public float Speed = 5;

	//GO do personagem que vai ser movimentado.
	public GameObject Spriter;

	//Define o numero do player.
	public int PlayerNumber;

	public Camera CameraMain;

	private Vector2 Movement;

	private Rigidbody2D rigidbody2D;
	void Start () {
		rigidbody2D = gameObject.GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate () {
		Movementation ();
		TestPosition ();
	}

	void Movementation() {

		//Define a direção no eixo Y
		float InputY = Input.GetAxisRaw("Vertical P" + PlayerNumber);
		//Define a direção no eixo X
		float InputX = Input.GetAxisRaw ("Horizontal P" + PlayerNumber);

		//Define pra qual lado o sprite vai estar virado
		if(InputX > 0)
			Spriter.GetComponent<SpriteRenderer> ().flipX = false;
		else if (InputX < 0)
			Spriter.GetComponent<SpriteRenderer> ().flipX = true;

		//Registra a direção que o personagem vai se mover
		Movement = new Vector2 (InputX * Speed, InputY * Speed);

		//Move o personagem na direção indicada.
		rigidbody2D.velocity = Movement;

	}

	//testa a posição do personagem em relação a camera
	void TestPosition(){
		//Calcula a distancia entre o personagem e a camera.
		var DistanceZ = (transform.position - CameraMain.transform.position).z;

		//Calcula o ponto maximo de movimentação pra esquerda.
		var Leftborder = CameraMain.ViewportToWorldPoint (new Vector3 (0.01f, 0, DistanceZ)).x;

		//Calcula o ponto maximo de movimentação pra direita.
		var Rightborder = CameraMain.ViewportToWorldPoint (new Vector3 (0.99f, 0, DistanceZ)).x;

		//Calcula o ponto maximo de movimentação pra Baixo.
		var Bottomborder = CameraMain.ViewportToWorldPoint (new Vector3 (0, -0.7f, DistanceZ)).y;

		//Calcula o ponto maximo de movimentação pra Cima.
		var Topborder = CameraMain.ViewportToWorldPoint (new Vector3 (0, 1.19f, DistanceZ)).y;

		//Mantem o personagem sempre dentro do espaço da camera.
		transform.position = new Vector3 (
			Mathf.Clamp (transform.position.x, Leftborder, Rightborder),
			Mathf.Clamp (transform.position.y, Bottomborder, Topborder), 
			transform.position.z);

	}
}
