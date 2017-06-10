using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour {

	public FightCollider col;
	// Update is called once per frame

	void Start(){
		col = this.GetComponentInChildren<FightCollider>();
	}
	void Update () {
		if((Input.GetButton("LT P1") || Input.GetButton("LT P2")) || Input.GetButton("PS4 L2")){
			col.Damage = 500;
		}
		else{
			col.Damage = 5;
		}
	}
}
