using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{

    [Header("BuildingsData")]
    //0.House, 1.Barracks, 2.ArrowTower, 3.Windmill, 4.Sawmill, 5.StoneWarehouse, 6.DragonTower
    public List<BuildingData> buildingsData;

    public static BuildManager instance;
    public static bool building = false;

    private HUDController HUD;
    private bool position = false;
    private BuildingData currentBuildingData;
    private ResourcesManager resourcesManager;
    private GameObject currentBuilding;
    private int woodBenefit = 0;
    private int stoneBenefit = 0;

    private RaycastHit hit;
    private Ray ray;
    private AudioSource source;
    private RadiusRange radiusRange;


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
        if (userInterface != null) {

            HUD = userInterface.GetComponent<HUDController>();
        }

        //Getting ResourcesManager
        GameObject manager = GameObject.FindGameObjectWithTag("Manager");
        if (manager != null) {

            resourcesManager = manager.GetComponent<ResourcesManager>();
        }

        source = GetComponent<AudioSource>();
    }

    void Update() {

        if (!EventSystem.current.IsPointerOverGameObject()) {

            if (building) {

                SelectBuildingPlacement();
            }
        }
    }

    //Diferent Building methods
    public void BuildHouse() {

        currentBuildingData = buildingsData[0];
        InstanciateBuilding();
    }

    public void BuildBarracks() {

        currentBuildingData = buildingsData[1];
        InstanciateBuilding();
    }

    public void BuildArrowTower() {

        currentBuildingData = buildingsData[2];
        InstanciateBuilding();
    }

    public void BuildWindmill() {

        currentBuildingData = buildingsData[3];
        InstanciateBuilding();
    }

    public void BuildSawmill() {

        currentBuildingData = buildingsData[4];
        InstanciateBuilding();
    }

    public void BuildStoneWarehouse() {

        currentBuildingData = buildingsData[5];
        InstanciateBuilding();
    }

    public void BuildDragonTower() {

        currentBuildingData = buildingsData[6];
        InstanciateBuilding();
    }

    //Building Instanciate
    public void InstanciateBuilding() {

        List<int> totalResources = resourcesManager.getResources();

        //Check if you have enough resources
        if (totalResources[0] >= currentBuildingData.goldCost && totalResources[1] >= currentBuildingData.foodCost && totalResources[2] >= currentBuildingData.woodCost && totalResources[3] >= currentBuildingData.metalCost) {

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {

                currentBuilding = Instantiate(currentBuildingData.buildingPrefab, hit.point, Quaternion.Euler(0, 0, 0));

                building = true;
            }
        } else {

            HUD.PlayResourcesAlert();
        }
    }

    //Building placement (position and rotation)
    public void SelectBuildingPlacement() {

        //Radius draw
        if (currentBuildingData.buildingName == "ArrowTower" || currentBuildingData.buildingName == "Sawmill" || currentBuildingData.buildingName == "StoneWarehouse" || currentBuildingData.buildingName == "DragonTower") {

            radiusRange = currentBuilding.GetComponentInChildren<RadiusRange>();
        }

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {

            if (hit.transform.gameObject.tag != "Building" && hit.transform.gameObject.tag != "Tree") {

                if (radiusRange != null) {

                    radiusRange.SetShowLine(true);
                }

                if (!position) {

                    currentBuilding.transform.position = hit.point;
                }

                if (currentBuildingData.buildingName == "Sawmill") {

                    woodBenefit = currentBuilding.GetComponentInChildren<Sawmill>().GetWoodBenefit();
                }

                if (currentBuildingData.buildingName == "StoneWarehouse") {

                    stoneBenefit = currentBuilding.GetComponentInChildren<StoneWarehouse>().GetStoneBenefit();
                }

                if (currentBuilding.GetComponentInChildren<BuilderHelper>().GetOnLight()) {

                    //Left click hold to rotate
                    if (Input.GetMouseButton(0)) {

                        position = true;

                        if (Input.GetAxis("Mouse X") > 0f) {

                            currentBuilding.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * 8);
                        } else if (Input.GetAxis("Mouse X") < 0f) {

                            currentBuilding.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * 8);
                        }
                    }

                    //Left click up to finally set building position and rotation
                    if (Input.GetMouseButtonUp(0)) {

                        currentBuilding.GetComponent<Collider>().enabled = true;

                        if (currentBuildingData.buildingName == "ArrowTower") {

                            currentBuilding.GetComponents<Collider>()[0].enabled = true;
                            currentBuilding.GetComponents<Collider>()[1].enabled = true;
                        }

                        if (currentBuildingData.buildingName == "Sawmill") {

                            currentBuilding.GetComponentInChildren<Sawmill>().ChangeBackTreeColor();
                        }

                        if (currentBuildingData.buildingName == "StoneWarehouse") {

                            currentBuilding.GetComponentInChildren<StoneWarehouse>().ChangeBackStoneColor();
                        }

                        if (currentBuildingData.buildingName == "DragonTower") {

                            currentBuilding.GetComponentInChildren<Tower>().enabled = true;
                        }

                        currentBuilding.GetComponentInChildren<Building>().enabled = true;
                        currentBuilding.GetComponentInChildren<FieldOfView>().enabled = true;
                        building = false;
                        position = false;
                        resourcesManager.RemoveBuildingCost(currentBuildingData.goldCost, currentBuildingData.foodCost, currentBuildingData.woodCost, currentBuildingData.metalCost);
                        resourcesManager.AddBuildingBenefits(currentBuildingData.goldBenefit, currentBuildingData.foodBenefit, woodBenefit, stoneBenefit, currentBuildingData.populationLimit);
                        woodBenefit = 0;
                        stoneBenefit = 0;
                        source.Play();
                    }
                } else {

                    if (Input.GetMouseButtonDown(0)) {

                        HUD.PlayBuildAlert();
                    }
                }  
            }

            //Right Click(Cancel construction)
            if (Input.GetMouseButtonDown(1)) {

                Destroy(currentBuilding);
                building = false;
                position = false;

                if (currentBuildingData.buildingName == "Sawmill") {

                    currentBuilding.GetComponentInChildren<Sawmill>().ChangeBackTreeColor();
                }

                if (currentBuildingData.buildingName == "StoneWarehouse") {

                    currentBuilding.GetComponentInChildren<StoneWarehouse>().ChangeBackStoneColor();
                }
            }
        }
    }
}