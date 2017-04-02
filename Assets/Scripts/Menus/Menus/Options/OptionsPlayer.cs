using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPlayer : MonoBehaviour {

	public bool InStairs;
	public bool UsingStairs;
	public GameObject StairsObj;
	public bool InLever;
	public bool UsingLever;
	//Array de direções que o player pode olhar
	public Transform[] Directions;
	//Direção atual que o player esta olhando
	public int ActualDirection;

	//define a velocidade de movimentação do player
	public float Speed;

	//define se o player esta em movimento ou não.
	public bool InMovement;

	//Define o numero do Player.
	public int PlayerNumber;

	//GO do personagem que sera movimentado.
	public GameObject Player;

	[SerializeField] private float JumpForce;
	[SerializeField] private bool InGround;

	public Camera CameraMain;


	public LayerMask NoIgnoredLayers = -1;
	[SerializeField] private float MaxJump;
	private bool Jumping;
	void Start () {
		CameraMain = Camera.main;
		ActualDirection = 3;
	}

	void FixedUpdate () {
		if (!UsingLever) {
			DirectionDefinition ();
			TestPosition ();
			Jump ();
		}
		if (InStairs) {
			if (Input.GetButtonDown ("X P1")) {
				UsingStairs = true;
			}
		}
		//quando o player estiver na escada, desliga a gravidade e altera apenas a velocidade Y do player.
		if (UsingStairs) {
			GetComponent<Rigidbody> ().useGravity = false;
			if (Input.GetAxis ("Vertical P1") > 0.2f || Input.GetAxis ("Vertical P1") < -0.2f) {
				Vector3 V3 = GetComponent<Rigidbody> ().velocity;
				V3.y = StairsObj.transform.up.y * Speed * Input.GetAxis ("Vertical P1");
				GetComponent<Rigidbody> ().velocity = V3;
			} else {
				Vector3 V3 = GetComponent<Rigidbody> ().velocity;
				V3.y = 0;
				GetComponent<Rigidbody> ().velocity = V3;
			}
		}else{
			GetComponent<Rigidbody>().useGravity = true;
		}
	}


	//Define a direção que o player olha de acordo com os botoes que ele aperta.
	void DirectionDefinition(){
		if (Input.GetAxisRaw ("Vertical P" + PlayerNumber) > 0) {
			InMovement = true;
			//Define a direção para Cima-Esquerda.
			if (Input.GetAxisRaw ("Horizontal P" + PlayerNumber) < 0) {
				ActualDirection = 4;
			} 
			//Define a direção para Cima-Direita.
			else if (Input.GetAxisRaw ("Horizontal P" + PlayerNumber) > 0) {
				ActualDirection = 5;
			} 
			//Define a direção para Cima.
			else {
				ActualDirection = 0;
			}
		} else if (Input.GetAxisRaw ("Vertical P" + PlayerNumber) < 0 && !UsingStairs) {
			InMovement = true;
			//Define a direção para Baixo-Esquerda
			if (Input.GetAxisRaw ("Horizontal P" + PlayerNumber) < 0) {
				ActualDirection = 6;
			} 
			//Define a direção para Baixo-Direita
			else if (Input.GetAxisRaw ("Horizontal P" + PlayerNumber) > 0) {
				ActualDirection = 7;
			} 
			//Define a direção para Baixo.
			else {
				ActualDirection = 1;
			}
		} 
		//Define a direção para Esquerda
		else if (Input.GetAxisRaw ("Horizontal P" + PlayerNumber) < 0) {
			InMovement = true;
			ActualDirection = 2;
		} 
		//Define a direção para Direita
		else if (Input.GetAxisRaw ("Horizontal P" + PlayerNumber) > 0) {
			InMovement = true;
			ActualDirection = 3;
		} else {
			InMovement = false;
		}
		transform.LookAt (new Vector3 (Directions [ActualDirection].position.x, transform.position.y, Directions [ActualDirection].position.z));

		//Define se o Player esta em movimento ou não, alterando apenas a velocidade em x e em z, para nao alterar a gravidade.
		if (InMovement) {
			Vector3 V3 = Player.GetComponent<Rigidbody> ().velocity;
			V3.x = transform.forward.x * Speed;
			V3.z = transform.forward.z * Speed;
			Player.GetComponent<Rigidbody> ().velocity = V3;
		} else if (!InMovement) {
			Vector3 V3 = Player.GetComponent<Rigidbody> ().velocity;
			V3.x = transform.forward.x * 0;
			V3.z = transform.forward.z * 0;
			Player.GetComponent<Rigidbody> ().velocity = V3;
		}

	}

	//função para pulo
	void Jump(){
		//verifica se o player esta encostando no chão
		InGround = Physics.Linecast (Player.transform.position, Player.transform.position - Vector3.up * 1.1f, NoIgnoredLayers);
		Debug.DrawLine (Player.transform.position,Player.transform.position - Vector3.up * 1.1f);

		//se estiver no chao, pula, apertando A no controle.
		if (Input.GetButtonDown ("A P" + PlayerNumber) && InGround) {
			Jumping = true;
			Vector3 V3 = Player.GetComponent<Rigidbody> ().velocity;
			V3.y = JumpForce;
			Player.GetComponent<Rigidbody> ().velocity = V3;
		}
		if(Input.GetButtonUp("A P" + PlayerNumber)){
			Jumping = false;
		}


		if(Jumping){
			Vector3 V3 = Player.GetComponent<Rigidbody> ().velocity;
			V3.y += JumpForce;
			Player.GetComponent<Rigidbody> ().velocity = V3;
		}
		if(Player.GetComponent<Rigidbody> ().velocity.y > MaxJump){
			Jumping = false;
		}
	}

	void TestPosition(){
		//Calcula a distancia entre o personagem e a camera.
		var DistanceZ = (transform.position - CameraMain.transform.position).z;

		//Calcula o ponto maximo de movimentação pra esquerda.
		var Leftborder = CameraMain.ViewportToWorldPoint (new Vector3 (0.01f, 0, DistanceZ)).x;

		//Calcula o ponto maximo de movimentação pra direita.
		var Rightborder = CameraMain.ViewportToWorldPoint (new Vector3 (0.99f, 0, DistanceZ)).x;

		//Calcula o ponto maximo de movimentação pra Baixo.
		var Bottomborder = CameraMain.ViewportToWorldPoint (new Vector3 (0, -1.2f, 0)).y;

		//Calcula o ponto maximo de movimentação pra Cima.
		var Topborder = CameraMain.ViewportToWorldPoint (new Vector3 (0, 1.2f, 0)).y;

		//Mantem o personagem sempre dentro do espaço da camera.
		transform.position = new Vector3 (
			Mathf.Clamp (transform.position.x, Leftborder, Rightborder),
			/*Mathf.Clamp (*/transform.position.y/*, Bottomborder, Topborder)*/,
			Mathf.Clamp (transform.position.z,Bottomborder,Topborder ));
	}
}
