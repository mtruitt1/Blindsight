using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//allows the game to be paused and open a menu
public class PauseMenu : MonoBehaviour {
    public GameObject canvas;
    public float volume;
    private AudioSource player;

    //gets the audiosource to play menu sounds with
    private void Start() {
        player = GetComponent<AudioSource>();
    }

    //sets the pause menu, cursor lock, and game manager state based on escape button push
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            GameManager.local.state = GameManager.local.state == GameManager.GameState.Paused ? GameManager.GameState.Playing : GameManager.GameState.Paused;
        }
        canvas.SetActive(GameManager.local.state != GameManager.GameState.Playing);
        Cursor.lockState = GameManager.local.state == GameManager.GameState.Playing ? CursorLockMode.Locked : CursorLockMode.None;
    }

    //resumes the game
    public void Resume() {
        GameManager.local.state = GameManager.GameState.Playing;
    }

    //quits to the main menu
    public void Quit() {
        GameManager.local.LoadMenu();
    }

    //plays the hover sound using the sound player
    public void PlayHover() {
        player.transform.position = Camera.main.transform.position;
        player.clip = GameManager.local.buttonHover;
        player.volume = GameManager.local.UIVolume;
        Time.timeScale = 1f;
        player.Play();
        Time.timeScale = 0f;
    }

    //plays the click sound using the sound player
    public void PlayClick() {
        player.transform.position = Camera.main.transform.position;
        player.clip = GameManager.local.buttonClick;
        player.volume = GameManager.local.UIVolume;
        Time.timeScale = 1f;
        player.Play();
        Time.timeScale = 0f;
    }
}