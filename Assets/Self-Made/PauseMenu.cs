using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {
    public GameObject canvas;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameManager.local.state == GameManager.GameState.Paused) {
                GameManager.local.state = GameManager.GameState.Playing;
            } else {
                GameManager.local.state = GameManager.GameState.Paused;
            }
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
}