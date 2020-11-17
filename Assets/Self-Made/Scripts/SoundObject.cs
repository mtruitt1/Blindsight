using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour {
    public bool pullParent = false;
    public float volMult = 1f;
    public List<AudioClip> sounds;
    public SoundObject owner = null;
    protected bool finishedInit = false;

    protected virtual void Start() {
        if (sounds.Count < 1 && owner != null && pullParent) {
            sounds = owner.sounds;
            volMult = owner.volMult;
        }
        if (owner == null) {
            owner = this;
        }
        finishedInit = true;
    }

    protected void PlayRandSound(float strength, bool strike, bool reg) {
        if (volMult > 0 && finishedInit) {
            try {
                AudioSource.PlayClipAtPoint(sounds[Random.Range(0, sounds.Count)], transform.position, strength * volMult);
            } catch {
                Debug.LogError("No sounds assigned on " + gameObject.name);
            }
        }
        GameManager.SpawnSound(strength, transform.position, strike, reg, this);
    }

    public List<SoundObject> GetParentList() {
        List<SoundObject> objects = new List<SoundObject>();
        if (owner != null && owner != this) {
            objects.AddRange(owner.GetParentList());
        }
        objects.Add(this);
        return objects;
    }
}