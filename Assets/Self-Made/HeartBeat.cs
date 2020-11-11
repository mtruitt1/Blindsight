using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeat : MonoBehaviour {
    public float strength = 0.5f;
    public int heartRate = 70;
    private float bps = 0f;

    private void Update() {
        if (GameManager.local.state == GameManager.GameState.Paused) {
            return;
        }
        bps -= Time.deltaTime;
        if (bps <= 0f) {
            SoundWave wave = Instantiate(GameManager.local.soundSphere).Emit(strength, true, null);
            wave.transform.parent = transform;
            wave.transform.localPosition = new Vector3();
            bps = 1 / (heartRate / 60);
        }
    }
}