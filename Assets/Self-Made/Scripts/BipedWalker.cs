using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//a biped walker is a script for any humanoids which need to move around the map: this script allows them to walk, turn, sprint, jump, and crouch
//this is important partially so that all bipeds have footsteps
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
    public float crouchMult = 0.5f;
    public float sprintMult = 2f;
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

    //get some of the base components required for the biped walker, such as the animator, rigidbody, capsule collider, and set up the foot colliders for footsteps
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

    //set the step strength based on stance, allow rotation, and smoothly manipulate the actual current forward/right/turn values for smooth transitions into animations
    protected virtual void Update() {
        if (GameManager.local.state == GameManager.GameState.Paused) {
            return;
        }
        if (crouch || turnCurrent != 0f) {
            realStrength = stepStrength * crouchMult;
        } else if (sprint) {
            realStrength = stepStrength * sprintMult;
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

    //exclusively for the player, sets the Animator component "Dead" bool true, playing the death animation
    public void SetDead() {
        model.SetBool("Dead", true);
    }

    //runs after Update(), intended to make sure that the velocity the player had when they left the ground is maintained
    private void LateUpdate() {
        if (!grounded) {
            rb.velocity = new Vector3(moveBall.airSpeed.x, rb.velocity.y, moveBall.airSpeed.z);
        }
    }

    //a foot "class". technically this could be a struct
    [System.Serializable]
    public class Foot {
        public Rigidbody footRB;
        public Vector3 localPosition;
        public Vector3 localEuler;
        public Vector3 localScale;
    }
}