﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCollider : MonoBehaviour {

	public float Damage;
    public GameObject particula;
	public string CharacterName;
	public GameObject Player;
	public GameObject EnemyHit;


	void OnTriggerEnter (Collider col){
		//aplicar a função de causar dano.
		if (Player != null) {
			if (Player.CompareTag ("Player1_3D") || Player.CompareTag ("Player2_3D") ||
			    Player.CompareTag ("Player3_3D") || Player.CompareTag ("Player4_3D")) {
				if (col.tag != "Player1_3D" && col.tag != "Player2_3D" && col.tag != "Player3_3D" && col.tag != "Player4_3D") {
					if (col.gameObject.GetComponent<Life> () != null) {
						//Aqui deve ser chamado o método(função) que substituirá o Update do script Life.
						EnemyHit = col.gameObject;
						//Aplica o Dano.
						ApplyDamage ();

						//Som de Ataque
                       
						//coloca particula de Ataque
						InstantiateParticle();

						//ativa a animação do inimigo
						ApplyEnemyHitAnim ();


					}
				}
			}
		} else {

			//coloca particula de Ataque
			InstantiateParticle ();

			if (col.gameObject.GetComponent<Life> () != null) {
				EnemyHit = col.gameObject;
			
				//Aplica o Dano.
				ApplyDamage ();

				if (col.gameObject.GetComponent<Life> ().LifeQuant >= col.gameObject.GetComponent<Life> ().Division) {
					if (col.CompareTag ("Player1_3D") || col.CompareTag ("Player2_3D") ||
							col.CompareTag ("Player3_3D") || col.CompareTag ("Player4_3D")) {
							col.GetComponent<Movimentacao3D> ().SetTakeDamageAnim ();
						}
					}
			}
            if(GetComponent<BoxCollider>() != null)
			GetComponent<BoxCollider> ().enabled = false;
		}
    }

	void ApplyDamage(){
		EnemyHit.GetComponent<Life> ().LifeQuant -= (int)Damage;
		EnemyHit.GetComponent<Life> ().UpdateLife ();
		if (EnemyHit.GetComponent<Life> ().LifeOF == Life.LifeType.Player) {
			EnemyHit.GetComponent<Life> ().ListOfImg [EnemyHit.gameObject.GetComponent<Life> ().QuantImgInScene - 1].GetComponent<ScaleLife> ().TatuLife -= (int)Damage;
			EnemyHit.GetComponent<Life> ().ListOfImg [EnemyHit.gameObject.GetComponent<Life> ().QuantImgInScene - 1].GetComponent<ScaleLife> ().UpdateScaleLife ();
		}
	}

	void ApplyEnemyHitAnim(){
		if (EnemyHit.GetComponent<FSMMosquito> () != null) {
			EnemyHit.GetComponent<FSMMosquito> ().state = FSMMosquito.FSMStates.Damage;
			EnemyHit.GetComponent<FSMMosquito> ().SetTakeDamageAnim ();
		}
		if (EnemyHit.GetComponent<FSMAranha> () != null) {
			EnemyHit.GetComponent<FSMAranha> ().state = FSMAranha.FSMStates.Damage;
			EnemyHit.GetComponent<FSMAranha> ().SetTakeDamageAnim ();
		}
	}

	

	public void InstantiateParticle(){
		if (particula != null)
		{
			ScreenShake.Instance.Shake(0.05f, 0.05f);
			GameObject part = Instantiate (particula, transform.position, Quaternion.identity) as GameObject;
			float timePart = part.GetComponent<ParticleSystem> ().duration;
			if (Player != null) {
				if (Player.GetComponent<Movimentacao3D> ().onScreen) {
					part.transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);
				}
			}
			GameObject.Destroy (part, timePart);
		}
	}

}
