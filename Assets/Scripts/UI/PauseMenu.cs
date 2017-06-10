using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	public static bool gamePaused;

	public GameObject pauseMenu;
	public void SairDoJogo(){
		Application.Quit();
	}
	public void SelecaoPersonagem(){
		SceneManager.LoadScene(1);
	}
	public void MainMenu(){
		SceneManager.LoadScene(0);
	}
	void Update(){
		if(Input.GetButtonDown("Start P1") || Input.GetButtonDown("Start P2")){
			gamePaused = !gamePaused;
			pauseMenu.SetActive(gamePaused);
		}
		if(Input.GetButtonDown("PS4 Options") || Input.GetButtonDown("PS4 Share")){
			gamePaused = !gamePaused;
			pauseMenu.SetActive(gamePaused);
		}
	}
}
