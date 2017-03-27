using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour {

	//dano minimo e maximo, se caso formos fazer um pequeno random.. caso n for, só deletar um.
	public float MinDamage;
	public float MaxDamage;

	//dano minimo e maximo do ataque forte.
	public float MinStrongDamage;
	public float MaxStrongDamage;

	[SerializeField] private Collider DamageCollider;


	private Movimentacao3D PlayerNumberRef;

	void Awake () {
		PlayerNumberRef = GetComponent<Movimentacao3D> ();
		DamageCollider.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		//liga o collider de dano, e define um dano randomico.
		if (Input.GetButtonDown ("X P" + PlayerNumberRef.PlayerNumber)) {
			DamageCollider.enabled = true;
			DamageCollider.gameObject.GetComponent<FightCollider> ().Damage = Random.Range (MinDamage, MaxDamage);
		}
		if (Input.GetButtonUp ("X P" + PlayerNumberRef.PlayerNumber)) {
			DamageCollider.enabled = false;
		}

		if (Input.GetButtonDown ("Y P" + PlayerNumberRef.PlayerNumber)) {
			DamageCollider.enabled = true;
			DamageCollider.gameObject.GetComponent<FightCollider> ().Damage = Random.Range (MinStrongDamage, MaxStrongDamage);
		}
		if (Input.GetButtonUp ("Y P" + PlayerNumberRef.PlayerNumber)) {
			DamageCollider.enabled = false;
		}
	}
}
