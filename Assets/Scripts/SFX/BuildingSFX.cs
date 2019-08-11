using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSFX : MonoBehaviour {

	[Header("Audios")]
    public AudioClip receiveDamage;
    public AudioClip build;

    [Header("Tower Audios")]
    public AudioClip arrowTower;
    public AudioClip dragonTower;

    private AudioSource source;


	void Start () {

        source = GetComponent<AudioSource>();
	}

    public void PlayReceiveDamageSound() {

        source.clip = receiveDamage;
        source.Play();
    }

    public void PlayBuildSound() {

        source.clip = build;
        source.Play();
    }

    public void PlayArrowTowerSound() {

        source.clip = build;
        source.Play();
    }

    public void PlayDragonTowerSound() {

        source.clip = build;
        source.Play();
    }
}
