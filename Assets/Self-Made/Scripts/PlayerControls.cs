using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//handles all player input and updates the biped walker to reflect it
//also does some other stuff such as wind noise, rock throwing, and player death
public class PlayerControls : MonoBehaviour {
    public static PlayerControls local;
    public BipedWalker walker;
    public PivotCamera pivot;
    public Transform cameraPoint;
    public float angleTolerance = 1f;
    public float turnSpeed = 5f;
    public float jumpPower = 10f;
    public float coolDown = 0.5f;
    public Transform rockSpawn;
    public float rockSpeed = 10f;
    public GameObject heartbeat;
    private float timer;
    private AudioSource wind = null;
    public AudioClip windClip;
    public float windVolMult = 1f;
    public float windVolExp = 1f;
    public float windVolOffset = 0f;
    public bool dead { get; private set; } = false;

    //gets the pivot camera if not already assigned, sets its target, and locks the cursor in. sets the local playercontrols to this
    private void Start() {
        local = this;
        pivot = Camera.main.GetComponent<PivotCamera>();
        pivot.target = cameraPoint;
        Cursor.lockState = CursorLockMode.Locked;
    }

    //sets turn goal, forward and right goals, as well as watched for sprint, crouch, and jump inputs. throws a rock if right mouse button pressed
    private void Update() {
        if (wind == null) {
            wind = pivot.gameObject.AddComponent<AudioSource>();
            wind.playOnAwake = false;
            wind.clip = windClip;
            wind.loop = true;
            wind.Play();
            wind.volume = 0f;
        } else {
            wind.volume = Mathf.Clamp(windVolMult * Mathf.Pow(Mathf.Abs(walker.rb.velocity.y) + windVolOffset, windVolExp), 0f, 1 - GameManager.local.fadeOut.color.a);
        }
        if (GameManager.local.state == GameManager.GameState.Paused) {
            wind.Pause();
            return;
        }
        wind.UnPause();
        if (dead) {
            if (1 - GameManager.local.fadeOut.color.a == 0) {
                GameManager.local.LoadScene(ExitDoor.local.levelNum);
            }
            return;
        }
        int layerMask = ~((1 << 11) | (1 << 12) | (1 << 13) | (1 << 14) | (1 << 16) | (1 << 17)); //all layers except player, enemies, enemy bounding walls, sound waves, player trigger zones, and spaces
        if (Physics.Linecast(cameraPoint.position, pivot.transform.position, out RaycastHit hitInfo, layerMask)) {
            pivot.GetComponent<Camera>().nearClipPlane = Vector3.Distance(pivot.transform.position, hitInfo.point) + 0.5f;
        }
        walker.forwardGoal = Input.GetAxis("Vertical");
        walker.rightGoal = Input.GetAxis("Horizontal");
        if (walker.grounded) {
            Vector3 cameraFacing = pivot.transform.forward;
            cameraFacing.y = 0f;
            if (walker.grounded && !walker.jump && !walker.crouch && Mathf.Abs(walker.forwardCurrent) < 0.1f && Mathf.Abs(walker.rightCurrent) < 0.1f && Vector3.Angle(transform.forward, cameraFacing) > angleTolerance) {
                walker.angleToTurn = Vector3.SignedAngle(transform.forward, cameraFacing, transform.up);
                
            } else {
                walker.angleToTurn = 0f;
                transform.LookAt(transform.position + cameraFacing, Vector3.up);
            }
            if (Input.GetKeyDown(KeyCode.Space)) {
                walker.jump = true;
                walker.rb.velocity += Vector3.up * jumpPower;
            }
            if (Input.GetKey(KeyCode.C)) {
                walker.crouch = true;
            } else {
                walker.crouch = false;
            }
            if (Input.GetKey(KeyCode.LeftShift)) {
                walker.sprint = true;
            } else {
                walker.sprint = false;
            }
        }
        timer -= Time.deltaTime;
        if (Input.GetMouseButtonDown(1) && timer <= 0f) {
            timer = coolDown;
            Rigidbody rb = Instantiate(GameManager.local.rock, rockSpawn.position, Quaternion.identity);
            try {
                rb.gameObject.GetComponent<SoundObject>().owner = walker;
            } catch {
                Debug.LogError("Couldn't set rock owner to player");
            }
            rb.velocity = rockSpeed * pivot.transform.forward;
        }
    }

    //does all the necessary player death stuff- stop the player, stop the player's heart, mark the player as dead, and starts the fade to black
    public void PlayerDeath() {
        heartbeat.SetActive(false);
        walker.forwardGoal = 0f;
        walker.rightGoal = 0f;
        walker.angleToTurn = 0f;
        dead = true;
        walker.SetDead();
        GameManager.local.cover = true;
    }
}