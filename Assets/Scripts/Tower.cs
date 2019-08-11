using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

	public BuildingData data;

    [Header("Turret Fire Options")]
    public GameObject movePart;
    public Transform firePoint;

    [Header("Turret Effects")]
    public AudioClip shootSound;

    private List<GameObject> enemiesInRange = new List<GameObject>();
    private GameObject target;
    private float fireRateCounter = 0f;
    private Animator animator;
    private int aux = 0;
    private AudioSource source;

    void Start() {
        
        if (movePart != null) {

            animator = GetComponent<Animator>();
        }

        source = GetComponent<AudioSource>();
    }

    void Update () {

        for (int i = 0; i < enemiesInRange.Count; i++) {

            if (enemiesInRange[i].GetComponent<Enemy>().GetIfDead()) {

                enemiesInRange.Remove(enemiesInRange[i]);
            }
        }

        if(enemiesInRange.Count > 0) {

            target = enemiesInRange[aux];

            if (target != null) {

                if (movePart != null) {

                    Vector3 look = target.transform.position - movePart.transform.position;
                    Quaternion newRotation = Quaternion.LookRotation(look, Vector3.up);
                    Vector3 rot = Quaternion.Lerp(movePart.transform.rotation, newRotation, Time.deltaTime * 5).eulerAngles;
                    movePart.transform.rotation = Quaternion.Euler(0f, rot.y, 0f);
                }
                
                if (fireRateCounter >= data.timeBetweenAttacks) {

                    if (movePart == null) {

                        Shoot();
                    } else {

                        animator.SetTrigger("Attack");
                    }
                    fireRateCounter = 0f; 
                }
            } else {
                enemiesInRange.RemoveAt(0);
            }

        } else {

            target = null;
        }
        fireRateCounter += Time.deltaTime / 1;

        if (target != null && enemiesInRange.Count > 0) {

            if (target.GetComponent<Enemy>().GetIfDead()) {

                if (enemiesInRange.Count - 1 > aux) {

                    aux += 1;
                    target = enemiesInRange[aux];
                } else {

                    enemiesInRange.Remove(target);
                    aux = 0;
                } 
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        
        if(other.CompareTag("Enemy") && !other.isTrigger) {

            enemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {

        if (other.CompareTag("Enemy") && !other.isTrigger) {

            enemiesInRange.Remove(other.gameObject);
        } 
    }

    private void Shoot() {

        GameObject bullet = Instantiate(data.proyectilPrefab, firePoint.position, Quaternion.identity);
        Proyectil bulletController = bullet.GetComponent<Proyectil>();

        if (target != null) {

            bulletController.SetTarget(target.transform);
        }
        
        bulletController.SetDamage(data.damage);
        source.Play();
    }
}
