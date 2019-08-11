using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroupController : MonoBehaviour {

    public float stopDistance = 2f;

    private NavMeshAgent agent;
    private Ray ray;
    private RaycastHit hit;
    private int groupCount;
    private WavesController wavesController;
    private Transform currentPath;
    private List<Transform> wayPoints;
    private int currentWayPoint = 0;
    private Vector3 nextPosition;
    private Vector3 position;


    // Use this for initialization
    void Start () {

        wavesController = GameObject.FindGameObjectWithTag("WavesController").GetComponent<WavesController>();

        currentPath = wavesController.SelectPath();

        agent = this.GetComponent<NavMeshAgent>();

        groupCount = this.transform.childCount;
    }
	
	// Update is called once per frame
	void Update () {

        NavMeshMove();

        if (Input.GetKeyDown("q")) {

            DisableGroup();
        }
    }

    private void Movement() {

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
           
            if (Input.GetMouseButtonDown(1)) {

                agent.SetDestination(new Vector3(hit.point.x, 0, hit.point.z));
            }
        }
    }

    private void NavMeshMove () {

        position = this.transform.position;

        if (currentWayPoint < currentPath.childCount) {

            nextPosition = currentPath.GetChild(currentWayPoint).position;
            agent.SetDestination(currentPath.GetChild(currentWayPoint).position);

            if (Vector3.Distance(agent.transform.position, nextPosition) <= stopDistance) {

                currentWayPoint++;
            }

        } else {

            agent.isStopped = true;
        }
    }

    private void DisableGroup() {

        agent.enabled = false;
        GetComponent<MiniMapComponent>().enabled = false;

        for (int i = 1; i <= groupCount; i++) {

            GetComponentsInChildren<NavMeshAgent>()[i].enabled = true;
        }     
        
        for (int i = 0; i < groupCount; i++) {

            GetComponentsInChildren<Enemy>()[i].enabled = true;
        }

        this.enabled = false;
    }

    private void OnTriggerEnter(Collider other) {
        
        if (other.gameObject.tag == "Vision") {

            DisableGroup();
        }
    }
}
