using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectChar3D : MonoBehaviour {

	[SerializeField] private Character Char;
	[SerializeField] private Data DataS;
	private GameObject Player;

	void Start () {
		DataS = GameObject.FindGameObjectWithTag ("DATA").GetComponent<Data> ();
	}

	void OnTriggerEnter (Collider col){
		if (col.CompareTag ("Player1") || col.CompareTag ("Player2") ||
		    col.CompareTag ("Player3") || col.CompareTag ("Player4"))
			//////////////////////////////////////////////////////////////
		{
			col.GetComponent<DetectChar> ().CanSelect = true;
			col.GetComponent<DetectChar> ().CharacterPreSelected = gameObject;
		}
	}

	void OnTriggerExit (Collider col){
		if (col.CompareTag ("Player1") || col.CompareTag ("Player2") ||
			col.CompareTag ("Player3") || col.CompareTag ("Player4"))
			//////////////////////////////////////////////////////////////
		{
			col.GetComponent<DetectChar> ().CanSelect = false;
			col.GetComponent<DetectChar> ().CharacterPreSelected = null;
		}
	}


	public void OnSelectCharacter(int PlayerNumb){
		if (PlayerNumb == 1) {
			DataS.P1SelectedCharacter = Char;
			DataS.P1SelectedCharacter.PlayerNumber = PlayerNumb;
		}
		else if(PlayerNumb == 2){
			DataS.P2SelectedCharacter = Char;
			DataS.P2SelectedCharacter.PlayerNumber = PlayerNumb;

		}
		else if (PlayerNumb == 3) {
			DataS.P3SelectedCharacter = Char;
			DataS.P3SelectedCharacter.PlayerNumber = PlayerNumb;
		}
		else if(PlayerNumb == 4){
			DataS.P4SelectedCharacter = Char;
			DataS.P4SelectedCharacter.PlayerNumber = PlayerNumb;

		}
	}

	public void OnDeselectCharacter(int PlayerNumb){
		if (PlayerNumb == 1)
			DataS.P1SelectedCharacter = null;
		else if(PlayerNumb == 2)
			DataS.P2SelectedCharacter = null;
		else if (PlayerNumb == 3)
			DataS.P3SelectedCharacter = null;
		else if(PlayerNumb == 4)
			DataS.P4SelectedCharacter = null;
	}
}
