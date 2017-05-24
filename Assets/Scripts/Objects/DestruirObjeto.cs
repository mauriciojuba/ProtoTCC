using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestruirObjeto : MonoBehaviour {

	public bool Throwed;

	void OnCollisionEnter(Collision hit){
		if (hit.gameObject.CompareTag ("Roomba")) {
			if (this.GetComponent<Life> () != null) {
				this.GetComponent<Life> ().LifeQuant = 0;
			}
		} else if (Throwed) {
			if (this.GetComponent<Life> () != null) {
				this.GetComponent<Life> ().LifeQuant = 0;
			}
		}
	}
}
