using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;

//the main menu class, where all of the menu stuff happens
//this class is mostly public void functions for easy "OnClick()" callbacks
public class MainMenu : MonoBehaviour {
    public ButtonManager playButton;
    public Transform heartBeatSpawn;
    public GameObject credits;
    private int loadLevelButton = 0;
    public ButtonManager levelButton;
    public Button lastLev;
    public Button nextLev;

    //do some basic game setup stuff, make sure the values for highest level are there. update the top button text if they are
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

    //make sure the load level button is displaying the right name and disable/enable the arrows depending on the state
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

    //play the highest level, or restart if past the highest level
    public void Play() {
        if (PlayerPrefs.GetInt("HighestLevel") >= GameManager.local.levels.Count - 1) {
            PlayerPrefs.SetInt("HighestLevel", -1);
        }
        GameManager.local.LoadHighestPlayable();
    }

    //loads a level based on the load level button
    public void LoadLevel() {
        GameManager.local.LoadScene(loadLevelButton);
    }

    //decreases the load level button's selected level
    public void DecreaseLevel() {
        loadLevelButton--;
        //loadLevelButton = Mathf.Clamp(Mathf.Clamp(loadLevelButton--, 0, GameManager.local.levels.Count - 1), 0, PlayerPrefs.GetInt("HighestLevel") + 1);
    }

    //increases the load level button's selected level
    public void IncreaseLevel() {
        loadLevelButton++;
        //loadLevelButton = Mathf.Clamp(Mathf.Clamp(loadLevelButton++, 0, GameManager.local.levels.Count - 1), 0, PlayerPrefs.GetInt("HighestLevel") + 1);
    }

    //quits the game
    public void Quit() {
        Application.Quit();
    }

    //toggles the credits and licensing menu
    public void ToggleCredits() {
        credits.SetActive(!credits.activeInHierarchy);
    }

    //plays the hover sound
    public void PlayHover() {
        AudioSource.PlayClipAtPoint(GameManager.local.buttonHover, Camera.main.transform.position, GameManager.local.UIVolume);
    }

    //plays the click sound
    public void PlayClick() {
        AudioSource.PlayClipAtPoint(GameManager.local.buttonClick, Camera.main.transform.position, GameManager.local.UIVolume);
    }
}