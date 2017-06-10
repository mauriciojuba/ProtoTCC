using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endDemo : MonoBehaviour {

	void OnTriggerEnter(Collider hit){
		if(hit.CompareTag("Player1_3D")){
			UnityEngine.SceneManagement.SceneManager.LoadScene ("Thanks");
		}
	}
}
