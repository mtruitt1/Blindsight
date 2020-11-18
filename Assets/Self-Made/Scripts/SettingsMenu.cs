using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;

public class SettingsMenu : MonoBehaviour {
    public static SettingsMenu local;
    public GameObject menu;
    public bool fpsOverlay { get; private set; } = false;
    public ButtonManager fpsButton;
    public bool fullScreen { get; private set; } = true;
    public ButtonManager fullscreenButton;
    private float? confirmTimer = null;
    public float confirmLength = 3f;
    public ButtonManager clearButton;
    public ButtonManager confirmButton;

    private void Start() {
        local = this;
        if (PlayerPrefs.HasKey("Fullscreen")) {
            fullScreen = PlayerPrefs.GetString("Fullscreen") == "True";
        }
        if (PlayerPrefs.HasKey("FPSOverlay")) {
            fpsOverlay = PlayerPrefs.GetString("FPSOverlay") == "True";
        }
    }

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

    public void ToggleMenu() {
        menu.SetActive(!menu.activeInHierarchy);
    }

    public void ToggleFullscreen() {
        fullScreen = !fullScreen;
        PlayerPrefs.SetString("Fullscreen", fullScreen ? "True" : "False");
    }

    public void ToggleFPS() {
        fpsOverlay = !fpsOverlay;
        PlayerPrefs.SetString("FPSOverlay", fpsOverlay ? "True" : "False");
    }

    public void StartClear() {
        confirmTimer = confirmLength;
    }

    public void ConfirmClear() {
        PlayerPrefs.DeleteAll();
        GameManager.local.LoadMenu();
        confirmTimer = null;
    }
}