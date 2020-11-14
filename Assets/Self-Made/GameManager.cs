using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (state == GameState.Paused) {
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1f;
        }
    }

    public enum GameState {
        Playing = 0,
        Paused = 1
    }
}