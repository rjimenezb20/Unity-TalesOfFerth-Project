using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour {

	public float speed = 3f;

    private int damage = 0;
    private Transform currentTarget;
    private IDamageable damageable;
    private float counter = 0f;
    private string targetType;
    private AudioSource source;
    private Renderer rend;

    private void Start() {

        source = GetComponent<AudioSource>();

        rend = GetComponent<Renderer>();
    }

    void Update () {

        Vector3 move;
        if (currentTarget == null) {

            move = Vector3.Lerp(this.transform.position, this.transform.position + -this.transform.up, Time.deltaTime * 10 * speed);
        } else {

            move = Vector3.MoveTowards(this.transform.position, currentTarget.position + new Vector3(0, 1, 0), Time.deltaTime * speed);
        }
        this.transform.position = move;

        counter += Time.deltaTime;
        if(counter >= 10f) {

            DestroyBullet();
        }
	}

    public void SetTarget(Transform target) {

        currentTarget = target;

        Vector3 dir = currentTarget.position - this.transform.position - new Vector3( 0, -90 ,0);
        Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = rot;
    }

    private void OnCollisionEnter(Collision other) {

        if (targetType == "Units") {

            if (other.collider.gameObject.tag == "Unit" || other.collider.gameObject.tag == "Building") {

                source.Play();
                other.collider.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
                DestroyBullet();
            } 

        } else {

            if (other.collider.gameObject.tag == "Enemy") {

                source.Play();
                other.collider.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
                DestroyBullet();
            } 
        }

        if (other.collider.gameObject.tag == "Ground") {

            Destroy();
        }
    }

    private void DestroyBullet() {

        rend.enabled = false;
        GetComponent<Collider>().enabled = false;
        Invoke("Destroy", 2f);
    }

    private void Destroy() {

        Destroy(gameObject);
    }

    public void SetDamage(int damage) {

        this.damage = damage;
    }

    public void SetTargetType(string type) {

        targetType = type;
    }
}
