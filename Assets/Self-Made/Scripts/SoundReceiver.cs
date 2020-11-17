using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundReceiver : SoundObject {
    public bool ignoreReg = true;
    public float strengthMinRaw = 0f;
    public float strengthMinPercent = 0f;
    protected HeardSound lastHeard = null;

    public void SoundTouched(SoundWave wave) {
        List<SoundObject> objects = wave.origin?.GetParentList();
        SoundObject highestParent = objects?[0];
        //Debug.Log("Received sound on " + name);
        if (wave.strength >= strengthMinRaw && (wave.strength / wave.maxStrength) >= strengthMinPercent && !(ignoreReg && wave.regularSound) && !objects.Contains(this)) {
            //Debug.Log("Sound validated on " + name);
            SoundReact(wave, highestParent, wave.origin);
        }
    }

    protected virtual void SoundReact(SoundWave wave, SoundObject highest, SoundObject maker) {
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