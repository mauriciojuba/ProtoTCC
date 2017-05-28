using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ApplyCombo: MonoBehaviour{
	private Combo combo1 = new Combo(new string[] {"X","X","X"});
	private Movimentacao3D AnimRef;
	public List<int> IndexCombo;
	public bool Attacking;

	void Start(){
		combo1.PlayerNumber = GetComponent<Movimentacao3D> ().PlayerNumber;
		AnimRef = GetComponent<Movimentacao3D> ();
	}

	void Update(){
		if (combo1.CheckCombo ()) {
			if (!IndexCombo.Contains (combo1.CurrentIndex)) {
				IndexCombo.Add (combo1.CurrentIndex);
			}
			AnimRef.SetAttackAnim (IndexCombo [0]);
			Attacking = combo1.Attacking;
		}
		if (IndexCombo.Count > 0) {
			if (AnimRef.Anim.GetCurrentAnimatorStateInfo (1).IsName ("Attack " + IndexCombo [0])) {
				if (IndexCombo.Count > 1) {
					AnimRef.SetAttackAnim (IndexCombo [1]);
					IndexCombo.Remove (IndexCombo [0]);
				} else {
					AnimRef.SetAttackAnim (0);
					IndexCombo.Clear ();
				}
			}

			if (!Attacking) {
				AnimRef.SetAttackAnim (0);
				IndexCombo.Clear ();
			}
		}
		if (Time.time > combo1.TimeLastButtonPressed + combo1.TimeBetweenButtons) {
			Attacking = false;
		}
	}
}



public class Combo{
	public string[] Buttons;
	public int PlayerNumber;
	public int CurrentIndex = 0;
	public bool Attacking;

	public float TimeBetweenButtons = 0.4f;
	public float TimeLastButtonPressed;
	public Combo(string[] b){
		Buttons = b;
	}

	public bool CheckCombo(){
		if (Time.time > TimeLastButtonPressed + TimeBetweenButtons) {
			CurrentIndex = 0;
			Attacking = false;
		}
		if (CurrentIndex < Buttons.Length) {
			if(Buttons[CurrentIndex] == "X" && Input.GetButtonDown("X P" + PlayerNumber) || Buttons[CurrentIndex] == "X" && Input.GetKeyDown(KeyCode.J) ||
			   Buttons[CurrentIndex] == "Y" && Input.GetButtonDown("Y P" + PlayerNumber) || Buttons[CurrentIndex] == "Y" && Input.GetKeyDown(KeyCode.I)){
				TimeLastButtonPressed = Time.time;
				CurrentIndex++;
				Attacking = true;
				return true;
			}
		}
		if (CurrentIndex >= Buttons.Length) {
			CurrentIndex = 0;
		}
		return false;
	}

}
