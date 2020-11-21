using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//anything with a rigidbody that should make a sound on contact gets one of these scripts-- the moveball and foot colliders are no different
public class StrikingObject : SoundObject {
    public float strengthMult = 1f;
    public float minColMag = 1f;
    public float maxColMag = 1000f;
    public float freezeTimer = -1f;
    private float freezeClock;
    private Rigidbody rb;

    protected override void Start() {
        base.Start();
        rb = GetComponent<Rigidbody>();
        freezeClock = freezeTimer;
    }

    private void OnCollisionEnter(Collision collision) {
        float strength = Mathf.Clamp(collision.relativeVelocity.magnitude, minColMag, maxColMag) * strengthMult;
        PlayRandSound(strength, true, false);
        //Debug.Log("Sound made, strength: " + strength + " @ " + transform.position.ToString());
    }

    private void Update() {
        if (freezeClock > 0f) {
            if (rb.velocity.sqrMagnitude <= 0.001f) {
                freezeClock -= Time.deltaTime;
                if (freezeClock <= 0f) {
                    Destroy(rb);
                }
            } else {
                freezeClock = freezeTimer;
            }
        }
        if (transform.position.y <= -100f) {
            Destroy(gameObject);
        }
    }
}