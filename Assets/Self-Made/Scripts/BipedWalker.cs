using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BipedWalker : SoundReceiver {
    protected Animator model;
    public MoveBall moveBall;
    public Rigidbody rb { get; protected set; }
    public CapsuleCollider bodyCollider { get; protected set; }
    public Transform root;
    public Foot left;
    public Foot right;
    public float changeSpeed = 5f;
    public float forwardGoal = 0f;
    public float rightGoal = 0f;
    public float turnGoal = 0f;
    public float angleToTurn = 0f;
    public float turnSpeed = 5f;
    public float stepStrength = 1f;
    public float minColMag = 1f;
    public float maxColMag = 1000f;
    public float forwardCurrent { get; protected set; } = 0f;
    public float rightCurrent { get; protected set; } = 0f;
    public float turnCurrent { get; protected set; } = 0f;
    public bool grounded { get; protected set; } = true;
    public float realStrength { get; protected set; } = 1f;
    public bool jump = false;
    public bool crouch = false;
    public bool sprint = false;
    public List<AudioClip> footstepSounds = new List<AudioClip>();
    public float footStepvolMult = 1f;
    protected List<StrikingObject> strikers = new List<StrikingObject>();

    protected override void Start() {
        base.Start();
        model = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        bodyCollider = GetComponent<CapsuleCollider>();
        Physics.IgnoreCollision(bodyCollider, moveBall.GetComponent<Collider>());
        List<Foot> feet = new List<Foot>() {
            left,
            right
        };
        foreach (Foot f in feet) {
            StrikingObject step = Instantiate(GameManager.local.footStep);
            step.strengthMult = stepStrength;
            step.volMult = footStepvolMult;
            step.minColMag = minColMag;
            step.maxColMag = maxColMag;
            step.transform.parent = f.footRB.transform;
            step.transform.localPosition = f.localPosition;
            step.transform.localEulerAngles = f.localEuler;
            step.transform.localScale = f.localScale;
            step.transform.parent = null;
            step.owner = this;
            FixedJoint joint = step.gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = f.footRB;
            strikers.Add(step);
            Physics.IgnoreCollision(bodyCollider, step.GetComponent<Collider>());
            Physics.IgnoreCollision(moveBall.GetComponent<Collider>(), step.GetComponent<Collider>());
        }
    }

    protected virtual void Update() {
        if (GameManager.local.state == GameManager.GameState.Paused) {
            return;
        }
        if (crouch || turnCurrent != 0f) {
            realStrength = stepStrength * 0.5f;
        } else if (sprint) {
            realStrength = stepStrength * 2f;
        } else {
            realStrength = stepStrength;
        }
        foreach (StrikingObject step in strikers) {
            step.strengthMult = realStrength;
            step.minColMag = minColMag;
            step.maxColMag = maxColMag;
        }
        grounded = moveBall.grounded;
        if (Mathf.Abs(angleToTurn) > 0.01f) {
            turnGoal = angleToTurn > 0 ? 1 : -1;
            transform.Rotate(new Vector3(0, angleToTurn, 0) * Time.deltaTime * turnSpeed);
        } else {
            turnGoal = 0;
        }
        forwardCurrent = Mathf.MoveTowards(forwardCurrent, forwardGoal, Time.deltaTime * changeSpeed);
        rightCurrent = Mathf.MoveTowards(rightCurrent, rightGoal, Time.deltaTime * changeSpeed);
        turnCurrent = Mathf.MoveTowards(turnCurrent, turnGoal, Time.deltaTime * changeSpeed);
        model.SetFloat("Forward", forwardCurrent);
        model.SetFloat("Right", rightCurrent);
        model.SetFloat("Turn", turnCurrent);
        model.SetBool("Grounded", grounded);
        model.SetBool("Jump", jump);
        model.SetBool("Crouch", crouch);
        model.SetBool("Sprint", sprint);
        jump = false;
    }

    public void SetDead() {
        model.SetBool("Dead", true);
    }

    private void LateUpdate() {
        if (!grounded) {
            rb.velocity = new Vector3(moveBall.airSpeed.x, rb.velocity.y, moveBall.airSpeed.z);
        }
    }

    [System.Serializable]
    public class Foot {
        public Rigidbody footRB;
        public Vector3 localPosition;
        public Vector3 localEuler;
        public Vector3 localScale;
    }
}