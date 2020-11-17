using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeat : SoundObject {
    public float strength = 0.5f;
    public int heartRate = 70;
    private float bps = 0f;
    public bool followOrigin = true;

    private void Update() {
        if (GameManager.local.state == GameManager.GameState.Paused) {
            return;
        }
        bps -= Time.deltaTime;
        if (bps <= 0f) {
            if (sounds.Count > 0) {
                AudioSource.PlayClipAtPoint(sounds[Random.Range(0, sounds.Count)], transform.position, strength * volMult);
            }
            SoundWave wave = Instantiate(GameManager.local.soundSphere).Emit(strength, true, this);
            wave.followOrigin = followOrigin;
            bps = 1f / (heartRate / 60f);
        }
    }
}