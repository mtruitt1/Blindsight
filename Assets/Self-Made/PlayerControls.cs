using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {
    public BipedWalker walker;
    public PivotCamera pivot;
    public Transform cameraPoint;
    public float angleTolerance = 1f;
    public float turnSpeed = 5f;
    public float jumpPower = 10f;
    public float coolDown = 0.5f;
    public Transform rockSpawn;
    public float rockSpeed = 10f;
    private float timer;

    private void Start() {
        pivot.target = cameraPoint;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        if (GameManager.local.state == GameManager.GameState.Paused) {
            return;
        }
        int layerMask = ~((1 << 11) | (1 << 12) | (1 << 13) | (1 << 14) | (1 << 16)); //all layers except player, enemies, enemy bounding walls, sound waves, and player trigger zones
        if (Physics.Linecast(cameraPoint.position, pivot.transform.position, out RaycastHit hitInfo, layerMask)) {
            pivot.camera.nearClipPlane = Vector3.Distance(pivot.transform.position, hitInfo.point) + 0.5f;
        }
        walker.forwardGoal = Input.GetAxis("Vertical");
        walker.rightGoal = Input.GetAxis("Horizontal");
        if (walker.grounded) {
            Vector3 cameraFacing = pivot.transform.forward;
            cameraFacing.y = 0f;
            if (walker.grounded && !walker.jump && !walker.crouch && Mathf.Abs(walker.forwardCurrent) < 0.1f && Mathf.Abs(walker.rightCurrent) < 0.1f && Vector3.Angle(transform.forward, cameraFacing) > angleTolerance) {
                float angle = Vector3.SignedAngle(transform.forward, cameraFacing, transform.up);
                walker.turnGoal = angle > 0 ? 1 : -1;
                transform.Rotate(new Vector3(0, angle, 0) * Time.deltaTime * turnSpeed);
            } else {
                walker.turnGoal = 0f;
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
            rb.velocity = rockSpeed * pivot.transform.forward;
        }
    }
}