using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sawmill : MonoBehaviour {

    private int woodBenefit = 0;
    private List<GameObject> trees;

    private void Start() {

        trees = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other) {
        
        if(other.gameObject.tag == "Tree") {

            woodBenefit += 1;
            other.gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
            trees.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {

        if(other.gameObject.tag == "Tree") {

            woodBenefit -= 1;
            other.gameObject.GetComponentInChildren<Renderer>().material.color = Color.white;
            trees.Remove(other.gameObject);
        }
    }

    public int GetWoodBenefit() {

        return woodBenefit;
    }

    public void ChangeBackTreeColor() {

        for (int i = 0; i < trees.Count; i++) {

            trees[i].GetComponentInChildren<Renderer>().material.color = Color.white;
        }
    }
}
