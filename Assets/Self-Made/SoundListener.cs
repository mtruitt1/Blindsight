using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundListener : MonoBehaviour {
    public BipedWalker walker;
    public float strengthMinRaw = 0f;
    public float strengthMinPercent = 0f;

    public void SoundTouched(SoundWave wave) {
        if (wave.strength >= strengthMinRaw && (wave.strength / wave.maxStrength) >= strengthMinPercent) {

        }
    }
}