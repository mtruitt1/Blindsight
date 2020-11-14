using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour {
    public float volMult = 1f;
    public List<AudioClip> sounds;
    public SoundObject owner = null;

    private void Start() {
        if (sounds.Count < 1 && owner != null) {
            sounds = owner.sounds;
            volMult = owner.volMult;
        }
    }

    protected void PlayRandSound(float strength, bool strike, bool reg) {
        try {
            AudioSource.PlayClipAtPoint(sounds[Random.Range(0, sounds.Count)], transform.position, strength * volMult);
        } catch {
            Debug.LogError("No sounds assigned");
        }
        GameManager.SpawnSound(strength, transform.position, true, false, owner);
    }
}