using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resurrect : MonoBehaviour {

	[SerializeField] private float Timer;
	[SerializeField] private float TimerMax;

	void Update(){
		if (Timer >= TimerMax) {
			Timer = 0;
			transform.parent.GetComponent<Movimentacao3D> ().CanMove = true;
			transform.parent.GetComponent<Movimentacao3D> ().Stunned = false;
			transform.parent.GetComponent<Life> ().LifeQuant += 299;
			transform.parent.GetComponent<Life> ().UpdateLife ();
		}
	}
	void OnTriggerStay(Collider col){
		if (col.CompareTag ("Player1_3D") || col.CompareTag ("Player2_3D") ||
		   	col.CompareTag ("Player3_3D") || col.CompareTag ("Player4_3D")) 
		{
			if (Input.GetButton ("Left Analog Button P" + col.GetComponent<Movimentacao3D> ().PlayerNumber)) {
				Timer += Time.deltaTime;
			}

			if (Input.GetButtonUp ("Left Analog Button P" + col.GetComponent<Movimentacao3D> ().PlayerNumber)) {
				Timer = 0;
			}
		}
	}
}
