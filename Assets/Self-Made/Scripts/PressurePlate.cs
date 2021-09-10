using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : SoundBouncer {
    public AudioClip stepOn;
    public AudioClip stepOff;
    public float depressionTime = 0.5f;
    public float pressUnpressSpeed = 5f;
    public Transform plate;
    public Vector3 positionOffset;
    public bool forcePress = false;
    private Vector3 plateStart;
    private List<MoveBall> currentlyOnPlate = new List<MoveBall>();
    private float holdTime = 0f;
    private float lastChangeTime = 0f;
    public SoundBouncer manuallyControl;
    public HeartBeat heldIndicator;

    protected override void Start() {
        base.Start();
        plateStart = plate.transform.localPosition;
        heldIndicator.gameObject.SetActive(false);
    }

    public override void DoBounceBehavior(SoundWave wave, SoundObject highest, SoundObject maker) {
        if (currentlyOnPlate.Count == 0) {
            PlayRandSound(strength, false, true);
        }
    }

    private MoveBall CheckColliderForMoveBall(Collider toCheck) {
        MoveBall returnBall = null;
        if (toCheck.TryGetComponent(out MoveBall ball)) {
            returnBall = ball;
        }
        if (toCheck.TryGetComponent(out BipedWalker walker)) {
            returnBall = walker.moveBall;
        }
        if (returnBall != null) {
            if (Time.time - lastChangeTime < 0.1f) {
                return null;
            }
            lastChangeTime = Time.time;
        }
        return returnBall;
    }

    private void OnTriggerEnter(Collider other) {
        MoveBall ball = CheckColliderForMoveBall(other);
        if (ball == null) {
            return;
        }
        if (!currentlyOnPlate.Contains(ball)) {
            currentlyOnPlate.Add(ball);
        }
        if (currentlyOnPlate.Count == 1 && holdTime <= 0f) {
            PlaySound(stepOn, strength, false, false);
        }
    }

    private void OnTriggerExit(Collider other) {
        MoveBall ball = CheckColliderForMoveBall(other);
        if (ball == null) {
            return;
        }
        if (currentlyOnPlate.Contains(ball)) {
            currentlyOnPlate.Remove(ball);
        }
    }

    private void Update() {
        if (currentlyOnPlate.Count > 0 || forcePress) {
            holdTime = depressionTime;
            heldIndicator.gameObject.SetActive(true);
            if (!manuallyControl.toggleable) {
                manuallyControl.DoBounceBehavior(null, null, null);
            }
        } else {
            heldIndicator.gameObject.SetActive(false);
            holdTime = holdTime > 0 ? Mathf.Clamp(holdTime - Time.deltaTime, 0f, Mathf.Infinity) : holdTime -= Time.deltaTime;
            if (currentlyOnPlate.Count == 0 && holdTime == 0f) {
                PlaySound(stepOff, strength, false, false);
                if (manuallyControl.toggleable) {
                    manuallyControl.DoBounceBehavior(null, null, null);
                }
            }
        }
        plate.transform.localPosition = Vector3.MoveTowards(plate.transform.localPosition, holdTime <= 0f ? plateStart : plateStart + positionOffset, pressUnpressSpeed);
    }
}