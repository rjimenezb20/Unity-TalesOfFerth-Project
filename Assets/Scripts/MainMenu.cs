using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    private void Awake() {

        Time.timeScale = 1;
    }

    public void GoToMap1 () {

        SceneManager.LoadScene(1);
    }

    public void QuitGame() {

        Application.Quit();
    }

}
