using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles all ground collision stuff to make the bipedwalker script simpler
public class MoveBall : MonoBehaviour {
    public bool grounded { get; private set; }
    public Vector3 airSpeed { get; private set; } = new Vector3();
    private bool onGround = true;
    private float unGroundedTime = 0f;

    //set onground, the "fake" grounded variable to true when colliding
    private void OnCollisionEnter(Collision collision) {
        onGround = true;
    }

    //set onground, the "fake" grounded variable to false when no longer colliding
    private void OnCollisionExit(Collision collision) {
        onGround = false;
        airSpeed = GetComponent<Rigidbody>().velocity;
    }

    //set onground, the "fake" grounded variable to true when colliding
    private void OnCollisionStay(Collision collision) {
        onGround = true;
    }

    //count down the ungrounded timer, which determines if a biped has been off the ground for long enough to be considered "in the air" or falling
    private void Update() {
        if (GameManager.local.state == GameManager.GameState.Paused) {
            return;
        }
        if (!onGround) {
            unGroundedTime += Time.deltaTime;
            if (unGroundedTime >= GameManager.local.moveBallFallDetect && grounded) {
                grounded = false;
            }
        } else {
            grounded = true;
            unGroundedTime = 0f;
        }
    }
}