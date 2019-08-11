using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable {

    public float stopDistance = 2f;
    public float attackOffset = 2f;
    public EnemyData enemyData;
    public Canvas healthCanvas;
    public Image healthBar;

    [Header("Distance")]
    public Transform instancePoint;

    private Renderer renderer;
    private WavesController wavesController;
    private NavMeshAgent agent;
    private Transform currentPath;
    private List<Transform> wayPoints;
    private int currentWayPoint = 0;
    private Vector3 nextPosition;
    private Animator anim;
    private Vector3 position;
    private float health;
    private UnitInRange unitInRange;
    private GameObject closeTarget;
    private GameObject target;
    private bool readyToAttack = true;
    private float timer = 0;
    private float healTimer = 0;
    private List<GameObject> targetsOnAttackRange;
    private int targetCounter = 0;
    private bool attacking = false;
    private Animator animator;
    private int swapAttack;
    private EnemySFX sfx;
    private Quaternion healthBarRotation;
    private GameObject rock;
    private bool throwing;
    private Proyectil bulletController;
    private Color initialColor;
    private bool dead = false;

    void Start () {

        wavesController = GameObject.FindGameObjectWithTag("WavesController").GetComponent<WavesController>();

        agent = GetComponent<NavMeshAgent>();

        currentPath = wavesController.SelectPath();

        anim = GetComponent<Animator>();

        health = enemyData.health;

        unitInRange = GetComponentInChildren<UnitInRange>();

        targetsOnAttackRange = new List<GameObject>();

        animator = GetComponent<Animator>();

        sfx = GetComponent<EnemySFX>();

        healthBarRotation = healthBar.transform.rotation;

        healthCanvas.enabled = false;

        renderer = GetComponent<Renderer>();

        initialColor = renderer.material.color;

        this.enabled = false;

    }
	
	void Update () {
        
        if (!dead) {

            AttackOnRange();

            //GOLEM throw
            if (throwing) {

                if (rock != null) {

                    rock.transform.position = instancePoint.transform.position;
                }
            }

            //Show health bar when damaged
            if (health < enemyData.health) {

                healthCanvas.enabled = true;
            } 

            //Health bar always looking the same direction
            healthCanvas.transform.rotation = healthBarRotation;

            //If theres a enemy move to it, else move
            if (closeTarget != null && !attacking) {

                MoveToTarget();

            } else if (!attacking) {

                NavMeshMove();
            }

            if (targetsOnAttackRange.Count <= 0) {

                attacking = false;
            }

            //Attack speed
            timer += Time.deltaTime / 1;
            if (readyToAttack == false && timer >= enemyData.timeBetweenAttacks) {

                readyToAttack = true;
                timer = 0;
            }

            if (agent != null) {

                if (agent.isStopped) {

                    animator.SetBool("Idle", true);
                    animator.SetBool("Running", false);
                } else {

                    animator.SetBool("Running", true);
                }
            }
        }

        if (dead) {

            this.GetComponent<Collider>().enabled = false;
            healthCanvas.enabled = false;
            agent.isStopped = true;
        }
	}

    //Controls the movement through nav mesh
    private void NavMeshMove () {

        if (this.transform.position != wavesController.GetLastPoint().position) {

            agent.SetDestination(wavesController.GetLastPoint().position);
        } else {

            agent.isStopped = true;
        }

        if (closeTarget == null && target == null && agent.isStopped == true && !attacking) {

            agent.isStopped = false;
        }
    }

    //Rotate forward
    private void RotateEnemy() {

        Vector3 target;
        if (currentWayPoint < currentPath.childCount) {

            target = currentPath.GetChild(currentWayPoint).position;
        } else {

            target = currentPath.GetChild(currentPath.childCount - 1).position;
        }

        Vector3 dir = target - this.transform.position;
        Quaternion newRot = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Lerp(this.transform.rotation, newRot, Time.deltaTime * 3f);
    }

    private void AttackOnRange() {

        if (targetsOnAttackRange.Count >= 1) {

            target = targetsOnAttackRange[0];

            Vector3 dir = target.transform.position - this.transform.position;
            Quaternion newRot = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Lerp(this.transform.rotation, newRot, Time.deltaTime * 3f);

            attacking = true;
            agent.isStopped = true;

            if (readyToAttack) {

                Attack();
                readyToAttack = false;
            }
        } else {
            
            attacking = false;
            if (agent != null) {

                agent.isStopped = false;
            }
            target = null;
        }
    }

    //Moves to target position
    private void MoveToTarget() {

        if (agent != null) {

            agent.SetDestination(closeTarget.transform.position);
            agent.isStopped = false;
        }
    }

    //Take Damage (IDamageable)
    public void TakeDamage(int damage) {

        health -= damage;
        healthBar.fillAmount = health / enemyData.health;
        sfx.PlayReceiveDamageSound();

        if (health <= 0) {

            Die();
            dead = true;
        }
    }

    //Attack
    public void Attack() {
        
        if (swapAttack == 0) {

            animator.SetTrigger("Attack");
            swapAttack = 1;

            if (enemyData.enemyName == "OrcWarrior") {

                sfx.PlayAttackSound();
            }
            
        } else {

            animator.SetTrigger("Attack2");
            swapAttack = 0;
            
            if (enemyData.enemyName == "OrcWarrior") {

                sfx.PlayAttackSound2();
            }
        }
    }

    public void DoDamage() {

        if (target != null) {

            target.GetComponent<IDamageable>().TakeDamage(enemyData.damage);
        }

        if (enemyData.enemyName == "GolemBoss") {

            sfx.PlayAttackSound();
        }
    }

    //When die
    public void Die() {

        animator.SetBool("Dead", true);
        Invoke("Destroy", 6);
    }

    private void Destroy() {

        Destroy(gameObject);
    }

    //Set if there's a close target (Called on UnitInRange)
    public void SetCloseTarget(GameObject target) {

        closeTarget = target;
    }

    public void SetTargetOnAttackRange(List<GameObject> target) {

        targetsOnAttackRange = target;
    }

    public float GetHealth() {

        return health;
    }

    public void SelectedForAttackAnim() {

        renderer.material.color = Color.Lerp(this.renderer.material.color, Color.red, 0.50f);
        Invoke("NormalColor", 1f);
    }

    private void NormalColor() {

        renderer.material.color = Color.Lerp(this.renderer.material.color, Color.white, 0.50f);
    }

    public bool GetIfDead() {

        return dead;
    }

    //GOLEM -----
    public void InstanciateRock() {

        rock = Instantiate(enemyData.proyectil, instancePoint.position, Quaternion.identity);
        bulletController = rock.GetComponent<Proyectil>();
        bulletController.SetTargetType("Units");
        bulletController.SetDamage(enemyData.damage);
        throwing = true;
    }

    public void ThrowRock() {

        throwing = false;
        rock.GetComponent<Collider>().enabled = true;

        if (target != null) {

            bulletController.SetTarget(target.transform);
        }
    }

    //LICH -----
    public void MagicAttack() {

        GameObject magicProyectil = Instantiate(enemyData.proyectil, instancePoint.position, Quaternion.identity);
        bulletController = magicProyectil.GetComponent<Proyectil>();
        bulletController.SetTargetType("Units");
        bulletController.SetDamage(enemyData.damage);

        if (target != null) {

            bulletController.SetTarget(target.transform);
        }
    }
}