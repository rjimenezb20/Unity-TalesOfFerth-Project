using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneWarehouse : MonoBehaviour {

	private int stoneBenefit = 0;
    private List<GameObject> stones;

    private void Start() {

        stones = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other) {
        
        if(other.gameObject.tag == "Stone") {

            stoneBenefit += 1;
            other.gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
            stones.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {

        if(other.gameObject.tag == "Stone") {

            stoneBenefit -= 1;
            other.gameObject.GetComponentInChildren<Renderer>().material.color = Color.grey;
            stones.Remove(other.gameObject);
        }
    }

    public int GetStoneBenefit() {

        return stoneBenefit;
    }

    public void ChangeBackStoneColor() {

        for (int i = 0; i < stones.Count; i++) {

            stones[i].GetComponentInChildren<Renderer>().material.color = Color.grey;
        }
    }
}
