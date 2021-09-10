using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSuppressor : SoundBouncer {
    public bool suppress { get; private set; } = true;
    protected AudioSource staticProducer;
    public AudioClip destructionSound;

    protected override void Start() {
        base.Start();
        staticProducer = GetComponent<AudioSource>();
    }

    protected override void SoundReact(SoundWave wave, SoundObject highest, SoundObject maker) {
        base.SoundReact(wave, highest, maker);
        if (suppress) {
            wave.SuppressSound();
        }
    }

    public override void DoBounceBehavior(SoundWave wave, SoundObject highest, SoundObject maker) {
        if (bouncables.Count > 0 && suppress) {
            suppress = false;
            PlayRandSound(strength, false, false);
        }
    }

    private void Update() {
        if (staticProducer == null) {
            return;
        }
        if (suppress && !staticProducer.isPlaying) {
            staticProducer.Play();
        } else if (!suppress && staticProducer.isPlaying) {
            staticProducer.Stop();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.rigidbody.tag == "Rock" && destructionSound != null && suppress) {
            suppress = false;
            staticProducer.Stop();
            staticProducer = null;
            PlaySound(destructionSound, strength, false, false);
        }
    }
}