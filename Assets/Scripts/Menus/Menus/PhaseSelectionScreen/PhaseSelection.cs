using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PhaseSelection : MonoBehaviour {

	[SerializeField] private List<Button> Phases;

	private Data DataS;

	void Start () {
		DataS = GameObject.FindWithTag ("DATA").GetComponent<Data> ();
		for (int i = 1; i < Phases.Count; i++) {
			if (DataS.CompletedPhases.Contains (i)) {
				Phases [i].interactable = true;
			} else {
				Phases [i].interactable = false;
			}
		}
	}
	
	public void PhaseSelected(string PhaseName){
		DataS.PhaseName = PhaseName;
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Selecao Personagens");
	}
}
