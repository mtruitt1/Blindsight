using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class replaces doors and allows any object to be moveable in any direction, or scaled
public class MoveableObject : SoundBouncer {
    public float moveTime = 0.5f;
    private float moveTimer = 0f;
    public bool moveConstantly;
    private Vector3 startPos;
    private Vector3 startEulers;
    private Vector3 startScale;
    public Vector3 endPos;
    public Vector3 endEulers;
    public Vector3 endScale = Vector3.one;
    public Transform moveObject;
    public BoxCollider carrySpace;

    //set the time to move, and get the starting information so movement works smoothly
    protected override void Start() {
        base.Start();
        moveTimer = moveTime;
        startPos = moveObject.localPosition;
        startEulers = moveObject.localEulerAngles;
        startScale = moveObject.localScale;
        if (carrySpace != null) {
            carrySpace.isTrigger = true;
        }
    }

    //if the object hasn't moved, set it to move and play a sound from its sound list
    public override void DoBounceBehavior(SoundWave wave, SoundObject highest, SoundObject maker) {
        ToggleMove();
    }

    protected void ToggleMove() {
        PlayRandSound(strength, false, false);
        toggleable = !toggleable;
    }

    //linearly transition from start to end vectors. the lerp function has reversed order because it makes it easier
    private void Update() {
        moveTimer = Mathf.Clamp(moveTimer + (toggleable ? -Time.deltaTime : Time.deltaTime), 0f, moveTime);
        if (moveConstantly && (moveTimer == 0f || Mathf.Abs(moveTimer - moveTime) < 0.001f)) {
            ToggleMove();
        }
        Vector3 moveDist = moveObject.position;
        moveObject.localPosition = Vector3.Lerp(endPos, startPos, moveTimer / moveTime);
        moveDist = moveObject.position - moveDist;
        moveObject.localEulerAngles = Vector3.Lerp(endEulers, startEulers, moveTimer / moveTime);
        moveObject.localScale = Vector3.Lerp(endScale, startScale, moveTimer / moveTime);
        if (carrySpace != null && moveDist.sqrMagnitude > 0f) {
            foreach (Collider col in Physics.OverlapBox(carrySpace.center + carrySpace.transform.localPosition + transform.position, carrySpace.bounds.extents, Quaternion.identity)) {
                if (col.attachedRigidbody != null) {
                    Rigidbody rb = col.attachedRigidbody;
                    if (!rb.isKinematic && rb.useGravity) {
                        rb.transform.position += new Vector3(moveDist.x, moveDist.y > 0 ? 0f : moveDist.y, moveDist.z);
                    }
                    //if (rb.TryGetComponent(out StrikingObject so)) {
                    //    so.pauseForFrame = true;
                    //}
                }
            }
        }
    }
}