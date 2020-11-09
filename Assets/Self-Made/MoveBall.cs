using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour {
    public bool grounded { get; private set; }
    public Vector3 airSpeed { get; private set; } = new Vector3();

    private void OnCollisionEnter(Collision collision) {
        grounded = true;
    }

    private void OnCollisionExit(Collision collision) {
        grounded = false;
        airSpeed = GetComponent<Rigidbody>().velocity;
    }

    private void OnCollisionStay(Collision collision) {
        grounded = true;
    }
}