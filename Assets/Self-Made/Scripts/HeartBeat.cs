using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the heartbeat class which spawns periodic sounds
//this is the only class which actually spawns in sounds which follow their spawner
public class HeartBeat : SoundObject {
    public float strength = 0.5f;
    public int heartRate = 70;
    private float bps = 0f;
    public bool followOrigin = true;

    //count down the timer, spawn sounds when appropriate
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