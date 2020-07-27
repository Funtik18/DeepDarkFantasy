using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    public Button buttonContinueGame;
    public Button buttonNewGame;
    public Button buttonSaveGame;
    public Button buttonLoadGame;
    public Button buttonPreferences;
    public Button buttonCredits;
    public Button buttonExit;

    int state = 0;

	private void Awake() {
        buttonContinueGame.onClick.AddListener(delegate { });
        buttonNewGame.onClick.AddListener(delegate { NewGame(); });
        buttonSaveGame.onClick.AddListener(delegate { });
        buttonLoadGame.onClick.AddListener(delegate { });
        buttonPreferences.onClick.AddListener(delegate { });
        buttonCredits.onClick.AddListener(delegate { });
        buttonExit.onClick.AddListener(delegate { Exit(); });
    }


    private void SetState() {
		switch (state) {
            case 0: {
                buttonContinueGame.gameObject.SetActive(false);
                buttonNewGame.gameObject.SetActive(true);
                buttonSaveGame.gameObject.SetActive(false);
                buttonLoadGame.gameObject.SetActive(true);
                buttonPreferences.gameObject.SetActive(true);
                buttonCredits.gameObject.SetActive(false);
                buttonExit.gameObject.SetActive(true);
            }
            break;
            default: {

			}
            break;
		}
	}
    private void NewGame() {
        SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
	}
    private void Exit() {
        Application.Quit();
	}
}
