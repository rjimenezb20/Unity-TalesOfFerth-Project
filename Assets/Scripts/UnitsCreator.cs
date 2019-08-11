using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsCreator : MonoBehaviour {

    [Header("BuildingsData")]
    //0.Soldier
    public List<UnitData> unitsData;

    public List<Transform> spawnPoint;

    private ResourcesManager resourcesManager;
    private UnitData currentUnit;
    private int spawnNumber = 0;
    private HUDController HUD;

    private void Start() {
        
        //Getting ResourcesManager
        GameObject manager = GameObject.FindGameObjectWithTag("Manager");
        if(manager != null) {

            resourcesManager = manager.GetComponent<ResourcesManager>();
        }

        //Getting HUD
        GameObject userInterface = GameObject.FindGameObjectWithTag("UserInterface");
        if(userInterface != null) {

            HUD = userInterface.GetComponent<HUDController>();
        }
    }

    public void CreateSoldier() {

        currentUnit = unitsData[0];
        InstanciateUnit();
    }

    private void InstanciateUnit() {

        List<int> totalResources = resourcesManager.getResources();

        if (totalResources[0] >= currentUnit.goldCost && totalResources[1] >= currentUnit.foodCost && totalResources[2] >= currentUnit.woodCost && totalResources[3] >= currentUnit.metalCost) {

            resourcesManager.RemoveUnitCost(currentUnit.goldCost, currentUnit.foodCost, currentUnit.woodCost, currentUnit.metalCost, currentUnit.populationCost);

            Instantiate(currentUnit.unitPrefab, spawnPoint[spawnNumber].position, Quaternion.identity);

            spawnNumber += 1;
            if (spawnNumber >= 6) {

                spawnNumber = 0;
            }
        } else {

            HUD.PlayResourcesAlert();
        }
    }
}
