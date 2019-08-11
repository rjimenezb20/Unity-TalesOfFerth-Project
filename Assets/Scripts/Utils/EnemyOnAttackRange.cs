using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOnAttackRange : MonoBehaviour {

	private Enemy enemy;
    private List<GameObject> targets;

    private void Start() {

        enemy = GetComponentInParent<Enemy>();
        targets = new List<GameObject>();
    }

    private void Update() {

        for (int i = 0; i < targets.Count; i++) {

            if (targets[i] == null) {

                targets.Remove(targets[i]);
                enemy.SetTargetOnAttackRange(targets);
            }  
        }
    }

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == "Unit" || other.gameObject.tag == "Building" && !other.isTrigger) {

            targets.Add(other.gameObject);
            enemy.SetTargetOnAttackRange(targets);
        }
    }

    private void OnTriggerExit(Collider other) {
        
        if (other.gameObject.tag == "Unit" || other.gameObject.tag == "Building" && !other.isTrigger) {

            targets.Remove(other.gameObject);
            enemy.SetTargetOnAttackRange(targets);
        }
    }
}
