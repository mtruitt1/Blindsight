using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour {
    public int levelNum = -2;
    public float doorSpeed = 5f;
    public List<Door> doors;
    private bool open = false;

    private void OnTriggerEnter(Collider other) {
        open = true;
        if (PlayerPrefs.HasKey("HighestLevel")) {
            if (levelNum > PlayerPrefs.GetInt("HighestLevel")) {
                PlayerPrefs.SetInt("HighestLevel", levelNum);
            }
        } else {
            PlayerPrefs.SetInt("HighestLevel", -1);
        }
    }

    private void Update() {
        if (GameManager.local.state == GameManager.GameState.Paused) {
            return;
        }
        if (open) {
            foreach (Door door in doors) {
                if (door.transform.localEulerAngles != door.endEulers) {
                    door.transform.localEulerAngles = Vector3.MoveTowards(door.transform.localEulerAngles, door.endEulers, Time.deltaTime * doorSpeed);
                }
                if (door.transform.localEulerAngles == door.endEulers) {
                    try {
                        if (levelNum == PlayerPrefs.GetInt("HighestLevel")) {
                            GameManager.local.LoadScene(levelNum + 1);
                        } else {
                            GameManager.local.LoadMenu();
                        }
                    } catch {
                        Debug.LogWarning("Levels not implemented past " + levelNum);
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class Door {
        public Transform transform;
        public Vector3 endEulers;
    }
}