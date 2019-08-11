using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

    public float healthBarXoffSet;
    public float healthBarYoffSet;
    public float healthBarZoffSet;

    private Camera mainCamera;

    void Start () {

        mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {

        this.transform.eulerAngles = new Vector3(mainCamera.transform.eulerAngles.x, mainCamera.transform.eulerAngles.y , transform.eulerAngles.z);
        this.transform.position = new Vector3(this.transform.parent.gameObject.transform.position.x + healthBarXoffSet, this.transform.parent.gameObject.transform.position.y + healthBarYoffSet, this.transform.parent.gameObject.transform.position.z + healthBarZoffSet);
    }
}
