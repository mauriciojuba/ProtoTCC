using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCollider : MonoBehaviour {

	public float Damage;

	void OnTriggerEnter (Collider col){
		//aplicar a função de causar dano.
		Debug.LogWarning ("Deal " + (int)Damage + " Damage");
		GetComponent<BoxCollider> ().enabled = false;
	}
}
