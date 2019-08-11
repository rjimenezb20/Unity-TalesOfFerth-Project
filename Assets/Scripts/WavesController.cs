using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class WavesController : MonoBehaviour {

    [Header("Enemies")]
    public List<GameObject> enemies = new List<GameObject>();
    public Transform spawnPoint1;
    public Transform spawnPoint2;

    [Header("Waves Prefab")]
    public GameObject wave1Prefab;
    public GameObject wave2Prefab;
    public GameObject wave3Prefab;
    public GameObject wave4Prefab;

    [Header("Path options")]
    public List<Transform> paths = new List<Transform>();
    public Transform townHall;

    private int aux = 0;
    private float timer;
    private HUDController HUD;
    private int currentWave = 0;
    private float minutes;
    private float seconds;
    private bool waveStarted;
    private int random;
    private int pathNumber;
    private bool wave1;
    private bool wave2;
    private bool wave3;
    private bool wave4;
    private bool wave1Done;
    private bool wave2Done;
    private bool wave3Done;
    private bool wave4Done;
    private GameManager gameManager;

    private void Start() {
        
        //Getting HUD
        GameObject userInterface = GameObject.FindGameObjectWithTag("UserInterface");
        if(userInterface != null) {

            HUD = userInterface.GetComponent<HUDController>();
        }

        GameObject game = GameObject.FindGameObjectWithTag("Manager");
        if (game != null) {

            gameManager = game.GetComponent<GameManager>();
        }
    }

    private void Update() {

        timer += Time.deltaTime / 1;
        seconds = timer;

        if (seconds >= 60) {

            timer = 0;
            seconds = 0;
            minutes++;
        }
        
        if (Mathf.RoundToInt(seconds) < 10) {

            HUD.SetTimer("0" + minutes + ": 0" + Mathf.RoundToInt(seconds));
        } else {

            HUD.SetTimer("0" + minutes + ":" + Mathf.RoundToInt(seconds));
        }
        
        if (minutes >= 2 && seconds >= 30 && !wave1Done) {

            wave1 = true;
            wave1Done = true;
            StartWave();
        }

        if (minutes >= 5 && !wave2Done) {

            wave1 = false;
            wave2 = true;
            wave2Done = true;
            StartWave();
        }

        if (minutes >= 7 && seconds >= 30 && !wave3Done) {

            wave2 = false;
            wave3 = true;
            wave3Done = true;
            StartWave();
        }

        if (minutes >= 10 && !wave4Done) {

            wave3 = false;
            wave4 = true;
            wave4Done = true;
            StartWave();
        }

        if (minutes >= 15 ) {

            gameManager.WinGame();
        }
    }

    public void StartWave() {

        random = Random.Range(1, 3);

        if (random == 1) {

            pathNumber = 0;
            WavesEnemies(spawnPoint1);
        }

        if (random == 2) {

            pathNumber = 1;
            WavesEnemies(spawnPoint2);
        }

        currentWave++;
        HUD.SetWaveNumber(currentWave);
    }

    private void WavesEnemies(Transform spawnPoint) {

        if (wave1) {

            Instantiate(wave1Prefab, spawnPoint.position, Quaternion.identity);
        }

        if (wave2) {

            Instantiate(wave2Prefab, spawnPoint.position, Quaternion.identity);
        }

        if (wave3) {

            Instantiate(wave3Prefab, spawnPoint.position, Quaternion.identity);
        }

        if (wave4) {

            Instantiate(wave4Prefab, spawnPoint.position, Quaternion.identity);
        }
    }

    public Transform SelectPath() {

        return paths[pathNumber];
    }

    public Transform GetLastPoint() {

        return townHall.transform;
    }
}
