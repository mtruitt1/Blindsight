using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager local;
    public SoundWave soundSphere;
    public Rigidbody rock;
    public StrikingObject footStep;
    public float strikeMult = 1f;
    public float moveBallFallDetect = 0.01f;
    public List<string> levels;
    public GameState state = GameState.Playing;
    public float UIVolume = 1f;
    public AudioClip buttonHover;
    public AudioClip buttonClick;
    public GameObject FPSOverlay;
    public TextMeshProUGUI fpsText;
    public float updateRateSeconds = 4.0F;

    int frameCount = 0;
    float dt = 0.0F;
    float fps = 0.0F;

    private void Awake() {
        local = this;
    }

    public void LoadMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public bool LoadScene(int index) {
        if (index > levels.Count - 1) {
            return false;
        }
        SceneManager.LoadScene(levels[index]);
        return true;
    }

    public static void SpawnSound(float str, Vector3 position, bool strike, bool reg, SoundObject maker) {
        if (strike) {
            Instantiate(local.soundSphere, position, Quaternion.identity).Emit(str * local.strikeMult, reg, maker);
        } else {
            Instantiate(local.soundSphere, position, Quaternion.identity).Emit(str, reg, maker);
        }
    }

    private void Update() {
        FPSOverlay.SetActive(SettingsMenu.local.fpsOverlay);
        Screen.fullScreen = SettingsMenu.local.fullScreen;
        if (state == GameState.Paused) {
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1f;
        }
        frameCount++;
        dt += Time.unscaledDeltaTime;
        if (dt > 1.0 / updateRateSeconds) {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0F / updateRateSeconds;
        }
        fpsText.text = System.Math.Round(fps, 1).ToString("0.0");
    }

    public enum GameState {
        Playing = 0,
        Paused = 1
    }
}