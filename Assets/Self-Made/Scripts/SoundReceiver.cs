using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the base class for anything which should receive and react to sound
//the soundwave class looks for a component of this type, or something that inherits from it, in order to pass sounds along
public class SoundReceiver : SoundObject {
    public bool ignoreReg = true;
    public float strengthMinRaw = 0f;
    public float strengthMinPercent = 0f;
    public float soundReactCooldown = 0.1f;
    protected HeardSound lastHeard = null;
    protected float lastHeardTime = 0f;

    //gets run by the soundwave, does any preliminary checking the soundreceiver should do before passing the information along to the next function in the queue for the object or inherited object
    public void SoundTouched(SoundWave wave) {
        List<SoundObject> objects = wave.origin?.GetParentList();
        SoundObject highestParent = objects?[0];
        //Debug.Log("Received sound on " + name);
        if (wave.strength >= strengthMinRaw && (wave.strength / wave.maxStrength) >= strengthMinPercent && !(ignoreReg && wave.regularSound) && !objects.Contains(this)) {
            //Debug.Log("Sound validated on " + name);
            SoundReact(wave, highestParent, wave.origin);
        }
    }

    //empty for any inheriting classes to fill in as needed
    protected virtual void SoundReact(SoundWave wave, SoundObject highest, SoundObject maker) {
    }

    //allows semipermanent storing of sounds even though the soundwave class isn't permanent
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