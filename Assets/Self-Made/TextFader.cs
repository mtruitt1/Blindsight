using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFader : MonoBehaviour {
    public bool start = true;
    public bool enter = false;
    public bool exit = false;
    private bool state;
    public float fadeSpeed = 5f;
    public List<MeshRenderer> texts = new List<MeshRenderer>();
    private Dictionary<MeshRenderer, Color> textsColors = new Dictionary<MeshRenderer, Color>();

    private void Start() {
        state = start;
        foreach (MeshRenderer text in texts) {
            textsColors.Add(text, text.materials[0].GetColor("_FaceColor"));
            text.materials[0].SetColor("_FaceColor", Color.Lerp(new Color(0f, 0f, 0f), textsColors[text], state ? 1f : 0f));
        }
    }

    private void Update() {
        if (GameManager.local.state == GameManager.GameState.Paused) {
            return;
        }
        foreach (KeyValuePair<MeshRenderer, Color> kvp in textsColors) {
            Vector3 current = ColorToV3(kvp.Key.materials[0].GetColor("_FaceColor"));
            kvp.Key.materials[0].SetColor("_FaceColor", V3ToColor(Vector3.MoveTowards(current, state ? new Vector3() : ColorToV3(kvp.Value), Time.deltaTime * fadeSpeed)));
        }
    }

    private void OnTriggerEnter(Collider other) {
        state = enter;
    }

    private void OnTriggerExit(Collider other) {
        state = exit;
    }

    private Vector3 ColorToV3(Color color) {
        return new Vector3(color.r, color.g, color.b);
    }

    private Color V3ToColor(Vector3 vector) {
        
        return new Color(vector.x, vector.y, vector.z);
    }
}