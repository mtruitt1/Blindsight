using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager local;
    public SoundWave soundSphere;
    public Rigidbody rock;
    public StrikingObject footStep;
    public float strikeMult = 1f;

    private void Awake() {
        local = this;
    }

    public static void SpawnSound(float str, Vector3 position, bool strike, bool reg, SoundListener maker) {
        if (strike) {
            Instantiate(local.soundSphere, position, Quaternion.identity).Emit(str * local.strikeMult, reg, maker);
        } else {
            Instantiate(local.soundSphere, position, Quaternion.identity).Emit(str, reg, maker);
        }
    }
}