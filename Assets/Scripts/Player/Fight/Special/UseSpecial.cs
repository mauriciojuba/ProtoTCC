using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSpecial : MonoBehaviour {

	public SpecialSkill SpecialRef;
	public int PlayerNumber;
	public bool Use;
	Vector3 InitPos;
	public Rigidbody RB;

	public float SpecialBar;
	public int SpecialItens;

	void Start(){
		PlayerNumber = GetComponent<Movimentacao3D> ().PlayerNumber;
		RB = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () {
		if (SpecialRef == null) {
			return;
		}
		if (Input.GetButtonDown (SpecialRef.ButtonToUse + PlayerNumber) && !Use && SpecialItens > 0) {
			if (SpecialRef.Rush) {
				Use = true;
				InitPos = transform.position;
				gameObject.GetComponent<Movimentacao3D> ().CanMove = false;
			} else {
				UseTheSpecial ();
			}
			SpecialItens--;
			Vector3 v3 = RB.velocity;
			v3.x = 0;
			v3.z = 0;
			RB.velocity = v3;
		}

		if (Use)
			UseTheSpecial ();
	}

	public void UpdateBar(){
		if (SpecialBar >= 100) {
			SpecialBar -= 100;
			SpecialItens++;
		}
	}

	public void UseTheSpecial(){
		if (SpecialRef.Rush) {
			RB.AddForce (transform.forward * SpecialRef.RushForce);
			if (Vector3.Distance (transform.position, InitPos) > SpecialRef.RushDistance) {
				RB.velocity = Vector3.zero;
				gameObject.GetComponent<Movimentacao3D> ().CanMove = true;
				Use = false;
			}
		} else if (SpecialRef.Magic) {
			switch (SpecialRef.MyStyle) {
			case SpecialSkill.MagicStyle.AOE:
				GameObject obj = GameObject.Instantiate (SpecialRef.MagicPrefab, transform.position, transform.rotation) as GameObject;
				Special SP = obj.GetComponent<Special> ();
				obj.GetComponent<Rigidbody> ().AddForce (obj.transform.forward * SpecialRef.AOEForce);
				SP.Damage = SpecialRef.AOEDamage;
				SP.Distance = SpecialRef.AOEDistance;
				SP.PushForce = SpecialRef.AOEPushForce;
				SP.Pull = SpecialRef.Pull;
				SP.Push = SpecialRef.Push;
				SP.InitPos = obj.transform.position;
				gameObject.GetComponent<Movimentacao3D> ().CanMove = true;
				break;
			}
		}
	}

	void OnCollisionEnter(Collision Col){
		if (SpecialRef.Rush && Use) {
			foreach (ContactPoint contact in Col.contacts) {
				if (Col.collider.CompareTag ("Enemy")) {
					Rigidbody RBE;
					RBE = Col.collider.gameObject.GetComponent<Rigidbody> ();
					if (RBE.mass < RB.mass) {
						RBE.AddExplosionForce (SpecialRef.RushPushForce, Col.collider.transform.position, 10.0f, 3.0f);
						//aplicar o dano no collider.
					} else {
						RB.velocity = Vector3.zero;
						gameObject.GetComponent<Movimentacao3D> ().CanMove = true;
						Use = false;
					}
				}
			}
		}
	}
}
