using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderHelper : MonoBehaviour {

    private bool onLight;

    private void OnTriggerStay(Collider other) {

        if (other.gameObject.tag == "Vision" && other.GetComponent<FieldOfView>().enabled) {

            onLight = true; 
        }
    }

    private void OnTriggerExit(Collider other) {
        
        if (other.gameObject.tag == "Vision") {

            onLight = false;
        }
    }

    public bool GetOnLight() {

        return onLight;
    }
}
