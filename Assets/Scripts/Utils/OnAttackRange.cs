using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAttackRange : MonoBehaviour {

    private Unit unit;
    private List<GameObject> enemies;

    private void Start() {

        unit = GetComponentInParent<Unit>();
        enemies = new List<GameObject>();
    }

    private void Update() {

        for (int i = 0; i < enemies.Count; i++) {

            if (enemies[i] == null) {

                enemies.Remove(enemies[i]);
                unit.SetEnemiesOnAttackRange(enemies);
            }  
        }
    }

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == "Enemy" && !other.isTrigger) {

            enemies.Add(other.gameObject);
            unit.SetEnemiesOnAttackRange(enemies);
        }
    }

    private void OnTriggerExit(Collider other) {
        
        if (other.gameObject.tag == "Enemy" && !other.isTrigger) {

            enemies.Remove(other.gameObject);
            unit.SetEnemiesOnAttackRange(enemies);
        }
    }
}
