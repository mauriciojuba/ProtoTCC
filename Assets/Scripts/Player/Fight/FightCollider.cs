using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCollider : MonoBehaviour {

	public float Damage;

	void OnTriggerEnter (Collider col){
		//aplicar a função de causar dano.
		if (col.gameObject.GetComponent<Life> () != null) {
			//Aqui deve ser chamado o método(função) que substituirá o Update do script Life.
			col.gameObject.GetComponent<Life> ().LifeQuant -= (int)Damage;
			col.gameObject.GetComponent<Life> ().UpdateLife ();
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
