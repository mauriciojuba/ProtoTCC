using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCharacters : MonoBehaviour {

	public GameObject P1Model,P2Model;
	public Transform P1_Pos,P2_Pos;
	private GameObject P1,P2;
	public Data DataS;

	void Awake() {
		DataS = GameObject.FindWithTag ("DATA").GetComponent<Data> ();

		if (DataS.P1SelectedCharacter != null)
			P1Model = DataS.P1SelectedCharacter.CharacterModel;
		if (DataS.P2SelectedCharacter != null)
			P2Model = DataS.P2SelectedCharacter.CharacterModel;



		P1 = GameObject.Instantiate (P1Model, P1_Pos.transform.position, P1_Pos.transform.rotation);
		P1.name = DataS.P1SelectedCharacter.CharacterName;
		P1.transform.FindChild("3D_Player").gameObject.GetComponent<Movimentacao3D> ().PlayerNumber = DataS.P1SelectedCharacter.PlayerNumber;
		P1.transform.FindChild("3D_Player").tag = "Player1_3D";
		P1.transform.FindChild("2D_Player").gameObject.GetComponent<Movimentacao2D> ().PlayerNumber = DataS.P1SelectedCharacter.PlayerNumber;
		P1.transform.FindChild("2D_Player").tag = "Player1_2D";

		if (P2Model != null) {
			P2 = GameObject.Instantiate (P2Model, P2_Pos.transform.position, P2_Pos.transform.rotation);
			P2.name = DataS.P2SelectedCharacter.CharacterName;
			P2.transform.FindChild("3D_Player").gameObject.GetComponent<Movimentacao3D> ().PlayerNumber = DataS.P2SelectedCharacter.PlayerNumber;
			P2.transform.FindChild("3D_Player").tag = "Player2_3D";
			P2.transform.FindChild("2D_Player").gameObject.GetComponent<Movimentacao2D> ().PlayerNumber = DataS.P2SelectedCharacter.PlayerNumber;
			P2.transform.FindChild("2D_Player").tag = "Player2_2D";
		}
	}		
}
