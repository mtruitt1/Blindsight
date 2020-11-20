using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the class responsible for ending the level and moving onto the next one
//there should only be one of these per level
public class ExitDoor : SoundObject {
    public static ExitDoor local;
    public int levelNum = -2;
    public float doorSpeed = 5f;
    public Transform posDoor;
    public Transform negDoor;
    private bool open = false;
    public Vector3 endEulers;
    private Vector3 startEulers;
    private float doorStrength = 10f;

    //do any soundobject start stuff, then set ExitDoor.local to this for easy access to the level number
    protected override void Start() {
        base.Start();
        local = this;
    }

    //when the player enters the trigger, increase the highest level number and make the door open
    private void OnTriggerEnter(Collider other) {
        if (!open) {
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
    }

    //actually swing the doors open, and when they're fully open, switch the scene to the next one, or the menu if no higher level exists
    private void Update() {
        if (GameManager.local.state == GameManager.GameState.Paused) {
            return;
        }
        if (open) {
            GameManager.local.cover = true;
            if (posDoor.localEulerAngles == startEulers && doorStrength > 0) {
                PlayRandSound(doorStrength, false, true);
                doorStrength = 0f;
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