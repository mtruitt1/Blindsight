using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : SoundBouncer {
    public float activationDistance = 1f;
    public Transform rotate;
    public float flipSpeed = 0.5f;
    private Vector3 startEulers;
    public Vector3 endEulers;
    public HeartBeat indicator;
    public BipedWalker player;
    private bool flip = false;
    private float flipTimer;
    public float activationStrength;
    public float activationVol = 1f;
    public float bounceStrength;

    protected override void Start() {
        base.Start();
        indicator.gameObject.SetActive(false);
        startEulers = rotate.localEulerAngles;
        flipTimer = flipSpeed;
    }

    protected override void DoBounceBehavior(SoundWave wave, SoundObject highest, SoundObject maker) {
        if (!flip) {
            volMult = 0f;
            PlayRandSound(bounceStrength, false, true);
        }
    }

    private void Update() {
        if (flip) {
            flipTimer -= Time.deltaTime;
            indicator.gameObject.SetActive(false);
            rotate.localEulerAngles = Vector3.Lerp(endEulers, startEulers, flipTimer / flipSpeed);
            if (flipTimer <= 0f && flipSpeed > 0f) {
                volMult = activationVol;
                PlayRandSound(activationStrength, false, false);
                flipSpeed = 0f;
            }
        } else {
            if (Vector3.Distance(player.transform.position, transform.position) <= activationDistance) {
                indicator.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E)) {
                    flip = true;
                }
            } else {
                indicator.gameObject.SetActive(false);
            }
        }
    }
}