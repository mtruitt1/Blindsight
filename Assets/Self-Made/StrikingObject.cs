using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikingObject : MonoBehaviour {
    public float strengthMult = 1f;
    public float minColMag = 1f;
    public float maxColMag = 1000f;
    public SoundReceiver owner = null;

    private void OnCollisionEnter(Collision collision) {
        float strength = Mathf.Clamp(collision.relativeVelocity.magnitude, minColMag, maxColMag) * strengthMult;
        GameManager.SpawnSound(strength, transform.position, true, false, owner);
        //Debug.Log("Sound made, strength: " + strength + " @ " + transform.position.ToString());
    }
}