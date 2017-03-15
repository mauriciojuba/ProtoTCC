using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectJoysticks : MonoBehaviour {
	//Armazena os controles;
	public string[] Joysticks;

	public int ActiveJoy,QuantSelected;

	//Define se o controle esta conectado ou nao.
	public bool P1Connected,P2Connected,P3Connected,P4Connected;

	//Define se o controle esta Ativo, "Apertou A".
	public bool P1Active,P2Active,P3Active,P4Active;


	[Tooltip("Coloque os ScrollRects de seleção de personagens de cada player, no seu devido lugar")]
	[SerializeField] private ScrollRect ScrollP1;
	[Tooltip("Coloque os ScrollRects de seleção de personagens de cada player, no seu devido lugar")]
	[SerializeField] private ScrollRect ScrollP2;
	[Tooltip("Coloque os ScrollRects de seleção de personagens de cada player, no seu devido lugar")]
	[SerializeField] private ScrollRect ScrollP3;
	[Tooltip("Coloque os ScrollRects de seleção de personagens de cada player, no seu devido lugar")]
	[SerializeField] private ScrollRect ScrollP4;
	[Tooltip("Coloque aqui os objetos para indicar 'Aperte A' para ativar o joystick")]
	[SerializeField]private GameObject Press_A_P2,Press_A_P3,Press_A_P4;

	void start(){
		P1Active = true;
	}

	void Update() {
		DetectJoysticksConnected ();
		ActiveJoysticks ();
		DetectActivity ();
	}

	//Ativa os controles quando apertar A.
	void ActiveJoysticks(){
		if (P2Connected) {
			if (!P2Active)
				Press_A_P2.SetActive (true);
			if (Input.GetButtonDown ("A P2")) {
				P2Active = true;
			}
		} else {
			P2Active = false;
			Press_A_P2.SetActive (false);
		}
		

		if (P3Connected) {
			if (!P3Active)
				Press_A_P3.SetActive (true);
			if (Input.GetButtonDown ("A P3")) {
				P3Active = true;
			}
		} else {
			P3Active = false;
			Press_A_P3.SetActive (false);
		}
		
		if (P4Connected) {
			if (!P4Active)
				Press_A_P4.SetActive (true);
			if (Input.GetButtonDown ("A P4")) {
				P4Active = true;
			}
		} else {
			P4Active = false;
			Press_A_P4.SetActive (false);
		}
		
	}

	//detecta atividade do controle.
	void DetectActivity(){
		if (P2Active) {
			Press_A_P2.SetActive (false);
			ScrollP2.content.gameObject.SetActive (true);
			ScrollP2.gameObject.GetComponent<SelectCharacter> ().enabled = true;
		} else {
			ScrollP2.content.gameObject.SetActive (false);
			ScrollP2.gameObject.GetComponent<SelectCharacter> ().enabled = false;
		}
		if (P3Active) {
			Press_A_P3.SetActive (false);
			ScrollP3.content.gameObject.SetActive (true);
			ScrollP3.gameObject.GetComponent<SelectCharacter> ().enabled = true;
		} else {
			ScrollP3.content.gameObject.SetActive(false);
			ScrollP3.gameObject.GetComponent<SelectCharacter>().enabled = false;
		}
		if (P4Active) {
			Press_A_P4.SetActive (false);
			ScrollP4.content.gameObject.SetActive (true);
			ScrollP4.gameObject.GetComponent<SelectCharacter> ().enabled = true;
		} else {
			ScrollP4.content.gameObject.SetActive (false);
			ScrollP4.gameObject.GetComponent<SelectCharacter> ().enabled = false;
		}
			
	}

	//detecta se os joyscticks estao conectados.
	void DetectJoysticksConnected(){
		Joysticks = new string[Input.GetJoystickNames ().Length];
		Joysticks = Input.GetJoystickNames ();
	

		if (Joysticks [0] != "")
			P1Connected = true;
		if (Joysticks.Length > 1 && Joysticks [1] != "")
			P2Connected = true;
		else
			P2Connected = false;
		if (Joysticks.Length > 2 && Joysticks [2] != "")
			P3Connected = true;
		else
			P3Connected = false;
		if (Joysticks.Length > 3 && Joysticks [3] != "")
			P4Connected = true;
		else
			P4Connected = false;
	}
}
