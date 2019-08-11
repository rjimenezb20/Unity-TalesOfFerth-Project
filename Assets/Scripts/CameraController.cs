using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [Header("Camera Limit")]
    public Transform topLimit;
    public Transform bottomLimit;
    public Transform rightLimit;
    public Transform leftLimit;

    public float speed = 0.5f;

    public GameObject checker;

    private Camera mainCamera;
    private float screenWidth;
    private float screenHeight;
    private Vector3 mousePosition;
    private Ray ray;
    private RaycastHit hit;
    private GameObject checkerToDestroy;

    void Start() {

        mainCamera = Camera.main;

        screenWidth = Screen.width;

        screenHeight = Screen.height;

        Time.timeScale = 1;
    }

    void Update () {

        SetPositionToMoveUnits();

        //Camera movement and limit
        mousePosition = Input.mousePosition;

        if (mousePosition.x <= 5 || Input.GetKey("a")) {

            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, mainCamera.transform.position + new Vector3(0, 0, 1), speed);
        }

        if (mousePosition.x >= screenWidth - 5 || Input.GetKey("d")) {

            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, mainCamera.transform.position + new Vector3(0, 0, -1), speed);
        }

        if (mousePosition.y <= 5 || Input.GetKey("s")) {

            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, mainCamera.transform.position + new Vector3(-1, 0, 0), speed);
        }

        if (mousePosition.y >= screenHeight - 5 || Input.GetKey("w")) {

            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, mainCamera.transform.position + new Vector3(1, 0, 0), speed);
        }

        mainCamera.transform.position = new Vector3(Mathf.Clamp(mainCamera.transform.position.x, bottomLimit.position.x, topLimit.position.x), Mathf.Clamp(mainCamera.transform.position.y, 25, 50), Mathf.Clamp(mainCamera.transform.position.z, rightLimit.position.z, leftLimit.position.z));

        //Scroll Zoom
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) { // forward 

            if (mainCamera.transform.position.y > 25) {

                mainCamera.transform.localPosition += new Vector3(1, -1, 0);
            }
                        
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0f) { // backwards 

            if (mainCamera.transform.position.y < 50) {

                mainCamera.transform.localPosition += new Vector3(-1, 1, 0);
            }
        }

    }

    //Instanciate collider for game feel for units
    public void SetPositionToMoveUnits() {

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(1)) {

            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {

                if (checkerToDestroy != null) {

                    Destroy(checkerToDestroy);
                }

                checkerToDestroy = Instantiate(checker, hit.point, Quaternion.identity);
            }
        }
    }
}

