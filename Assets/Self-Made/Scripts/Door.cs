using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : SoundBouncer {
    public float openTime = 0.5f;
    private bool open = false;
    private float openTimer = 0f;
    private Vector3 startEulers;
    public Vector3 endEulers;
    public Transform rotate;

    protected override void Start() {
        base.Start();
        openTimer = openTime;
        startEulers = rotate.localEulerAngles;
    }

    protected override void DoBounceBehavior(SoundWave wave, SoundObject highest, SoundObject maker) {
        PlayRandSound(strength, false, false);
        open = true;
    }

    private void Update() {
        if (open) {
            openTimer -= Time.deltaTime;
            rotate.localEulerAngles = Vector3.Lerp(endEulers, startEulers, openTimer / openTime);
        }
    }
}