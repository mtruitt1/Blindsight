using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the gear. behaves mainly as a soundbouncer, but also spins
public class Gear : SoundBouncer {
    public float turnSpeed = 5f;
    private bool turn = false;
    public RotateAxis turnAxis = RotateAxis.Forward;

    //bounce sound when hit
    protected override void DoBounceBehavior(SoundWave wave, SoundObject highest, SoundObject maker) {
        base.DoBounceBehavior(wave, highest, maker);
        turn = true;
    }

    //spin if meant to spin
    private void Update() {
        if (turn) {
            transform.RotateAround(transform.position, AxisToV3(turnAxis), turnSpeed * Time.deltaTime);
        }
    }

    //convert an axis to a Vector3 for easy use
    private Vector3 AxisToV3(RotateAxis ax) {
        switch (ax) {
            case RotateAxis.Forward: return transform.forward;
            case RotateAxis.Right: return transform.right;
            case RotateAxis.Up: return transform.up;
        }
        return new Vector3();
    }

    //the three main axes
    public enum RotateAxis {
        Forward = 0,
        Up = 1,
        Right = 2
    }
}