using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : SoundObject {
    public int levelNum = -2;
    public float doorSpeed = 5f;
    public Transform posDoor;
    public Transform negDoor;
    private bool open = false;
    public Vector3 endEulers;
    private float timer = 0f;
    private Vector3 startEulers;
    private float doorStrength = 10f;

    private void OnTriggerEnter(Collider other) {
        open = true;
        if (PlayerPrefs.HasKey("HighestLevel")) {
            if (levelNum > PlayerPrefs.GetInt("HighestLevel")) {
                PlayerPrefs.SetInt("HighestLevel", levelNum);
            }
        } else {
            PlayerPrefs.SetInt("HighestLevel", -1);
        }
        startEulers = posDoor.localEulerAngles;
    }

    private void Update() {
        if (GameManager.local.state == GameManager.GameState.Paused) {
            return;
        }
        if (open) {
            if (posDoor.localEulerAngles == startEulers) {
                PlayRandSound(doorStrength, false, true);
            }
            if (posDoor.localEulerAngles != endEulers) {
                posDoor.localEulerAngles = Vector3.MoveTowards(posDoor.localEulerAngles, endEulers, Time.deltaTime * doorSpeed);
                negDoor.localEulerAngles = posDoor.localEulerAngles * -1f;
            }
            if (posDoor.localEulerAngles == endEulers) {
                if (levelNum == PlayerPrefs.GetInt("HighestLevel")) {
                    if (!GameManager.local.LoadScene(levelNum + 1)) {
                        GameManager.local.LoadMenu();
                    }
                } else {
                    GameManager.local.LoadMenu();
                }
            }
        }
    }
}