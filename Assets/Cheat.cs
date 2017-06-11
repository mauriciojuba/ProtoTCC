using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour {

	public FightCollider col;
	public float InitDamage;
	// Update is called once per frame

	void Start(){
		col = this.GetComponentInChildren<FightCollider>();
		InitDamage = col.Damage;
	}
	void Update () {
		if(Input.GetAxisRaw("LT P" + GetComponent<Movimentacao3D>().PlayerNumber) >= 1){
			col.Damage = 500;
		}
		else {
			col.Damage = InitDamage;
		}
	}
}
