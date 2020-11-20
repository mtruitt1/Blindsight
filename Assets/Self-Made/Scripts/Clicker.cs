using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the class used to generate the snapping sound
//originally I intended for the sound to be a click sound like a bottle cap, but decided against it
public class Clicker : SoundObject {
    public float strength = 10f;
    public float coolDown = 0.25f;
    private float timer = 0f;

    //decreases the cooldown, snaps when the timer is less than 0 and the left mouse button is clicked
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