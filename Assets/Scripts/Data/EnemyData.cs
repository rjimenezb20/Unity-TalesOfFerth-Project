using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "New Enemy")]
public class EnemyData : ScriptableObject {

    [Header("Enemy")]
    public GameObject buildingPrefab;
    public string enemyName;
    public int health;
    public int damage;
    public float timeBetweenAttacks;

    [Header("Distance Attack")]
    public GameObject proyectil;

}
