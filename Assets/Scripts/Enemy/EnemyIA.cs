using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIA : MonoBehaviour {

	#region Privates Variables
	private Rigidbody RB;
	private bool Left,Front,Right,Back;
	private float timer,CDAtk;
	#endregion

	[Header("Ativa/Desativa o script")]
	public bool Active;

	public enum State {Chase, Round, Attack};
	[Header("Estado Atual")]
	public State MyActualState = State.Chase;
	[Header("Foco do inimigo")]
	public Transform Target;

	[Header("Tempo para trocar de direção")]
	[Range(2,8)]
	public float timerMax;

	[Header("Tempo de recarga do Ataque")]
	[Range(8,30)]
	public float AttackCD;

	[Header("Distancia minima de perseguição")]
	[Range(10,20)]
	public float MinDist;

	[Header("Distancia para ataque")]
	[Range(2,4)]
	public float AtkDist;

	[Header("Velocidade de rotação")]
	[Range(5,10)]
	public float RotateSpeed;

	[Header("Velocidade de movimentação")]
	[Range(5,15)]
	public float Speed;

	[Header("Velocidade de 'Rounding'")]
	[Range(2,5)]
	public float RoundSpeed;

	void Start () {
		RB = gameObject.GetComponent<Rigidbody> ();
		Front = true;
		CDAtk = AttackCD;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Active){
			switch (MyActualState) {
				case State.Chase : 	  ChaseState ();     break;
				case State.Round :    RoundState ();     break;
				case State.Attack:    AttackState ();    break;
			}
		}
	}
		
	void ChaseState(){
		var rotation = Quaternion.LookRotation (Target.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * RotateSpeed);

		if (Vector3.Distance (transform.position, Target.position) > MinDist) {
			RB.velocity = transform.forward * Speed;
		} else {
			MyActualState = State.Round;
		}
			

	}
	void RoundState(){
		var rotation = Quaternion.LookRotation (Target.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * RotateSpeed);
		CDAtk -= Time.deltaTime;
		if (Vector3.Distance (transform.position, Target.position) < MinDist && CDAtk > 0) {
			timer += Time.deltaTime;
			if (timer > timerMax) {
				SelectSide ();
				timer = 0;
			}
			if (Vector3.Distance (transform.position, Target.position) < AtkDist && CDAtk > 0) {
				RB.velocity = -transform.forward * RoundSpeed;
				if (Front) {
					Front = false;
					SelectSide ();
				}

			} else {
				if (Left) {
					RB.velocity = -transform.right * RoundSpeed;
				} else if (Right) {
					RB.velocity = transform.right * RoundSpeed;
				} else if (Front) {
					RB.velocity = transform.forward * RoundSpeed;
				} else if (Back) {
					RB.velocity = -transform.forward * RoundSpeed;
				}
			}
		}else if (Vector3.Distance (transform.position, Target.position) > AtkDist && CDAtk <= 0) {
			RB.velocity = transform.forward * Speed;
		} else if (Vector3.Distance (transform.position, Target.position) > MinDist)
			MyActualState = State.Chase;

		if (Vector3.Distance (transform.position, Target.position) < AtkDist && CDAtk <= 0) {
			MyActualState = State.Attack;
		}
	}
	void AttackState(){
		StartCoroutine (Attack());

	}
	IEnumerator Attack(){
		yield return new WaitForSeconds (1);
		CDAtk = AttackCD;
		yield return new WaitForSeconds (1);
		if (Vector3.Distance (transform.position, Target.position) > MinDist)
			MyActualState = State.Chase;
		else
			MyActualState = State.Round;
	}
	void SelectSide(){
		float R = Random.Range (0, 8);
		if (R <= 2) {
			Left = true;
			Right = false;
			Front = false;
			Back = false;
		} else if (R <= 4) {
			Left = false;
			Right = true;
			Front = false;
			Back = false;
		} else if (R <= 6 && Vector3.Distance (transform.position, Target.position) > AtkDist) {
			Left = false;
			Right = false;
			Front = true;
			Back = false;
		} else if(R <= 8 && Vector3.Distance (transform.position, Target.position) < MinDist) {
			Left = false;
			Right = false;
			Front = false;
			Back = true;
		}
	}

	//Testes Pra seleção de Foco
	/* Transform SelectTarget (Transform P1, Transform P2,Transform P3){
		//Distancia Do Player1 em X e em Z
		float DistP1X = transform.position.x - P1.position.x;
		float DistP1Z = transform.position.z - P1.position.z;
		//Distancia Do Player2 em X e em Z
		float DistP2X = transform.position.x - P2.position.x;
		float DistP2Z = transform.position.z - P2.position.z;

		//Distancia Do Player3 em X e em Z
		float DistP3X = transform.position.x - P3.position.x;
		float DistP3Z = transform.position.z - P3.position.z;


		//Soma as distancias do Player1
		float DistP1 = DistP1X + DistP1Z;
		//Soma as distancias do Player2
		float DistP2 = DistP2X + DistP2Z;
		//Soma as distancias do Player3
		float DistP3 = DistP3X + DistP3Z;

		//Decide qual player esta mais perto para focar.
		if (DistP1 < DistP2 && DistP1 < DistP3) {
			return P1;
		} else if (DistP2 < DistP3) {
			return P2;
		} else
			return P3;

	}*/
}
