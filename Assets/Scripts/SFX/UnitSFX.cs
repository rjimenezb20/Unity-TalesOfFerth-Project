using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSFX : MonoBehaviour {

    [Header("Audios")]
    public AudioClip receiveDamage;
    public AudioClip attack;
    public AudioClip attack2;

    private AudioSource source;


	void Start () {

        source = GetComponent<AudioSource>();
	}

    public void PlayReceiveDamageSound() {

        source.clip = receiveDamage;
        source.Play();
    }

    public void PlayAttackSound() {

        source.clip = attack;
        source.Play();
    }

    public void PlayAttackSound2() {

        source.clip = attack2;
        source.Play();
    }
}
