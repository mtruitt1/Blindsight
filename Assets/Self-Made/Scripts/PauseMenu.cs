using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {
    public GameObject canvas;
    public float volume;
    private AudioSource player;

    private void Start() {
        player = GetComponent<AudioSource>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            GameManager.local.state = GameManager.local.state == GameManager.GameState.Paused ? GameManager.GameState.Playing : GameManager.GameState.Paused;
        }
        canvas.SetActive(GameManager.local.state != GameManager.GameState.Playing);
        Cursor.lockState = GameManager.local.state == GameManager.GameState.Playing ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void Resume() {
        GameManager.local.state = GameManager.GameState.Playing;
    }

    public void Quit() {
        GameManager.local.LoadMenu();
    }

    public void PlayHover() {
        player.transform.position = Camera.main.transform.position;
        player.clip = GameManager.local.buttonHover;
        player.volume = GameManager.local.UIVolume;
        Time.timeScale = 1f;
        player.Play();
        Time.timeScale = 0f;
    }

    public void PlayClick() {
        player.transform.position = Camera.main.transform.position;
        player.clip = GameManager.local.buttonClick;
        player.volume = GameManager.local.UIVolume;
        Time.timeScale = 1f;
        player.Play();
        Time.timeScale = 0f;
    }
}