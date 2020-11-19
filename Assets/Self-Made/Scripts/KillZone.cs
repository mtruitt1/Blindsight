using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour {
    public AudioClip clip;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            PlayerControls.local.PlayerDeath();
            AudioSource.PlayClipAtPoint(clip, transform.position, 3f);
        }
    }
}