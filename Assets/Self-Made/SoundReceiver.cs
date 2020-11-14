using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundReceiver : SoundObject {
    public bool ignoreReg = true;
    public float strengthMinRaw = 0f;
    public float strengthMinPercent = 0f;
    protected HeardSound lastHeard = null;

    public void SoundTouched(SoundWave wave) {
        if (wave.strength >= strengthMinRaw && (wave.strength / wave.maxStrength) >= strengthMinPercent && (!ignoreReg || wave.regularSound) && wave.origin != this && wave.origin?.tag != "Enemy") {
            SoundReact(wave);
        }
    }

    protected virtual void SoundReact(SoundWave wave) {
        
    }

    public class HeardSound {
        public float strength;
        public float percent;
        public Vector3 position;

        public HeardSound(float s, float p, Vector3 pos) {
            strength = s;
            percent = p;
            position = pos;
        }
    }
}