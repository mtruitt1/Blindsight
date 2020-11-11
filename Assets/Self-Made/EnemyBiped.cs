using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBiped : BipedWalker {
    public int degreesTolerance = 1;
    public float distanceTolerance = 0.1f;

    protected override void Start() {
        base.Start();
        foreach (StrikingObject striker in strikers) {
            striker.owner = this;
        }
    }

    protected override void SoundReact(SoundWave wave) {
        base.SoundReact(wave);
        bool notAnotherEnemy = true;
        if (wave.origin != null) {
            notAnotherEnemy = wave.origin.tag != "Enemy";
        }
        if (notAnotherEnemy) {
            lastHeard = new HeardSound(wave.strength, wave.strength / wave.maxStrength, wave.transform.position);
            forwardGoal = 0f;
            rightGoal = 0f;
            Vector3 flatHere = transform.position;
            flatHere.y = 0f;
            Vector3 flatThere = wave.transform.position;
            flatThere.y = 0f;
            turnGoal = Vector3.SignedAngle(root.forward, flatThere - flatHere, transform.up) > 0 ? 1 : -1;
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
                forwardGoal = 1 - lastHeard.percent;
            } else if (Vector3.Distance(flatHere, flatThere) <= distanceTolerance) {
                forwardGoal = 0f;
            }
        }
    }
}