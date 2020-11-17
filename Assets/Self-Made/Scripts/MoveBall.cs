using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour {
    public bool grounded { get; private set; }
    public Vector3 airSpeed { get; private set; } = new Vector3();
    private bool onGround = true;
    private float unGroundedTime = 0f;

    private void OnCollisionEnter(Collision collision) {
        onGround = true;
    }

    private void OnCollisionExit(Collision collision) {
        onGround = false;
        airSpeed = GetComponent<Rigidbody>().velocity;
    }

    private void OnCollisionStay(Collision collision) {
        onGround = true;
    }

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