﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour
{

    //dano minimo e maximo, se caso formos fazer um pequeno random.. caso n for, só deletar um.
    public float MinDamage;
    public float MaxDamage;

    //dano minimo e maximo do ataque forte.
    public float MinStrongDamage;
    public float MaxStrongDamage;

    [SerializeField] private Collider DamageCollider;


	[SerializeField] private Movimentacao3D PlayerNumberRef;
	[SerializeField] private string CharacterName;

    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
		if (CharacterName == "Liz") {
			if (PlayerNumberRef.onScreen) {
				DamageCollider.GetComponent<BoxCollider> ().size = new Vector3 (1, 2, 1);
			} else {
				DamageCollider.GetComponent<BoxCollider> ().size = new Vector3 (1, 0.5f, 0.5f);
			}
		}
    }

	public void ActiveCollider(){
		DamageCollider.enabled = true;
		DamageCollider.gameObject.GetComponent<FightCollider>().Damage = Random.Range(MinDamage, MaxDamage);
	}

	public void DesactiveCollider(){
		DamageCollider.enabled = false;
	}
}
