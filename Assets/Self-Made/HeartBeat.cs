using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeat : MonoBehaviour {
    public float strength = 0.5f;
    public int heartRate = 70;
    private float bps = 0f;

    private void Update() {
        bps -= Time.deltaTime;
        if (bps <= 0f) {
            GameManager.SpawnSound(strength, transform.position, false, true, null);
            bps = 1 / (heartRate / 60);
        }
    }
}