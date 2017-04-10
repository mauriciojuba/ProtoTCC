using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectChar : MonoBehaviour {

	public bool CanSelect;
	private bool Selected;
	public GameObject CharacterPreSelected;
	private int PlayerNumber;

	[SerializeField] private DetectJoysticks DetectS;


	void Start () {
		PlayerNumber = GetComponent<OptionsPlayer> ().PlayerNumber;
	}
	
	// Update is called once per frame
	void Update () {
		if (CanSelect) {
			if (Input.GetButtonDown ("X P" + PlayerNumber) && !Selected) {
				CharacterPreSelected.GetComponent<SelectChar3D> ().OnSelectCharacter (PlayerNumber);
				GetComponent<Rigidbody> ().velocity = Vector3.zero;
				GetComponent<OptionsPlayer> ().enabled = false;

					DetectS.QuantSelected++;
				Selected = true;

			}
			if (Selected) {
				if (Input.GetButtonDown ("B P" + PlayerNumber)) {
					CharacterPreSelected.GetComponent<SelectChar3D> ().OnDeselectCharacter (PlayerNumber);
					GetComponent<OptionsPlayer> ().enabled = true;
					Selected = false;
					DetectS.QuantSelected--;
				}
			}
		}

	
	}




}
