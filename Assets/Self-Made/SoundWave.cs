using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWave : MonoBehaviour {
    public float speed = 5f;
    public float fadeExponent = 1f;
    public float strength { get; private set; }
    public float maxStrength { get; private set; }
    private Material mat;

    public void Emit(float str) {
        maxStrength = str;
        strength = str;
        mat = GetComponent<Renderer>().material;
    }

    private void Update() {
        strength -= Time.deltaTime;
        if (strength <= 0f) {
            Destroy(gameObject);
        } else {
            float t = Mathf.InverseLerp(maxStrength, 0f, strength);
            transform.localScale = new Vector3(1, 1, 1) * t * maxStrength * speed;
            mat.SetFloat("Vector1_AA7E86CB", Mathf.Pow(1 - t, fadeExponent));
        }
    }
}