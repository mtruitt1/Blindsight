using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBouncer : SoundReceiver {
    public SoundReceiver onlyFrom;
    public float strength;

    protected override void SoundReact(SoundWave wave) {
        base.SoundReact(wave);
        if (wave.origin == onlyFrom) {
            PlayRandSound(strength, false, true);
        }
    }
}