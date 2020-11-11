using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class PivotCamera : MonoBehaviour {
    public Camera camera;

    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    float x = 45f;
    float y = 45f;

    private void Start() {
        camera = GetComponent<Camera>();
    }

    void LateUpdate() {
        if (GameManager.local.state == GameManager.GameState.Paused) {
            return;
        }
        if (target) {
            float mouseX = 0f;
            float mouseY = 0f;
            float newDistance = distance;

            mouseX = Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            mouseY = Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            x += mouseX;
            y -= mouseY;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -newDistance);
            Vector3 position = rotation * negDistance + target.position;

            RaycastHit hit;
            if (Physics.Linecast(target.position, position, out hit, 1 << 10)) {
                y += mouseY;
                x -= mouseX;
            } else {
                distance = newDistance;
            }

            rotation = Quaternion.Euler(y, x, 0);
            negDistance = new Vector3(0.0f, 0.0f, -distance);
            position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    public static float ClampAngle(float angle, float min, float max) {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}