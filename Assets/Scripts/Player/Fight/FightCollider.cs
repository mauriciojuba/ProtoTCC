using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCollider : MonoBehaviour {

	public float Damage;

	void OnTriggerEnter (Collider col){
		//aplicar a função de causar dano.
		Debug.LogWarning ("Deal " + (int)Damage + " Damage");
        if (col.gameObject.GetComponent<Life>() != null)
        {
            //Aqui deve ser chamado o método(função) que substituirá o Update do script Life.
			col.gameObject.GetComponent<Life> ().LifeQuant -= (int)Damage;
			col.gameObject.GetComponent<Life> ().UpdateLife ();
        }
		GetComponent<BoxCollider> ().enabled = false;
	}
}
