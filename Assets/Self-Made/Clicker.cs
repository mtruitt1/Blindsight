using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour {
    public float strength = 10f;
    public float coolDown = 0.25f;
    private float timer = 0f;

    private void Update() {
        timer -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && timer <= 0f) {
            timer = coolDown;
            GameManager.SpawnSound(strength, transform.position, false, false, null);
        }
    }
}