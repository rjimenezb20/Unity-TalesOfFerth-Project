using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Units", menuName = "New Unit")]
public class UnitData : ScriptableObject {

    [Header("Unit")]
    public GameObject unitPrefab;
    public string unitName;

    [Header("Stats")]
    public int health;
    public int damage;
    public float range;
    public float timeBetweenAttacks;

    [Header("Cost")]
    public int goldCost;
    public int foodCost;
    public int woodCost;
    public int metalCost;
    public int populationCost;

}
