using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : SoundBouncer {
    public float turnSpeed = 5f;
    private bool turn = false;
    public RotateAxis turnAxis = RotateAxis.Forward;

    protected override void DoBounceBehavior(SoundWave wave, SoundObject highest, SoundObject maker) {
        base.DoBounceBehavior(wave, highest, maker);
        turn = true;
    }

    private void Update() {
        if (turn) {
            transform.RotateAround(transform.position, AxisToV3(turnAxis), turnSpeed * Time.deltaTime);
        }
    }

    private Vector3 AxisToV3(RotateAxis ax) {
        switch (ax) {
            case RotateAxis.Forward: return transform.forward;
            case RotateAxis.Right: return transform.right;
            case RotateAxis.Up: return transform.up;
        }
        return new Vector3();
    }

    public enum RotateAxis {
        Forward = 0,
        Up = 1,
        Right = 2
    }
}