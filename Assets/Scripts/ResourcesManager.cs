using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour {

    [Header("Starting Resources Settings")]
    public int startingGold = 800;
    public int startingFood = 800;
    public int startingWood = 800;
    public int startingMetal = 800;
    public int startingPopulation = 0;
    public int startingPopulationLimit = 10;

    [Header("Resources Gaining Settings (Time)")]
    public int goldGainingRating = 5;
    public int foodGainingRating = 4;
    public int woodGainingRating = 3;
    public int metalGainingRating = 10;

    private HUDController HUD;

    //Total Resources
    private int goldAmount;
    private int foodAmount;
    private int woodAmount;
    private int metalAmount;
    private int populationAmount;
    private int populationLimitAmount;

    //Increase amount over time
    private int goldIncreaseRating = 11;
    private int foodIncreaseRating = 12;
    private int woodIncreaseRating = 13;
    private int metalIncreaseRating = 14;

    //Resources timers
    private float goldTimer;
    private float foodTimer;
    private float woodTimer;
    private float metalTimer;


    void Start () {

        //Getting the HUD
        GameObject userInterface = GameObject.FindGameObjectWithTag("UserInterface");
        if(userInterface != null) {

            HUD = userInterface.GetComponent<HUDController>();
        }

        //Set starting resources
        HUD.SetGoldAmount(startingGold);
        HUD.SetFoodAmount(startingFood);
        HUD.SetWoodAmount(startingWood);
        HUD.SetMetalAmount(startingMetal);
        HUD.SetPopulationAmount(startingPopulation);
        HUD.SetPopulationLimitAmount(startingPopulationLimit);

        goldAmount = startingGold;
        foodAmount = startingFood;
        woodAmount = startingWood;
        metalAmount = startingMetal;
        populationAmount = startingPopulation;
        populationLimitAmount = startingPopulationLimit;

    }
	
	
	void Update () {

        //Gain resources after specific time
        goldTimer += Time.deltaTime / 1;
        if (goldTimer > goldGainingRating) {

            goldAmount += goldIncreaseRating;
            HUD.SetGoldAmount(goldAmount);
            goldTimer = 0;
        }

        foodTimer += Time.deltaTime / 1;
        if (foodTimer > foodGainingRating) {

            foodAmount += foodIncreaseRating;
            HUD.SetFoodAmount(foodAmount);
            foodTimer = 0;
        }

        woodTimer += Time.deltaTime / 1;
        if (woodTimer > woodGainingRating) {

            woodAmount += woodIncreaseRating;
            HUD.SetWoodAmount(woodAmount);
            woodTimer = 0;
        }

        metalTimer += Time.deltaTime / 1;
        if (metalTimer > metalGainingRating) {

            metalAmount += metalIncreaseRating;
            HUD.SetMetalAmount(metalAmount);
            metalTimer = 0;
        }
	}

    //Resources manage
    public void RemoveBuildingCost(int gold, int food, int wood, int metal) {

        RemoveGold(gold);
        RemoveFood(food);
        RemoveWood(wood);
        RemoveMetal(metal);
    }

    public void AddBuildingBenefits(int gold, int food, int wood, int metal, int populationLimit) {

        AddGoldIcreaseRating(gold);
        AddFoodIcreaseRating(food);
        AddWoodIcreaseRating(wood);
        AddMetalIcreaseRating(metal);
        AddPopulationLimit(populationLimit);
    }

    public void RemoveUnitCost(int gold, int food, int wood, int metal, int population) {

        RemoveGold(gold);
        RemoveFood(food);
        RemoveWood(wood);
        RemoveMetal(metal);
        AddPopulation(population);
    }

    //Add Resources
    public void AddGoldIcreaseRating(int amount) {

        goldIncreaseRating += amount;
    }

    public void AddFoodIcreaseRating(int amount) {

        foodIncreaseRating += amount;
    }

    public void AddWoodIcreaseRating(int amount) {

        woodIncreaseRating += amount;
    }

    public void AddMetalIcreaseRating(int amount) {

        metalIncreaseRating += amount;
    }

    public void AddPopulation(int amount) {

        populationAmount += amount;
        HUD.SetPopulationAmount(populationAmount);
    }

    public void AddPopulationLimit(int amount) {

        populationLimitAmount += amount;
        HUD.SetPopulationLimitAmount(populationLimitAmount);
    }

    //Remove Resources
    public void RemoveGold(int amount) {

        goldAmount -= amount;
        HUD.SetGoldAmount(goldAmount);
    }

    public void RemoveFood(int amount) {

        foodAmount -= amount;
        HUD.SetFoodAmount(foodAmount);
    }

    public void RemoveWood(int amount) {

        woodAmount -= amount;
        HUD.SetWoodAmount(woodAmount);
    }

    public void RemoveMetal(int amount) {

        metalAmount -= amount;
        HUD.SetMetalAmount(metalAmount);
    }

    public void RemovePopulation(int amount) {

        populationAmount -= amount;
        HUD.SetPopulationAmount(populationAmount);
    }

    public void RemovePopulationLimit(int amount) {

        populationLimitAmount -= amount;
        HUD.SetPopulationLimitAmount(populationLimitAmount);
    }


    //HUD Manage
    public void ShowGoldIncrease() {

        HUD.ShowGoldIncrease(goldIncreaseRating);
    }

    public void ShowFoodIcrease() {

        HUD.ShowFoodIncrease(foodIncreaseRating);
    }

    public void ShowWoodIncrease() {

        HUD.ShowWoodIncrease(woodIncreaseRating);
    }

    public void ShowMetalIncrease() {

        HUD.ShowMetalIncrease(metalIncreaseRating);
    }

    //Give array with totalResources
    public List<int> getResources() {

        List<int> resourcesList = new List<int> { goldAmount, foodAmount, woodAmount, metalAmount, populationAmount, populationLimitAmount };
        
        return resourcesList;
    }
}
