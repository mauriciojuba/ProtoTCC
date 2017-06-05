﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCollider : MonoBehaviour {

	public float Damage;

	void OnTriggerEnter (Collider col){
		//aplicar a função de causar dano.
		if (col.CompareTag ("Player1_3D") || col.CompareTag ("Player2_3D") ||
		    col.CompareTag ("Player3_3D") || col.CompareTag ("Player4_3D")) 
		{
			col.GetComponent<Movimentacao3D> ().SetTakeDamageAnim ();
		}
		if (col.gameObject.GetComponent<Life> () != null) {
			//Aqui deve ser chamado o método(função) que substituirá o Update do script Life.
			col.gameObject.GetComponent<Life> ().LifeQuant -= (int)Damage;
			col.gameObject.GetComponent<Life> ().UpdateLife ();
			if (col.gameObject.GetComponent<Life> ().LifeOF == Life.LifeType.Player) {
				col.gameObject.GetComponent<Life> ().ListOfImg [col.gameObject.GetComponent<Life> ().QuantImgInScene - 1].GetComponent<ScaleLife> ().TatuLife -= (int)Damage;
				col.gameObject.GetComponent<Life> ().ListOfImg [col.gameObject.GetComponent<Life> ().QuantImgInScene - 1].GetComponent<ScaleLife> ().UpdateScaleLife ();
			}
		} else {
			Debug.LogWarning ("Deal " + Damage + " Damage");
		}
		if (col.gameObject.GetComponent<FSMMosquito> () != null) {
			col.gameObject.GetComponent<FSMMosquito> ().state = FSMMosquito.FSMStates.Damage;
			col.gameObject.GetComponent<FSMMosquito> ().SetTakeDamageAnim ();
		}
		GetComponent<BoxCollider> ().enabled = false;
	}
}
