using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the lever which the player can interact with to open metal doors
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
    public float bounceStrength;
    public AudioClip flipSound;
    private bool inRange = false;
    private bool state = false;

    //turn the indicator off, get the starting euler angles, and set the flip timer
    protected override void Start() {
        base.Start();
        indicator.gameObject.SetActive(false);
        startEulers = rotate.localEulerAngles;
        flipTimer = flipSpeed;
    }

    //react to a sound and bounce it if not flipped
    public override void DoBounceBehavior(SoundWave wave, SoundObject highest, SoundObject maker) {
        if (!flip && !inRange) {
            PlayRandSound(bounceStrength, false, true);
        }
    }

    //flip the switch, or otherwise pulse when the player is nearby until they press E
    private void Update() {
        if (player == null) {
            player = PlayerControls.local.GetComponent<BipedWalker>();
        }
        if (Vector3.Distance(player.transform.position, transform.position) <= activationDistance) {
            inRange = true;
            indicator.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E)) {
                flip = true;
                state = !state;
            }
        } else {
            inRange = false;
            indicator.gameObject.SetActive(false);
        }
        if (flip) {
            flipTimer = Mathf.Clamp(flipTimer + (state ? -Time.deltaTime : Time.deltaTime), 0f, flipSpeed);
            rotate.localEulerAngles = Vector3.Lerp(endEulers, startEulers, flipTimer / flipSpeed);
            indicator.gameObject.SetActive(false);
            if (flipTimer == 0f || Mathf.Abs(flipTimer - flipSpeed) < 0.0001f) {
                flip = false;
                PlaySound(flipSound, activationStrength, false, false);
            }
        }
    }
}