using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the actual soundwave, a sphere which expands outwards until it dies out
public class SoundWave : MonoBehaviour {
    public float speed = 5f;
    public float fadeExponent = 1f;
    public bool regularSound { get; private set; } = false;
    public float strength { get; private set; }
    public float maxStrength { get; private set; }
    public SoundObject origin { get; private set; }
    public bool followOrigin = false;
    private Material mat;

    //the emit function is like a constructor for the class, but not exactly
    public SoundWave Emit(float str, bool reg, SoundObject maker) {
        name = "SoundWave(" + maker.name + ")";
        maxStrength = str;
        strength = str;
        regularSound = reg;
        origin = maker;
        mat = GetComponent<Renderer>().material;
        transform.position = maker.transform.position;
        return this;
    }

    //expand and fade the intensity of the light effect, only when the game is unpaused
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
            if (followOrigin) {
                transform.position = origin.transform.position;
            }
        }
    }

    //whenever this soundwave hits something, it should try to pass the sound on to it if it can
    private void OnTriggerEnter(Collider other) {
        //Debug.Log("Hit " + other.gameObject.name + " but didn't give sound");
        if (other.TryGetComponent(out SoundReceiver listener)) {
            //Debug.Log("Giving sound to " + other.gameObject.name);
            listener.SoundTouched(this);
        }
    }
}