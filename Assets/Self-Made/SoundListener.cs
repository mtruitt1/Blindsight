using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundListener : BipedWalker {
    public bool ignoreReg = true;
    public float strengthMinRaw = 0f;
    public float strengthMinPercent = 0f;
    public int degreesTolerance = 1;
    public float distanceTolerance = 0.1f;
    protected HeardSound lastHeard = null;

    protected override void Start() {
        base.Start();
        foreach (StrikingObject striker in strikers) {
            striker.owner = this;
        }
    }

    public void SoundTouched(SoundWave wave) {
        if (wave.strength >= strengthMinRaw && (wave.strength / wave.maxStrength) >= strengthMinPercent && (!ignoreReg || wave.regularSound) && wave.origin != this && wave.origin?.tag != "Enemy") {
            lastHeard = new HeardSound(wave.strength, wave.strength / wave.maxStrength, wave.transform.position);
            forwardGoal = 0f;
            rightGoal = 0f;
            Vector3 flatHere = transform.position;
            flatHere.y = 0f;
            Vector3 flatThere = wave.transform.position;
            flatThere.y = 0f;
            turnGoal = Vector3.SignedAngle(root.forward, flatThere - flatHere, transform.up);
        }
    }

    protected override void Update() {
        base.Update();
        if (lastHeard != null) {
            Vector3 flatHere = transform.position;
            flatHere.y = 0f;
            Vector3 flatThere = lastHeard.position;
            flatThere.y = 0f;
            if (Vector3.Angle(root.forward, flatThere - flatHere) <= degreesTolerance) {
                turnGoal = 0f;
                turnCurrent = 0f;
                forwardGoal = 1 - lastHeard.percent;
            } else if (Vector3.Distance(flatHere, flatThere) <= distanceTolerance) {
                forwardGoal = 0f;
            }
        }
    }

    public class HeardSound {
        public float strength;
        public float percent;
        public Vector3 position;

        public HeardSound(float s, float p, Vector3 pos) {
            strength = s;
            percent = p;
            position = pos;
        }
    }
}