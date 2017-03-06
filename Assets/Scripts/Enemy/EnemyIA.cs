using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIA : MonoBehaviour {

	#region Privates Variables
	private Rigidbody RB;
	//Direções que o inimigo pode escolher.
	private bool Left,Front,Right,Back;
	//tempo de ataque e troca de direção.
	private float timer,CDAtk;
	//Distancia que ele começa a rondar.
	public float DistX;
	#endregion

	[Header("Ativa/Desativa o script")]
	public bool Active;

	//Estados do inimigo.
	public enum State {Chase, Round, Attack};
	[Header("Estado Atual")]
	public State MyActualState = State.Chase;
	[Header("Foco do inimigo")]
	public Transform Target;

	[Header("Tempo para trocar de direção")]
	[Range(1,8)]
	public float timerMax;

	[Header("Tempo de recarga do Ataque")]
	[Range(8,30)]
	public float AttackCD;

	[Header("Distancia minima para Ronda")]
	[Range(5,20)]
	public float MinDist;

	[Header("Distancia Maxima para Ronta")]
	[Range(10,30)]
	public float MaxDist;

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
		SelectSide ();
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
		

	#region Estado de Chase
	void ChaseState(){
		//randomiza a distancia que ele vai começar a rondar.
		DistX = Random.Range (MinDist, MaxDist);
		//mantem o inimigo olhando pro player
		var rotation = Quaternion.LookRotation (Target.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * RotateSpeed);

		//calcula se o inimigo esta dentro da distacia para ronda ou não.
		if (Vector3.Distance (transform.position, Target.position) < DistX) {
			MyActualState = State.Round;
		} else {
			RB.velocity = transform.forward * Speed;
		}
	}
	#endregion

	#region Estado de Ronda
	void RoundState(){
		//mantem o inimigo olhando pro player
		var rotation = Quaternion.LookRotation (Target.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * RotateSpeed);
		//Tempo de ataque.
		CDAtk -= Time.deltaTime;

		//verifica a distancia do jogador.
		if (Vector3.Distance (transform.position, Target.position) < DistX + 10 && CDAtk > 0) {
			timer += Time.deltaTime;
			if (timer > timerMax) {
				//seleciona uma direção depois de certo tempo.
				SelectSide ();
				timer = 0;
			}
			//verifica se o jogador esta muito proximo e se afasta, se caso o tempo de ataque nao estiver zerado.
			if (Vector3.Distance (transform.position, Target.position) < AtkDist && CDAtk > 0) {
				Front = false;
				Back = true;
			}
			#region Movimentação nas direções.
			if (Left && Front) {
				RB.velocity = (-transform.right + transform.forward) * Random.Range(0,RoundSpeed);
			} else if (Right && Front) {
				RB.velocity = (transform.right + transform.forward) * Random.Range(0,RoundSpeed);
			} else if (Left && Back) {
				RB.velocity = (-transform.right + (-transform.forward)) * Random.Range(0,RoundSpeed);
			} else if (Right && Back) {
				RB.velocity = (transform.right + (-transform.forward)) * Random.Range(0,RoundSpeed);
			} else if (Right) {
				RB.velocity = transform.right * Random.Range(0,RoundSpeed);
			} else if (Left) {
				RB.velocity = -transform.right * Random.Range(0,RoundSpeed);
			}
			#endregion
	
		}else if (Vector3.Distance (transform.position, Target.position) > AtkDist && CDAtk <= 0) {
			RB.velocity = transform.forward * Speed;
		}else if (Vector3.Distance (transform.position, Target.position) > DistX + 10)
			MyActualState = State.Chase;

		if (Vector3.Distance (transform.position, Target.position) < AtkDist && CDAtk <= 0) {
			MyActualState = State.Attack;
		}
	}
	#endregion
	#region Estado de Ataque.
	//Decidir o ataque.
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
	#endregion

	#region Seleção de direção
	//Seleciona Randomicamente uma direção para se mover.
	void SelectSide(){
		float R = Random.Range (0, 12);
		if (R <= 2 && Vector3.Distance (transform.position, Target.position) > AtkDist) {
			Left = true;
			Right = false;
			Front = true;
			Back = false;
		} else if (R <= 4 && Vector3.Distance (transform.position, Target.position) > AtkDist) {
			Left = false;
			Right = true;
			Front = true;
			Back = false;
		} else if (R <= 6 && Vector3.Distance (transform.position, Target.position) < MinDist) {
			Left = true;
			Right = false;
			Front = false;
			Back = true;
		} else if (R <= 8 && Vector3.Distance (transform.position, Target.position) < MinDist) {
			Left = false;
			Right = true;
			Front = false;
			Back = true;
		} else if (R <= 6) {
			Left = false;
			Right = true;
			Front = false;
			Back = false;
		} else if (R <= 12) {
			Left = true;
			Right = false;
			Front = false;
			Back = true;
		} else {
			SelectSide ();
		}
	}
	#endregion
	//Testes Pra seleção de Foco
	/* Transform SelectTarget (Transform P1, Transform P2){
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
