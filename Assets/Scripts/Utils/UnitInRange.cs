using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInRange : MonoBehaviour {

    public Enemy enemy;

    private void Start() {

        enemy = GetComponentInParent<Enemy>();
    }

    public void OnTriggerStay(Collider other) {
        
        if (other.gameObject.tag == "Unit" || other.gameObject.tag == "Building" && !other.isTrigger) {

            enemy.SetCloseTarget(other.gameObject);
        }
    }
}
