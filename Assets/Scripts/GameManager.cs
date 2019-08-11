using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject townHall;
    public Texture2D cursorImage;
    public Texture2D attackCursorImage;

    private HUDController HUD;
    private int cases = 0;
    public static GameManager instance;

    private void Awake() {

        if (instance == null) {

            instance = this;

        } else if (instance != this) {

            Destroy(this);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start() {

        //Getting HUD
        GameObject userInterface = GameObject.FindGameObjectWithTag("UserInterface");
        if(userInterface != null) {

            HUD = userInterface.GetComponent<HUDController>();
        }

        ChangeToNormalCursor();
    }

    void Update () {

        if (Input.GetKeyDown("escape")) {

            OpenAndCloseInGameMenu();
        }
	}

    public void WinGame() {

        Time.timeScale = 0;
        HUD.ShowVictoryText();
    }

    public void OpenAndCloseInGameMenu() {

        switch (cases) {

            case 0:
                HUD.ShowInGameMenu();
                Time.timeScale = 0;
                cases = 1;
                break;

            case 1:
                HUD.HideInGameMenu();
                Time.timeScale = 1;
                cases = 0;
                break;
        }
    }

    public void QuitGame() {

        Application.Quit();
    }

    public void GoToMainMenu() {

        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void GoToMap1 () {

        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void ChangeToNormalCursor() {

        Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.Auto);
    }

    public void ChangeToAttackCursor() {

        Cursor.SetCursor(attackCursorImage, Vector2.zero, CursorMode.Auto);
    }

}
