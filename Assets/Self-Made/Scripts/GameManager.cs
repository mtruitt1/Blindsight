using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

//handles anything that multiple different classes need access to, to be able to do, etc.
//uses a static GameManager.local for easy access, allows me to save a prefab with some default settings
public class GameManager : MonoBehaviour {
    public static GameManager local;
    public SoundWave soundSphere;
    public Rigidbody rock;
    public StrikingObject footStep;
    public float strikeMult = 1f;
    public float moveBallFallDetect = 0.01f;
    public List<Level> levels;
    public GameState state = GameState.Playing;
    public float UIVolume = 1f;
    public AudioClip buttonHover;
    public AudioClip buttonClick;
    public GameObject FPSOverlay;
    public TextMeshProUGUI fpsText;
    public float updateRateSeconds = 4.0F;
    private List<Space> allSpaces = new List<Space>();
    public bool cover = true;
    public Image fadeOut;
    public float fadeSpeed = 0.5f;
    public float ambientVol = 1f;
    public List<AudioClip> ambient = new List<AudioClip>();
    private AudioSource musicPlayer;
    int frameCount = 0;
    float dt = 0.0F;
    float fps = 0.0F;

    //sets the application target framerate the makes sure this is the singleton before any start functions
    private void Awake() {
        local = this;
        Application.targetFrameRate = 1000;
    }

    //gets all the spaces in the level, sets the fade opacity, and gets the music player
    private void Start() {
        allSpaces.AddRange(GameObject.FindObjectsOfType<Space>());
        fadeOut.color = new Color(0, 0, 0, cover ? 1f : 0f);
        cover = false;
        musicPlayer = Camera.main.GetComponent<AudioSource>();
        musicPlayer.volume = (1 - fadeOut.color.a) * ambientVol;
        musicPlayer.clip = ambient[Random.Range(0, ambient.Count)];
        musicPlayer.Play();
    }

    //loads the main menu
    public void LoadMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    //loads a scene using index, so I don't have to rely on strings and names anywhere else in the code
    public bool LoadScene(int index) {
        if (index > levels.Count - 1) {
            return false;
        }
        SceneManager.LoadScene(levels[index].sceneName);
        return true;
    }

    //always loads the highest playable scene. this could break, but is never used in a way that breaks.
    public void LoadHighestPlayable() {
        LoadScene(PlayerPrefs.GetInt("HighestLevel") + 1);
    }

    //spawns a sound wave at a specific point, typically at the same point as a sound is playing
    public static void SpawnSound(float str, Vector3 position, bool strike, bool reg, SoundObject maker) {
        if (strike) {
            Instantiate(local.soundSphere, position, Quaternion.identity).Emit(str * local.strikeMult, reg, maker);
        } else {
            Instantiate(local.soundSphere, position, Quaternion.identity).Emit(str, reg, maker);
        }
    }

    //gets a list of rooms that a point is inside of-- can return multiple in places where overlap exists
    public List<Space> GetRoomsForPoint(Vector3 point) {
        List<Space> roomsForPoint = new List<Space>();
        //Debug.Log("START GET ROOMS");
        foreach (Space space in allSpaces) {
            if (space.CheckPointInsideRoom(point)) {
                roomsForPoint.Add(space);
            }
        }
        //Debug.Log("END GET ROOMS");
        return roomsForPoint;
    }

    //adjusts the fade opacity, the ambience volume, and handles timescale. also deals with the fps counter
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
            dt -= 1.0f / updateRateSeconds;
        }
        fpsText.text = System.Math.Round(fps, 1).ToString("0.0");
        fadeOut.color = new Color(0, 0, 0, Mathf.Clamp01(fadeOut.color.a + (Time.deltaTime * (cover ? 1 : -1) * fadeSpeed)));
        musicPlayer.volume = (1 - fadeOut.color.a) * ambientVol;
        if (!musicPlayer.isPlaying) {
            musicPlayer.clip = ambient[Random.Range(0, ambient.Count)];
            musicPlayer.Play();
        }
    }

    //what state the game is in-- this could potentially have been a "paused" boolean, but I was anticipating possibly needing more states
    public enum GameState {
        Playing = 0,
        Paused = 1
    }

    //a class that allows me to give scenes nicer names than the ones I give them in the editor
    [System.Serializable]
    public class Level {
        public string Name;
        public string sceneName;
    }
}