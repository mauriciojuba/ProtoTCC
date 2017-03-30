using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour {

	public enum LifeType {Player, Enemy, Boss};
	public LifeType LifeOF;
	public float LifeQuant;
	public List<GameObject> ListOfImg;

	private int PlayerNumber;
	private GameObject LifeOBJ;
	private GameObject Container;
	private int Division;
	private int QuantImgInScene;
	[SerializeField] private int QuantImg;
	[SerializeField] private GameObject LifeSpritePrefab;

	void Awake () {
		PlayerNumber = GetComponent<Movimentacao3D> ().PlayerNumber;
		LifeOBJ = GameObject.Find ("UI").transform.FindChild ("LifeP" + PlayerNumber).gameObject;
		Container = LifeOBJ.transform.FindChild ("Container").gameObject;
		Division = 30;
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
		LifeOBJ.SetActive (true);
		QuantImg = (int)LifeQuant / Division;
		if (QuantImgInScene < QuantImg) {
				GameObject gb = GameObject.Instantiate (LifeSpritePrefab);
				gb.transform.SetParent (Container.transform);
				QuantImgInScene++;
				ListOfImg.Add (gb);
		} else if (QuantImgInScene > QuantImg) {
			Destroy (ListOfImg [QuantImgInScene - 1]);
			ListOfImg.RemoveAt (QuantImgInScene - 1);
			QuantImgInScene--;
		}

		if (Input.GetButtonDown ("LB P" + PlayerNumber)) {
			LifeQuant -= Random.Range (20, 50);
		}
		if (Input.GetButtonDown ("RB P" + PlayerNumber)) {
			LifeQuant += Random.Range (20, 50);
		}
	}

	void EnemyLife(){
	
	}

	void BossLife(){
	
	}
}
