using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeScene : MonoBehaviour {

	public int Index;

	public void ChangeScene(int SceneIndex){
		SceneManager.LoadScene(SceneIndex);
	}

	void OnTriggerEnter(Collider col){
		ChangeScene (Index);
	}
}
