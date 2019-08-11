using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

    [Header("Amount Texts")]
    public Text goldAmount;
    public Text foodAmount;
    public Text woodAmount;
    public Text metalAmount;
    public Text populationAmount;
    public Text populationLimitAmount;

    [Header("Increase Texts")]
    public Text goldIncrease;
    public Text foodIncrease;
    public Text woodIncrease;
    public Text metalIncrease;

    [Header("Menus")]
    public GameObject InGameMenu;
    public GameObject unitMenu;
    public Button soldierBt;

    [Header("Alerts")]
    public GameObject buildAlert;
    public GameObject resourcesAlert;
    public GameObject defeatAlert;
    public GameObject winAlert;

    [Header("Waves & Time")]
    public Text waveNumber;
    public Text timer;

    // Updating texts methods
    public void SetGoldAmount(int amount) {

        if (goldAmount != null) {

            this.goldAmount.text = amount.ToString();
        }
    }

    public void SetFoodAmount(int amount) {

        if (foodAmount != null) {

            foodAmount.text = amount.ToString();
        }
    }

    public void SetWoodAmount(int amount) {

        if (woodAmount != null) {

            woodAmount.text = amount.ToString();
        }
    }

    public void SetMetalAmount(int amount) {

        if (metalAmount != null) {

            metalAmount.text = amount.ToString();
        }
    }

    public void SetPopulationAmount(int amount) {

        if (populationAmount != null) {

            populationAmount.text = amount.ToString();
        }
    }

    public void SetPopulationLimitAmount(int amount) {

        if (populationLimitAmount != null) {

            populationLimitAmount.text = amount.ToString();
        }
    }

    public void SetWaveNumber(int number) {

        if (waveNumber != null) {

            waveNumber.text = number.ToString("D2");
        }
    }

    public void SetTimer(string time) {

        if (timer != null) {

            timer.text = time.ToString();
        }
    }

    // Show and hide increase texts
    public void ShowGoldIncrease(int amount) {

        if (goldIncrease != null) {

            goldIncrease.text = "(+" + amount.ToString() + ")";
            goldIncrease.enabled = true;
        }   
    }

    public void HideGoldIncrease() {

        goldIncrease.enabled = false;
    }

    public void ShowFoodIncrease(int amount) {

         if (foodIncrease != null) {

            foodIncrease.text = "(+" + amount.ToString() + ")";
            foodIncrease.enabled = true;
        }   
    }

    public void HideFoodIncrease() {

        foodIncrease.enabled = false;
    }

    public void ShowWoodIncrease(int amount) {

       if (woodIncrease != null) {

            woodIncrease.text = "(+" + amount.ToString() + ")";
            woodIncrease.enabled = true;
        }   
    }

    public void HideWoodIncrease() {

        woodIncrease.enabled = false;
    }

    public void ShowMetalIncrease(int amount) {

        if (metalIncrease != null) {

            metalIncrease.text = "(+" + amount.ToString() + ")";
            metalIncrease.enabled = true;
        }   
    }

    public void HideMetalIncrease() {

        metalIncrease.enabled = false;
    }


    //Show and hide unitsMenu
    public void ShowUnitsMenu() {

         unitMenu.SetActive(true);
    }

    public void HideUnitsMenu() {

        unitMenu.SetActive(false);
    }


    //Unit Menu
    public void SetCurrentBarracks(UnitsCreator unitCreator) {

        soldierBt.onClick.AddListener(unitCreator.CreateSoldier);   
    }

    public void RemoveCurrentBarracks() {

        soldierBt.onClick.RemoveAllListeners();
    }

    //Alerts
    public void PlayResourcesAlert() {

        resourcesAlert.GetComponents<DOTweenAnimation>()[0].DOPlayForward();
        Invoke("RewindResourcesAlert", 1.5f);
    }

    public void RewindResourcesAlert() {

        resourcesAlert.GetComponents<DOTweenAnimation>()[0].DOPlayBackwards();
    }

    public void PlayBuildAlert() {

        buildAlert.GetComponents<DOTweenAnimation>()[0].DOPlayForward();
        Invoke("RewindBuildAlert", 1.5f);
    }

    public void RewindBuildAlert() {

        buildAlert.GetComponents<DOTweenAnimation>()[0].DOPlayBackwards();
    }

    public void ShowVictoryText() {

        winAlert.SetActive(true); 
    }

    public void ShowDefeatText() {

        defeatAlert.SetActive(true);
    }


    //Menu
    public void ShowInGameMenu() {

        InGameMenu.GetComponent<DOTweenAnimation>().DOPlayForward();
    }

    public void HideInGameMenu() {

        InGameMenu.GetComponent<DOTweenAnimation>().DOPlayBackwards();
    }


    //Info
    public void ShowInfo(GameObject info) {

        info.SetActive(true);
    }

    public void HideInfo(GameObject info) {

        info.SetActive(false);
    }

}
