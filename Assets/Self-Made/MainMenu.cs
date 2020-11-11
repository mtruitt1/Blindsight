using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;

public class MainMenu : MonoBehaviour {
    public ButtonManager playButton;
    public Transform heartBeatSpawn;

    private void Start() {
        if (PlayerPrefs.HasKey("HighestLevel")) {
            if (PlayerPrefs.GetInt("HighestLevel") > 0) {
                playButton.buttonText = "CONTINUE";
            }
        } else {
            PlayerPrefs.SetInt("HighestLevel", -1);
        }
    }

    private void Update() {
        foreach (Transform child in heartBeatSpawn) {
            child.parent = null;
        }
        Vector3 mouseIn = Input.mousePosition;
        mouseIn.z = 5;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseIn);
        heartBeatSpawn.position = new Vector3(mouseWorld.x, mouseWorld.y, heartBeatSpawn.position.z);
    }

    public void Play() {
        GameManager.local.LoadScene(PlayerPrefs.GetInt("HighestLevel") + 1);
    }

    public void Tutorial() {
        GameManager.local.LoadScene(0);
    }

    public void Restart() {
        PlayerPrefs.SetInt("HighestLevel", 0);
        Play();
    }

    public void Quit() {
        Application.Quit();
    }
}