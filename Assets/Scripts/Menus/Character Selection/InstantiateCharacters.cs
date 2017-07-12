using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCharacters : MonoBehaviour {

	//variaveis para buscar no codigo de dados.
	public GameObject P1Model,P2Model;
	public Transform P1_Pos,P2_Pos;
	private GameObject P1,P2;
	private GameObject P1_3d_Player, P2_3d_Player;
	public Data DataS;

	void Awake() {
		if (GameObject.FindWithTag ("DATA") != null) {
			DataS = GameObject.FindWithTag ("DATA").GetComponent<Data> ();
	
			//pega o modelo de personagem que o jogador 1 escolheu.
			if (DataS.P1SelectedCharacter != null)
				P1Model = DataS.P1SelectedCharacter.CharacterModel;
			//pega o modelo de personagem que o jogador 2 escolheu.
			if (DataS.P2SelectedCharacter != null)
				P2Model = DataS.P2SelectedCharacter.CharacterModel;


			//instancia o modelo de personagem do jogador 1.
			P1 = GameObject.Instantiate (P1Model, P1_Pos.transform.position, P1_Pos.transform.rotation);
			//define o nome do personagem
			P1.name = DataS.P1SelectedCharacter.CharacterName;
			//Variavel criada para guardar o child 3d_Player.
			P1_3d_Player = P1.transform.Find ("3D_Player").gameObject;
			//ativa o script de movimentacao 3D para o jogador 1
			ApplyP1Components();

			if (P2Model != null) {
				//instancia o modelo de personagem do jogador 2.
				P2 = GameObject.Instantiate (P2Model, P2_Pos.transform.position, P2_Pos.transform.rotation);
				//define o nome do personagem
				P2.name = DataS.P2SelectedCharacter.CharacterName;
				//Variavel criada para guardar o child 3d_Player.
				P2_3d_Player = P2.transform.Find ("3D_Player").gameObject;
				//ativa o script de movimentacao 3D para o jogador 1
				ApplyP2Components();
			}
		}
	}


	void ApplyP1Components(){
		P1_3d_Player.GetComponent<Movimentacao3D> ().PlayerNumber = DataS.P1SelectedCharacter.PlayerNumber;
		P1_3d_Player.GetComponent<Life> ().LifeQuant = DataS.P1SelectedCharacter.Life;
		P1_3d_Player.GetComponent<Life> ().LifeOF = Life.LifeType.Player;
		P1_3d_Player.GetComponent<UseSpecial> ().SpecialRef = DataS.P1SelectedCharacter.Special;
		P1_3d_Player.GetComponent<Life> ().LifeSpritePrefab = DataS.P1Life;
		P1_3d_Player.tag = "Player1_3D";
	}

	void ApplyP2Components(){
		P2_3d_Player.GetComponent<Movimentacao3D> ().PlayerNumber = DataS.P2SelectedCharacter.PlayerNumber;
		P2_3d_Player.GetComponent<Life> ().LifeQuant = DataS.P2SelectedCharacter.Life;
		P2_3d_Player.GetComponent<Life> ().LifeOF = Life.LifeType.Player;
		P2_3d_Player.GetComponent<UseSpecial> ().SpecialRef = DataS.P2SelectedCharacter.Special;
		P2_3d_Player.GetComponent<Life> ().LifeSpritePrefab = DataS.P2Life;
		P2_3d_Player.tag = "Player2_3D";			
	}
}
