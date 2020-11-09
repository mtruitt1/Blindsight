using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrower : MonoBehaviour {
    public Rigidbody rock;
    public float coolDown = 0.5f;
    public Transform rockSpawn;
    public float rockSpeed = 10f;
    private float timer;

    private void Update() {
        timer -= Time.deltaTime;
        if (Input.GetMouseButtonDown(1) && timer <= 0f) {
            timer = coolDown;
            Rigidbody rb = Instantiate(rock, rockSpawn.position, Quaternion.identity);
            rb.velocity = rockSpeed * rockSpawn.forward;
        }
    }
}