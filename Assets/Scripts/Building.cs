using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Building : MonoBehaviour, IDamageable {

    public bool selected = false;
    public BuildingData buildingData;
    public GameObject selectedRectangle;
    public Canvas healthCanvas;
    public Image healthBar;

    private Ray ray;
    private RaycastHit hit;
    private HUDController HUD;
    private SelectionController selectionController;
    private string currentBuilding;
    private bool barracksSetted = false;
    private bool aux = false;
    private float health;
    private BuildingSFX sfx;
    private Quaternion healthBarRotation;
    private bool damaged = false;
    private RadiusRange radiusRange;


    private void Start() {

        healthBarRotation = healthBar.transform.rotation;

        currentBuilding = buildingData.buildingName;

        //Getting HUD
        GameObject userInterface = GameObject.FindGameObjectWithTag("UserInterface");
        if(userInterface != null) {

            HUD = userInterface.GetComponent<HUDController>();
        }

        health = buildingData.health;

        sfx = GetComponent<BuildingSFX>();

        if (currentBuilding == "ArrowTower" || currentBuilding == "Sawmill" || currentBuilding == "StoneWarehouse" || currentBuilding == "DragonTower") {

            radiusRange = GetComponentInChildren<RadiusRange>();
        }
    }

    void Update() {

        if (health < buildingData.health) {

            damaged = true;
            healthCanvas.enabled = true;
        }

        healthCanvas.transform.rotation = healthBarRotation;

        //Select on click
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!EventSystem.current.IsPointerOverGameObject()) {

            if (Physics.Raycast(ray, out hit)) {

                if (Input.GetMouseButtonDown(0) && hit.transform.position == this.transform.position) {

                    selected = true;
                } else if (Input.GetMouseButtonDown(0) && hit.transform.position != this.transform.position) {

                    selected = false;
                }
            }
        }

        if (selected) {

            if (currentBuilding == "ArrowTower" || currentBuilding == "Sawmill" || currentBuilding == "StoneWarehouse" || currentBuilding == "DragonTower") {

                radiusRange.SetShowLine(true);
            }

            buildingActions();
            selectedRectangle.SetActive(true);
            healthCanvas.enabled = true;
            aux = false;

        } else {

            if (currentBuilding == "ArrowTower" || currentBuilding == "Sawmill" || currentBuilding == "StoneWarehouse" || currentBuilding == "DragonTower") {

                radiusRange.SetShowLine(false);
            }

            selectedRectangle.SetActive(false);
            if (currentBuilding == "Barracks") {

                if (!aux) {

                    HUD.RemoveCurrentBarracks();
                    barracksSetted = false;
                    HUD.HideUnitsMenu();
                    aux = true;
                }           
            }

            if (!damaged) {

                healthCanvas.enabled = false;
            }
        }
    }

    private void buildingActions() {

        if (currentBuilding == "Barracks") {

            HUD.ShowUnitsMenu();

            if (!barracksSetted) {

                HUD.SetCurrentBarracks(this.gameObject.GetComponent<UnitsCreator>());
                barracksSetted = true;
            }
        }
    }

    //Take Damage (IDamageable)
    public void TakeDamage(int damage) {

        health -= damage;
        healthBar.fillAmount = health / buildingData.health;

        if (health <= 0) {

            if (currentBuilding == "TownHall") {

                LoseGame();
            }

            sfx.PlayReceiveDamageSound();
            Destroy();
        }
    }

    //When destroyed
    public void Destroy() {

        Destroy(gameObject);
    }

    public float GetHealth() {

        return health;
    }

    private void LoseGame() {

        Time.timeScale = 0;
        HUD.ShowDefeatText();
    }
}