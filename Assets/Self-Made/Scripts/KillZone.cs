using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//kills the player on contact
public class KillZone : MonoBehaviour {
    public AudioClip clip;

    //if the player touches this collider, kill it
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            PlayerControls.local.PlayerDeath();
            if (clip != null) {
                AudioSource.PlayClipAtPoint(clip, transform.position, 3f);
            }
        }
    }
}