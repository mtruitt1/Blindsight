using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {
    public BipedWalker walker;

    private void Update() {
        walker.forwardGoal = Input.GetAxis("Vertical");
        walker.rightGoal = Input.GetAxis("Horizontal");
        if (walker.grounded) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                walker.jump = true;
                Debug.Log("Jumping");
            }
            if (Input.GetKey(KeyCode.Q)) {
                walker.turnGoal = -1f;
            } else {
                walker.turnGoal = 0f;
            }
            if (Input.GetKey(KeyCode.E)) {
                walker.turnGoal = 1f;
            }
            if (!Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E)) {
                walker.turnGoal = 0f;
            }
            if (Input.GetKey(KeyCode.C)) {
                walker.crouch = true;
            } else {
                walker.crouch = false;
            }
            if (Input.GetKey(KeyCode.LeftShift)) {
                walker.sprint = true;
            } else {
                walker.sprint = false;
            }
        }
    }
}