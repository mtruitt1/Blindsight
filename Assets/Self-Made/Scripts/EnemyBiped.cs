using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBiped : BipedWalker {
    public int degreesTolerance = 1;
    public float distanceTolerance = 0.1f;
    public float sprintDistance = 0f;

    protected override void Start() {
        base.Start();
        foreach (StrikingObject striker in strikers) {
            striker.owner = this;
        }
    }

    protected override void SoundReact(SoundWave wave, SoundObject highest, SoundObject maker) {
        base.SoundReact(wave, highest, maker);
        bool notAnotherEnemy = true;
        if (highest != null) {
            notAnotherEnemy = highest.tag != "Enemy";
        }
        if (notAnotherEnemy) {
            //use raycast to check if within sight
            lastHeard = new HeardSound(wave.strength, wave.strength / wave.maxStrength, wave.transform.position);
        }
    }

    protected override void Update() {
        base.Update();
        if (lastHeard != null) {
            Vector3 flatHere = transform.position;
            flatHere.y = 0f;
            Vector3 flatThere = lastHeard.position;
            flatThere.y = 0f;
            float distance = Vector3.Distance(flatHere, flatThere);
            sprint = distance >= sprintDistance;
            if (Vector3.Angle(root.forward, flatThere - flatHere) <= degreesTolerance) {
                angleToTurn = 0f;
            } else {
                angleToTurn = Vector3.SignedAngle(root.forward, flatThere - flatHere, transform.up);
            }
            if (distance <= distanceTolerance) {
                forwardGoal = 0f;
                lastHeard = null;
            } else {
                forwardGoal = 1f;
            }
        }
    }
}