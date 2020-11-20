using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//bounces sound from the onlyfrom, or from anything if onlyfrom is null
//mainly used as a class to inherit from
public class SoundBouncer : SoundReceiver {
    public SoundObject onlyFrom;
    public float strength;

    //react to a sound being made
    protected override void SoundReact(SoundWave wave, SoundObject highest, SoundObject maker) {
        base.SoundReact(wave, highest, maker);
        //Debug.Log("Addressing bounce on " + name);
        if (wave.origin.GetParentList().Contains(onlyFrom) || onlyFrom == null) {
            //Debug.Log("Confirmed bounce on " + name);
            DoBounceBehavior(wave, highest, maker);
        }
    }

    //play a sound, can be overridden
    protected virtual void DoBounceBehavior(SoundWave wave, SoundObject highest, SoundObject maker) {
        PlayRandSound(strength, false, true);
    }
}