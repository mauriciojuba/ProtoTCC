using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InDialogue : MonoBehaviour {

	//script apenas para definir se o dialogo esta ativo ou não.
	[SerializeField] private GameObject NPCDialogueBox;

	public void ActiveDialogue (bool DialogueActive) {
		NPCDialogueBox.SetActive (DialogueActive);
	}
}
