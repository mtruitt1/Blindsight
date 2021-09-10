using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UnstableObject : SoundBouncer {
    private Rigidbody rb;
    public float stabilityTime = 0f;
    private float? stabilityTimer = null;

    protected override void Start() {
        base.Start();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public override void DoBounceBehavior(SoundWave wave, SoundObject highest, SoundObject maker) {
        base.DoBounceBehavior(wave, highest, maker);
        PlayRandSound(strength, false, false);
        stabilityTimer = stabilityTime;
    }

    private void Update() {
        if (stabilityTimer > 0f) {
            stabilityTimer -= Time.deltaTime;
            if (stabilityTimer <= 0f) {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }
    }
}