using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RadiusRange : MonoBehaviour {

    public int segments = 60;
    public float xradius = 5;
    public float yradius = 5;

    private LineRenderer line;
    private bool showLine = true;
    private bool created = false;

	void Start () {

        line = gameObject.GetComponent<LineRenderer>();

        line.positionCount = segments + 1;
        line.useWorldSpace = false;
    }

    private void Update() {

        if (showLine) {

            line.positionCount = segments + 1;
            CreatePoints();

        } else if (!showLine) {

            line.positionCount = 0;
            created = false;
        }
    }

    void CreatePoints() {

        if (!created) {

            float x;
            float z;

            float angle = 0f;

            for (int i = 0; i < (segments + 1); i++) {

                x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
                z = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

                line.SetPosition(i, new Vector3(x, 0, z));

                angle += (360f / segments);
            }

            created = true;
        } 
    }

    public bool GetShowLine() {

        return showLine;
    }

    public void SetShowLine(bool aux) {

        showLine = aux;
    }
}
