using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	public GameObject uiMainMenu;
	public GameObject uiMiniGame;
	public GameObject uiEndGame;
	public GameManager gameManager;

	// Use this for initialization
	void Start () {
		uiMainMenu.SetActive(true);
		uiMiniGame.SetActive(false);
		uiEndGame.SetActive(false);
	}

	public void playGame(){
		uiMainMenu.SetActive(false);
		uiMiniGame.SetActive(true);
		uiEndGame.SetActive(false);
		gameManager.activateStageContainer(true);
	}

	public void endGame(){
		uiMiniGame.SetActive(false);
		uiEndGame.SetActive(true);
		uiMainMenu.SetActive(false);
		gameManager.activateStageContainer(false);
	}

	public void mainUI(){
		uiMainMenu.SetActive(true);
		uiMiniGame.SetActive(false);
		uiEndGame.SetActive(false);
		gameManager.activateStageContainer(false);
	}
		
}
