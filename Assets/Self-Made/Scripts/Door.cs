using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the class for the metal door-- it inherits from the soundbouncer class like the lever and gear both do
public class Door : SoundBouncer {
    public float openTime = 0.5f;
    private bool open = false;
    private float openTimer = 0f;
    private Vector3 startEulers;
    public Vector3 endEulers;
    public Transform rotate;

    //set the time to open, and get the starting euler angles so rotation works smoothly
    protected override void Start() {
        base.Start();
        openTimer = openTime;
        startEulers = rotate.localEulerAngles;
    }

    //if the door is not open, play its sound and set it to open
    protected override void DoBounceBehavior(SoundWave wave, SoundObject highest, SoundObject maker) {
        if (!open) {
            PlayRandSound(strength, false, false);
        }
        open = true;
    }

    //linearly transition from start to end eulers. the lerp function has reversed order because it makes it easier
    private void Update() {
        if (open) {
            openTimer -= Time.deltaTime;
            rotate.localEulerAngles = Vector3.Lerp(endEulers, startEulers, openTimer / openTime);
        }
    }
}