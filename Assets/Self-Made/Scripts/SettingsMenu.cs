using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;

//contains all the functions for changing settings/updating data
public class SettingsMenu : MonoBehaviour {
    public static SettingsMenu local;
    public GameObject menu;
    public GameObject dataMenu;
    public bool fpsOverlay { get; private set; } = false;
    public ButtonManager fpsButton;
    public bool fullScreen { get; private set; } = true;
    public ButtonManager fullscreenButton;
    private float? confirmTimer = null;
    public float confirmLength = 3f;
    public ButtonManager clearButton;
    public ButtonManager confirmButton;

    //set these values based on the playerprefs
    private void Start() {
        local = this;
        if (PlayerPrefs.HasKey("Fullscreen")) {
            fullScreen = PlayerPrefs.GetString("Fullscreen") == "True";
        }
        if (PlayerPrefs.HasKey("FPSOverlay")) {
            fpsOverlay = PlayerPrefs.GetString("FPSOverlay") == "True";
        }
    }

    //updates button text, does the confirm countdown timer
    private void Update() {
        fpsButton.buttonText = "FPS COUNTER: " + (fpsOverlay ? "ON" : "OFF");
        fpsButton.UpdateUI();
        fullscreenButton.buttonText = "FULLSCREEN: " + (fullScreen ? "ON" : "OFF");
        fullscreenButton.UpdateUI();
        if (confirmTimer != null) {
            confirmButton.buttonText = "CONFIRM? " + Mathf.CeilToInt(confirmTimer.GetValueOrDefault());
            confirmButton.UpdateUI();
            clearButton.gameObject.SetActive(false);
            confirmButton.gameObject.SetActive(true);
            confirmTimer -= Time.unscaledDeltaTime;
            if (confirmTimer.GetValueOrDefault() <= 0f) {
                confirmTimer = null;
            }
        } else {
            clearButton.gameObject.SetActive(true);
            confirmButton.gameObject.SetActive(false);
        }
    }

    //opens the settings, and the data menu if in the main menu(where the dataMenu variable is filled and non-null)
    public void ToggleMenu() {
        menu.SetActive(!menu.activeInHierarchy);
        if (dataMenu != null) {
            dataMenu.SetActive(!dataMenu.activeInHierarchy);
        }
    }

    //toggles fullscreen, the GameManager actually updates Screen.fullscreen
    public void ToggleFullscreen() {
        fullScreen = !fullScreen;
        PlayerPrefs.SetString("Fullscreen", fullScreen ? "True" : "False");
    }

    //tells the GameManager to toggle the fps overlay
    public void ToggleFPS() {
        fpsOverlay = !fpsOverlay;
        PlayerPrefs.SetString("FPSOverlay", fpsOverlay ? "True" : "False");
    }

    //unlocks all levels
    public void UnlockAll() {
        PlayerPrefs.SetInt("HighestLevel", 10);
        GameManager.local.LoadMenu();
    }

    //starts the process of clearing playerprefs
    public void StartClear() {
        confirmTimer = confirmLength;
    }

    //finishes the process of clearing playerprefs, reloads the menu
    public void ConfirmClear() {
        PlayerPrefs.DeleteAll();
        GameManager.local.LoadMenu();
        confirmTimer = null;
    }
}