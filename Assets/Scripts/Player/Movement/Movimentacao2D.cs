using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimentacao2D : MonoBehaviour {

	public float Speed = 5;

	public GameObject Spriter;

	void Start () {
		
	}
	


	void Update () {
		Movement ();
	}

	void Movement() {
		//movimenta o Player pra cima
		if(Input.GetKey(KeyCode.W)){
			transform.Translate(Vector3.up*Time.deltaTime*Speed); 
		}
		//movimenta o Player pra baixo
		if(Input.GetKey(KeyCode.S)){
			transform.Translate(Vector3.down*Time.deltaTime*Speed); 
		}
		//movimenta o Player pra esquerda
		if(Input.GetKey(KeyCode.A)){
			// vira a sprite pro lado esquerdo
			Spriter.GetComponent<SpriteRenderer> ().flipX = true;
			transform.Translate(Vector3.left*Time.deltaTime*Speed); 
		}
		//movimenta o Player pra direita
		if(Input.GetKey(KeyCode.D)){
			// vira a sprite pro lado direito
			Spriter.GetComponent<SpriteRenderer> ().flipX = false;
			transform.Translate(Vector3.right*Time.deltaTime*Speed); 
		}
	}
}
