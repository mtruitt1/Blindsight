using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//bounces sound from the onlyfrom, or from anything if onlyfrom is null
//mainly used as a class to inherit from
public class SoundBouncer : SoundReceiver {
    public bool manualOnly = false;
    public List<SoundObject> bouncables = new List<SoundObject>();
    public float strength;
    public bool toggleable { get; protected set; } = false;

    //react to a sound being made
    protected override void SoundReact(SoundWave wave, SoundObject highest, SoundObject maker) {
        base.SoundReact(wave, highest, maker);
        if (Time.time - lastHeardTime < soundReactCooldown || manualOnly) {
            return;
        }
        //Debug.Log("Addressing bounce on " + name);
        bool doBounce = bouncables.Count == 0;
        if (!doBounce) {
            foreach (SoundObject bouncable in bouncables) {
                if (wave.origin.GetParentList().Contains(bouncable)) {
                    //Debug.Log("Confirmed bounce on " + name);
                    doBounce = true;
                    break;
                }
            }
        }
        if (doBounce) {
            DoBounceBehavior(wave, highest, maker);
        }
    }

    //play a sound, can be overridden
    public virtual void DoBounceBehavior(SoundWave wave, SoundObject highest, SoundObject maker) {
        PlayRandSound(strength, false, true);
    }
}