using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCutscene1A : MonoBehaviour {

	public Animator Roomba;
	void OnTriggerEnter(Collider hit){
		if(hit.CompareTag("Player1_3D")){
			Roomba.enabled = true;
		}
	}
}
