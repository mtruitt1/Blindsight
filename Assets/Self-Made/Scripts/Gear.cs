using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the gear. behaves mainly as a soundbouncer, but also spins
public class Gear : SoundBouncer {
    public float turnSpeed = 5f;
    public RotateAxis turnAxis = RotateAxis.Forward;

    //bounce sound when hit
    public override void DoBounceBehavior(SoundWave wave, SoundObject highest, SoundObject maker) {
        base.DoBounceBehavior(wave, highest, maker);
        toggleable = !toggleable;
    }

    //spin if meant to spin
    private void Update() {
        if (toggleable) {
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