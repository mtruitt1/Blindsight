using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//anything with a rigidbody that should make a sound on contact gets one of these scripts-- the moveball and foot colliders are no different
public class StrikingObject : SoundObject {
    public float strengthMult = 1f;
    public float minColMag = 1f;
    public float maxColMag = 1000f;

    private void OnCollisionEnter(Collision collision) {
        float strength = Mathf.Clamp(collision.relativeVelocity.magnitude, minColMag, maxColMag) * strengthMult;
        PlayRandSound(strength, true, false);
        //Debug.Log("Sound made, strength: " + strength + " @ " + transform.position.ToString());
    }
}