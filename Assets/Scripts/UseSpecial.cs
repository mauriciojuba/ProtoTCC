using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSpecial : MonoBehaviour {

	public SpecialSkill Special;
	public int PlayerNumber;
	public bool Use;
	Vector3 InitPos;
	public Rigidbody RB;

	void Start(){
		PlayerNumber = GetComponent<Movimentacao3D> ().PlayerNumber;
		RB = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown (Special.ButtonToUse + PlayerNumber) && !Use) {
			Use = true;
			InitPos = transform.position;
			gameObject.GetComponent<Movimentacao3D> ().CanMove = false;
			Vector3 v3 = RB.velocity;
			v3.x = 0;
			v3.z = 0;
			RB.velocity = v3;
		}

		if (Use)
			UseTheSpecial ();
	}


	public void UseTheSpecial(){
		if (Special.Rush) {

			RB.AddForce (transform.forward * Special.RushForce);
			if (Vector3.Distance (transform.position, InitPos) > Special.RushDistance) {
				RB.velocity = Vector3.zero;
				gameObject.GetComponent<Movimentacao3D> ().CanMove = true;
				Use = false;
			}
		}
	}

	void OnCollisionEnter(Collision Col){
		if (Special.Rush && Use) {
			if (Col.collider.CompareTag ("Enemy")) {
				Rigidbody RBE;
				RBE = Col.collider.gameObject.GetComponent<Rigidbody> ();
				if (RBE.mass < 10)
					RBE.AddExplosionForce (Special.RushPushForce, Col.collider.transform.position, 10.0f, 3.0f);
				//aplicar o dano no collider.
				else {
					RB.velocity = Vector3.zero;
					gameObject.GetComponent<Movimentacao3D> ().CanMove = true;
					Use = false;
				}
			}
		}
	}
}
