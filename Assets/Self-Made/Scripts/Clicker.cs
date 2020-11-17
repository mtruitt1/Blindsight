using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : SoundObject {
    public float strength = 10f;
    public float coolDown = 0.25f;
    private float timer = 0f;

    private void Update() {
        if (GameManager.local.state == GameManager.GameState.Paused) {
            return;
        }
        timer -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && timer <= 0f) {
            timer = coolDown;
            PlayRandSound(strength, false, false);
        }
    }
}