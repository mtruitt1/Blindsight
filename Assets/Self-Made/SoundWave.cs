using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWave : MonoBehaviour {
    public float speed = 5f;
    public float fadeExponent = 1f;
    public bool regularSound { get; private set; } = false;
    public float strength { get; private set; }
    public float maxStrength { get; private set; }
    public SoundObject origin { get; private set; }
    private Material mat;

    public SoundWave Emit(float str, bool reg, SoundObject maker) {
        maxStrength = str;
        strength = str;
        regularSound = reg;
        origin = maker;
        mat = GetComponent<Renderer>().material;
        return this;
    }

    private void Update() {
        if (GameManager.local.state == GameManager.GameState.Paused) {
            return;
        }
        strength -= Time.deltaTime;
        if (strength <= 0f) {
            Destroy(gameObject);
        } else {
            float t = Mathf.InverseLerp(maxStrength, 0f, strength);
            transform.localScale = new Vector3(1, 1, 1) * t * maxStrength * speed;
            mat.SetFloat("Vector1_AA7E86CB", Mathf.Pow(1 - t, fadeExponent));
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out SoundReceiver listener)) {
            listener.SoundTouched(this);
        }
    }
}