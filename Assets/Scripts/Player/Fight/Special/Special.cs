using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Special : MonoBehaviour {
	[HideInInspector]
	public bool Pull,Push,AoE;
	[HideInInspector]
	public float Damage,PushForce;
	[HideInInspector]
	public Vector3 InitPos;
	[HideInInspector]
	public float Distance;

	[SerializeField] private Transform PosToPull;

	Rigidbody RB;
	void Start(){
		RB = GetComponent<Rigidbody> ();
	}
	void Update(){
		if (Vector3.Distance (transform.position, InitPos) > Distance) {
			RB.velocity = Vector3.zero;
		}
	}

	void OnTriggerStay(Collider Col){
		if (Col.CompareTag ("Enemy") || Col.CompareTag ("Interactable")) {
			if (Pull) {
				PullEnemys (Col.GetComponent<Rigidbody>());
				if (Col.GetComponent<FSMMosquito> () != null) {
					ApplyDamageFSMMosquitoOverTime (Col.GetComponent<FSMMosquito> ());
				}
				if (Col.GetComponent<Life> () != null) {
					ApplyDamageLifeOverTime (Col.GetComponent</*substituir pelo script de life novo*/Life> ());
				}
			} else if (Push) {
				PushEnemys (Col.GetComponent<Rigidbody>());
				if (Col.GetComponent<FSMMosquito> () != null) {
					ApplyDamageFSMMosquitoOverTime (Col.GetComponent<FSMMosquito> ());
				}
				if (Col.GetComponent<Life> () != null) {
					ApplyDamageLifeOverTime (Col.GetComponent</*substituir pelo script de life novo*/Life> ());
				}
			}
		}
	}

	void OnTriggerEnter(Collider Col){
		if(!AoE) {
			if (Col.GetComponent<FSMMosquito> () != null) {
				ApplyDamageFSMMosquito (Col.GetComponent<FSMMosquito> ());
			}
			if (Col.GetComponent<Life> () != null) {
				ApplyDamageLife (Col.GetComponent<Life> ());
			}
			Destroy(gameObject);
		}
	}


	void PullEnemys(Rigidbody Col){
		Col.GetComponent<Rigidbody> ().AddExplosionForce (-PushForce, PosToPull.position, gameObject.GetComponent<SphereCollider> ().radius, 3.0f, ForceMode.VelocityChange);
	}

	void ApplyDamageFSMMosquitoOverTime (FSMMosquito Col){
		Col.state = FSMMosquito.FSMStates.Damage;
		Col.SetTakeDamageAnim ();
		Col.Life -= Time.deltaTime * Damage;
	}

	void ApplyDamageLifeOverTime (/*substituir pelo script de life novo*/ Life Col){
		Col.LifeQuant -= Time.deltaTime * Damage;
	}

	void ApplyDamageFSMMosquito (FSMMosquito Col){
		Col.state = FSMMosquito.FSMStates.Damage;
		Col.SetTakeDamageAnim ();
		Col.Life -= Damage;
	}

	void ApplyDamageLife (/*substituir pelo script de life novo*/ Life Col){
		Col.LifeQuant -= Damage;
	}

	void PushEnemys(Rigidbody Col){
		Col.AddExplosionForce (PushForce, PosToPull.position, gameObject.GetComponent<SphereCollider> ().radius, 3.0f, ForceMode.VelocityChange);
	}
}
