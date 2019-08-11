using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buildings", menuName = "New Building")]
public class BuildingData : ScriptableObject {

    [Header("Building")]
    public GameObject buildingPrefab;
    public string buildingName;
    public int health;

    [Header("Costs")]
    public int goldCost;
    public int foodCost;
    public int woodCost;
    public int metalCost;

    [Header("Benefits")]
    public int goldBenefit;
    public int foodBenefit;
    public int woodBenefit;
    public int metalBenefit;
    public int populationLimit;

    [Header("Towers")]
    public int damage;
    public float timeBetweenAttacks;
    public GameObject proyectilPrefab;
}