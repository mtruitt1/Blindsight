using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;

public class MainMenu : MonoBehaviour {
    public ButtonManager playButton;
    public Transform heartBeatSpawn;
    public GameObject credits;
    private int loadLevelButton = 0;
    public ButtonManager levelButton;
    public Button lastLev;
    public Button nextLev;

    private void Start() {
        Cursor.lockState = CursorLockMode.None;
        if (PlayerPrefs.HasKey("HighestLevel")) {
            if (PlayerPrefs.GetInt("HighestLevel") >= 0) {
                playButton.buttonText = "CONTINUE";
            }
            if (PlayerPrefs.GetInt("HighestLevel") >= GameManager.local.levels.Count - 1) {
                playButton.buttonText = "RESTART";
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
        levelButton.buttonText = "LOAD: " + GameManager.local.levels[loadLevelButton].Name;
        levelButton.UpdateUI();
        if (loadLevelButton == 0) {
            lastLev.interactable = false;
        } else {
            lastLev.interactable = true;
        }
        if (loadLevelButton == GameManager.local.levels.Count - 1 || loadLevelButton >= PlayerPrefs.GetInt("HighestLevel")) {
            nextLev.interactable = false;
        } else {
            nextLev.interactable = true;
        }
    }

    public void Play() {
        if (PlayerPrefs.GetInt("HighestLevel") >= GameManager.local.levels.Count - 1) {
            PlayerPrefs.SetInt("HighestLevel", -1);
        }
        GameManager.local.LoadHighestPlayable();
    }

    public void LoadLevel() {
        GameManager.local.LoadScene(loadLevelButton);
    }

    public void DecreaseLevel() {
        loadLevelButton--;
        //loadLevelButton = Mathf.Clamp(Mathf.Clamp(loadLevelButton--, 0, GameManager.local.levels.Count - 1), 0, PlayerPrefs.GetInt("HighestLevel") + 1);
    }

    public void IncreaseLevel() {
        loadLevelButton++;
        //loadLevelButton = Mathf.Clamp(Mathf.Clamp(loadLevelButton++, 0, GameManager.local.levels.Count - 1), 0, PlayerPrefs.GetInt("HighestLevel") + 1);
    }

    public void Quit() {
        Application.Quit();
    }

    public void ToggleCredits() {
        credits.SetActive(!credits.activeInHierarchy);
    }

    public void PlayHover() {
        AudioSource.PlayClipAtPoint(GameManager.local.buttonHover, Camera.main.transform.position, GameManager.local.UIVolume);
    }

    public void PlayClick() {
        AudioSource.PlayClipAtPoint(GameManager.local.buttonClick, Camera.main.transform.position, GameManager.local.UIVolume);
    }
}