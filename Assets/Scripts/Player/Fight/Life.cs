using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour {

	public enum LifeType {Player, Enemy, Boss};
	public LifeType LifeOF;
	private int PlayerNumber;
	public float LifeQuant;

	private Text LifeText;
	void Awake () {
		PlayerNumber = GetComponent<Movimentacao3D> ().PlayerNumber;
		GameObject.Find ("UI").transform.FindChild ("LifeP" + PlayerNumber).gameObject.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		switch (LifeOF) {
		case LifeType.Player:
			PlayerLife ();
			break;
		case LifeType.Enemy:
			EnemyLife();
			break;
		case LifeType.Boss:
			BossLife();
			break;
		}
	}

	void PlayerLife(){
		LifeText = GameObject.Find ("UI").transform.FindChild ("LifeP" + PlayerNumber).gameObject.GetComponent<Text> ();
		LifeText.text = "Life P" + PlayerNumber + ": " + (int)LifeQuant;
	}

	void EnemyLife(){
	
	}

	void BossLife(){
	
	}
}
