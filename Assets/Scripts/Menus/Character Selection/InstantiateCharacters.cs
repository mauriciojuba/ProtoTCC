﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCharacters : MonoBehaviour {

	//variaveis para buscar no codigo de dados.
	public GameObject P1Model,P2Model;
	public Transform P1_Pos,P2_Pos;
	private GameObject P1,P2;
	public Data DataS;

	void Awake() {
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
		//ativa o script de movimentacao 3D para o jogador 1
		P1.transform.FindChild("3D_Player").gameObject.GetComponent<Movimentacao3D> ().PlayerNumber = DataS.P1SelectedCharacter.PlayerNumber;
		P1.transform.FindChild("3D_Player").tag = "Player1_3D";
		//ativa o script de movimentacao 2D para o jogador 1
		P1.transform.FindChild("2D_Player").gameObject.GetComponent<Movimentacao2D> ().PlayerNumber = DataS.P1SelectedCharacter.PlayerNumber;
		P1.transform.FindChild("2D_Player").tag = "Player1_2D";

		if (P2Model != null) {
			//instancia o modelo de personagem do jogador 2.
			P2 = GameObject.Instantiate (P2Model, P2_Pos.transform.position, P2_Pos.transform.rotation);
			//define o nome do personagem
			P2.name = DataS.P2SelectedCharacter.CharacterName;
			//ativa o script de movimentacao 3D para o jogador 1
			P2.transform.FindChild("3D_Player").gameObject.GetComponent<Movimentacao3D> ().PlayerNumber = DataS.P2SelectedCharacter.PlayerNumber;
			P2.transform.FindChild("3D_Player").tag = "Player2_3D";
			//ativa o script de movimentacao 2D para o jogador 1
			P2.transform.FindChild("2D_Player").gameObject.GetComponent<Movimentacao2D> ().PlayerNumber = DataS.P2SelectedCharacter.PlayerNumber;
			P2.transform.FindChild("2D_Player").tag = "Player2_2D";
		}
	}		
}