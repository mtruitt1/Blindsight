using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBouncer : SoundReceiver {
    public SoundObject onlyFrom;
    public float strength;

    protected override void SoundReact(SoundWave wave, SoundObject highest, SoundObject maker) {
        base.SoundReact(wave, highest, maker);
        //Debug.Log("Addressing bounce on " + name);
        if (wave.origin.GetParentList().Contains(onlyFrom) || onlyFrom == null) {
            //Debug.Log("Confirmed bounce on " + name);
            DoBounceBehavior(wave, highest, maker);
        }
    }

    protected virtual void DoBounceBehavior(SoundWave wave, SoundObject highest, SoundObject maker) {
        PlayRandSound(strength, false, true);
    }
}