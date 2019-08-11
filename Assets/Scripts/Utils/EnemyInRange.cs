using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInRange : MonoBehaviour {

    public Unit unit;
    public GameObject closeEnemy;

    private void Start() {

        unit = GetComponentInParent<Unit>();
    }

    private void OnTriggerStay(Collider other) {

        if (other.gameObject.tag == "Enemy" && !other.isTrigger) {

            closeEnemy = other.gameObject;
            unit.SetEnemyToAttack(closeEnemy);
        }
    }

    private void OnTriggerExit(Collider other) {

        if (other.gameObject.tag == "Enemy" && !other.isTrigger) {

            closeEnemy = null;
            unit.SetEnemyToAttack(closeEnemy);
        }
    }
}
