using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour {

    [Header("Selection texture")]
    public Texture2D selectionHighlight = null;
    public static Rect selection = new Rect(0, 0, 0, 0);

    private Vector3 startClick = -Vector3.one;


    void Update () {
		
        if (!BuildManager.building) {

            Selection();
        }
	}

    //Rectangle selection
    private void Selection() {

        if (Input.GetMouseButtonDown(0)) {

            startClick = Input.mousePosition;

        } else if (Input.GetMouseButtonUp(0)) {

            if(selection.width < 0) {

                selection.x += selection.width;
                selection.width = -selection.width;
            }

            if (selection.height < 0) {

                selection.y += selection.height;
                selection.height = -selection.height;
            }

            startClick = -Vector3.one;
        }

        if (Input.GetMouseButton(0)) {

            selection = new Rect(startClick.x, InvertMouseY(startClick.y), Input.mousePosition.x - startClick.x, InvertMouseY(Input.mousePosition.y) - InvertMouseY(startClick.y));
        }
    }

    public static float InvertMouseY(float y) {

        return Screen.height - y;
    }

    //Draw rectangle
    private void OnGUI() {
        
        if (startClick != -Vector3.one) {

            GUI.color = new Color(1, 1, 1, 0.5f);
            GUI.DrawTexture(selection, selectionHighlight);
        }
    }
}
