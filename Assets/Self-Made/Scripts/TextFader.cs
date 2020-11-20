using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//makes it easy to fade text in for tutorial purposes
//technically could also be used to fade it out, but has not been used for that yet
public class TextFader : MonoBehaviour {
    public bool start = true;
    public bool enter = false;
    public bool exit = false;
    private bool state;
    public float fadeSpeed = 5f;
    public List<MeshRenderer> texts = new List<MeshRenderer>();
    private Dictionary<MeshRenderer, Color> textsColors = new Dictionary<MeshRenderer, Color>();

    //set the state to the start state, then gets and sets the color of each of the text mesh pro renderers fed in
    private void Start() {
        state = start;
        foreach (MeshRenderer text in texts) {
            textsColors.Add(text, text.materials[0].GetColor("_FaceColor"));
            text.materials[0].SetColor("_FaceColor", Color.Lerp(new Color(0f, 0f, 0f), textsColors[text], state ? 1f : 0f));
        }
    }

    //if the game isn't paused, update the color of text based on the state
    private void Update() {
        if (GameManager.local.state == GameManager.GameState.Paused) {
            return;
        }
        foreach (KeyValuePair<MeshRenderer, Color> kvp in textsColors) {
            Vector3 current = ColorToV3(kvp.Key.materials[0].GetColor("_FaceColor"));
            kvp.Key.materials[0].SetColor("_FaceColor", V3ToColor(Vector3.MoveTowards(current, state ? new Vector3() : ColorToV3(kvp.Value), Time.deltaTime * fadeSpeed)));
        }
    }

    //set the state to the enter state
    private void OnTriggerEnter(Collider other) {
        state = enter;
    }

    //set the state to the exit state
    private void OnTriggerExit(Collider other) {
        state = exit;
    }

    //converts RGB to XYZ to be able to use Vector3 functions
    private Vector3 ColorToV3(Color color) {
        return new Vector3(color.r, color.g, color.b);
    }

    //converts XYZ to RGB to actually pass back to the material
    private Color V3ToColor(Vector3 vector) {
        return new Color(vector.x, vector.y, vector.z);
    }
}