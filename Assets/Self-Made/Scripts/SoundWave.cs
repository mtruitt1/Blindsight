using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the actual soundwave, a sphere which expands outwards until it dies out
public class SoundWave : MonoBehaviour {
    public float speed = 5f;
    public float fadeExponent = 1f;
    public bool regularSound { get; private set; } = false;
    public float strength { get; private set; }
    public float visStrength { get; private set; }
    public float maxStrength { get; private set; }
    public SoundObject origin { get; private set; }
    public bool followOrigin = false;
    private Material mat;
    private float strengthLossMult = 1f;
    public bool suppressable { get; protected set; }

    //the emit function is like a constructor for the class, but not exactly
    public SoundWave Emit(float str, bool reg, SoundObject maker, bool suppress = true) {
        name = "SoundWave(" + maker.name + ")";
        maxStrength = str;
        strength = str;
        visStrength = str;
        regularSound = reg;
        origin = maker;
        mat = GetComponent<Renderer>().material;
        transform.position = maker.transform.position;
        suppressable = suppress;
        foreach (Collider col in Physics.OverlapSphere(transform.position, 0.01f)) {
            if (col.TryGetComponent(out SoundSuppressor suppressor)) {
                if (suppressor.suppress && suppressable) {
                    strength = 0f;
                    visStrength = 0f;
                }
            }
        }
        return this;
    }

    //expand and fade the intensity of the light effect, only when the game is unpaused
    private void Update() {
        if (GameManager.local.state == GameManager.GameState.Paused) {
            return;
        }
        strength -= Time.deltaTime;
        visStrength -= Time.deltaTime * strengthLossMult;
        if (strength <= 0f || visStrength <= 0f) {
            Destroy(gameObject);
        } else {
            transform.localScale = new Vector3(1, 1, 1) * Mathf.InverseLerp(maxStrength, 0f, strength) * maxStrength * speed;
            mat.SetFloat("Vector1_AA7E86CB", Mathf.Pow(1 - Mathf.InverseLerp(maxStrength, 0f, visStrength), fadeExponent));
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

    public void SuppressSound() {
        if (suppressable) {
            strengthLossMult = strength / 0.25f;
        }
    }
}