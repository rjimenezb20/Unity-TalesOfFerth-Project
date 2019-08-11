using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Unit : MonoBehaviour, IDamageable {

    [Header("BuildingsData")]
    public float rotationSpeed = 6;

    public bool selected = false;
    public GameObject selectionCircle;
    public UnitData unitData;
    public Canvas healthCanvas;
    public Image healthBar;

    private Renderer rend;
    private Ray ray;
    private RaycastHit hit;
    private CharacterController characterController;
    private Vector3 move;
    private bool order = false;
    private bool arrivedPosition = false;
    private bool attackOnArrive = false;
    private bool readyToAttack = true;
    private GameObject enemySelected;
    private GameObject enemyToAttack;
    private List<GameObject> enemiesOnAttackRange;
    private GameObject target;
    private float timer;
    private float health;
    private LayerMask layerMask;
    private Animator animator;
    private int swapAttack = 0;
    private UnitSFX sfx;
    private GameManager gameManager;
    private ResourcesManager resourcesManager;
    private NavMeshAgent agent;

    void Start () {

        rend = GetComponent<Renderer>();

        characterController = GetComponent<CharacterController>();

        move = this.transform.position;

        health = unitData.health;

        layerMask = ~(1 << LayerMask.NameToLayer("Checker"));

        animator = GetComponent<Animator>();

        enemiesOnAttackRange = new List<GameObject>();

        sfx = GetComponent<UnitSFX>();

        agent = GetComponent<NavMeshAgent>();

        GameObject gameManagers = GameObject.FindGameObjectWithTag("Manager"); 
        if(gameManagers != null) {

            gameManager = gameManagers.GetComponent<GameManager>();
            resourcesManager = gameManagers.GetComponent<ResourcesManager>();
        }
    }
	
	void Update () {

        CheckIfSelected();

        //When selected
        if (selected) {

            selectionCircle.SetActive(true);
            healthCanvas.enabled = true;
            ControlUnit();
            Debug.Log("Polla");
            
        } else {

            selectionCircle.SetActive(false);
            healthCanvas.enabled = false;
        }

        AttackSpeed();
        GoToCloseTarget();
        OnAttackRange();

        if (target != null) {

            if (target.GetComponent<Enemy>().GetIfDead()) { 

                target = null;
            }
        }

        if (enemySelected != null) {

            if (enemySelected.GetComponent<Enemy>().GetIfDead()) {

                enemySelected = null;
            }
        }

        if (enemyToAttack != null) {

            if (enemyToAttack.GetComponent<Enemy>().GetIfDead()) {

                enemyToAttack = null;
            }
        }
    }

    //Set place to move the unit or attack
    private void ControlUnit() {

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {

            //Cursor Change
            if (hit.transform.gameObject.tag == "Enemy") {

                gameManager.ChangeToAttackCursor();
            } else {

                gameManager.ChangeToNormalCursor();
            }
           
            //On right click
            if (Input.GetMouseButtonDown(1)) {

                if (hit.transform.gameObject.tag == "Enemy") {

                    enemySelected = hit.transform.gameObject;
                    agent.SetDestination(hit.transform.gameObject.transform.position);
                    hit.collider.gameObject.GetComponent<Enemy>().SelectedForAttackAnim();
                    agent.isStopped = false;

                } else {

                    agent.SetDestination(new Vector3(hit.point.x, 0, hit.point.z));
                    animator.SetBool("Running", true);
                    arrivedPosition = false;
                    order = true;
                    agent.isStopped = false;
                    target = null;
                    enemyToAttack = null;
                } 
            }
        }
            
        //Stop if arrive destination
        if (this.transform.position == agent.destination) {

            agent.isStopped = true;
            order = false;
        }

        //Stop animation
        if (agent.isStopped) {

            animator.SetBool("Running", false);
            animator.SetBool("Idle", true);
        } 
    }

    //If not moving and target setted, move to the target position
    private void GoToCloseTarget() {

        if (enemyToAttack != null && target == null && !order) {

            agent.SetDestination(enemyToAttack.transform.position);
            agent.isStopped = false;
        }
    }

    //If enemy is on attack range
    private void OnAttackRange() {

        if (!order) {

            if (enemiesOnAttackRange.Count >= 1) {

                if (enemySelected == null) {

                    target = enemyToAttack;
                } else {

                    target = enemySelected;
                }
                
                if (target != null) {

                    for (int i = 0; i < enemiesOnAttackRange.Count; i++) {

                        if (target == enemiesOnAttackRange[i]) {

                            agent.isStopped = true;
                            Vector3 dir = target.transform.position - this.transform.position;
                            Quaternion newRot = Quaternion.LookRotation(dir);
                            transform.rotation = Quaternion.Lerp(this.transform.rotation, newRot, Time.deltaTime * 3f);
                            Attack();
                        }
                    } 
                }    
            }       
        }
    }
    
    //Checks if you are trying to select an unit
    private void CheckIfSelected() {

        //Selection rectangle
        if (rend.isVisible && Input.GetMouseButtonUp(0)) {

            Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
            camPos.y = SelectionController.InvertMouseY(camPos.y);

            if (SelectionController.selection.Contains(camPos, true)) {

                selected = true;
            }
        }

        //Select on click
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!EventSystem.current.IsPointerOverGameObject()) {

            if (Physics.Raycast(ray, out hit)) {

                if (Input.GetMouseButtonDown(0) && hit.transform.position == this.transform.position) {

                    selected = true;
                } else if (Input.GetMouseButtonDown(0) && hit.transform.position != this.transform.position) {

                    selected = false;
                }
            }
        }
    }

    private void OnCollisionStay(Collision collision) {

        //Stopping unit when arrive (Group interaction - Game feeling)
        if (collision.gameObject.tag == "Unit" && collision.gameObject.GetComponent<Unit>().GetIfArrived()) {
            
            arrivedPosition = true;
            agent.isStopped = true;
        }
    }

    private void OnTriggerEnter(Collider collision) {

        //Check if unit is close to clicked position
        if (collision.gameObject.tag == "Checker") {

            arrivedPosition = true;
            agent.isStopped = true;
            order = false;
        }
    }

    //Take Damage (IDamageable)
    public void TakeDamage(int damage) {

        sfx.PlayReceiveDamageSound();

        health -= damage;
        healthBar.fillAmount = health / unitData.health;

        if (health <= 0) {

            Die();
        }
    }

    //Attack enemy
    public void Attack () {

        if (target != null) {

            if (readyToAttack) {

                if (swapAttack == 0) {

                    animator.SetTrigger("Attack");
                    swapAttack = 1;
                    sfx.PlayAttackSound();
                } else {

                    animator.SetTrigger("Attack2");
                    swapAttack = 0;
                    sfx.PlayAttackSound2();
                }

                readyToAttack = false;
            }
        }
    }

    //Attack Speed
    private void AttackSpeed() {

        timer += Time.deltaTime / 1;
        if (readyToAttack == false && timer >= unitData.timeBetweenAttacks) {

            readyToAttack = true;
            timer = 0;
        }
    }

    //When die
    private void Die() {

        resourcesManager.RemovePopulation(1);
        Destroy(gameObject);
    }

    //Update enemy on attack range list
    public void SetEnemiesOnAttackRange(List<GameObject> enemies) {

        enemiesOnAttackRange = enemies;
    }

    //Update enemy on range to move to attack list
    public void SetEnemyToAttack(GameObject enemy) {

         enemyToAttack = enemy;
    }

    //Subtracts health from target
    public void DoDamage() {

        if (target != null) {

            target.GetComponent<IDamageable>().TakeDamage(unitData.damage);
        }
    }

    //Return if unit has arrived position
    private bool GetIfArrived() {

        return arrivedPosition;
    }

    //Return health
    public float GetHealth() {

        return health;
    }
}

 