using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BipedWalker : MonoBehaviour {
    private Animator model;
    public SphereCollider moveBall;
    private Rigidbody rb;
    public Foot left;
    public Foot right;
    public float changeSpeed = 5f;
    public float forwardGoal = 0f;
    public float rightGoal = 0f;
    public float turnGoal = 0f;
    public float stepStrength = 1f;
    public float minColMag = 1f;
    public float maxColMag = 1000f;
    public float forwardCurrent { get; private set; } = 0f;
    public float rightCurrent { get; private set; } = 0f;
    public float turnCurrent { get; private set; } = 0f;
    public bool grounded { get; private set; } = true;
    public float realStrength { get; private set; } = 1f;
    public bool jump = false;
    public bool crouch = false;
    public bool sprint = false;
    private List<StrikingObject> strikers = new List<StrikingObject>();

    private void Start() {
        model = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        List<Foot> feet = new List<Foot>() {
            left,
            right
        };
        foreach (Foot f in feet) {
            StrikingObject step = Instantiate(GameManager.local.footStep);
            step.strengthMult = stepStrength;
            step.minColMag = minColMag;
            step.maxColMag = maxColMag;
            step.transform.parent = f.footRB.transform;
            step.transform.localPosition = f.localPosition;
            step.transform.localEulerAngles = f.localEuler;
            step.transform.localScale = f.localScale;
            step.transform.parent = null;
            FixedJoint joint = step.gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = f.footRB;
            strikers.Add(step);
        }
    }

    private void Update() {
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

    [System.Serializable]
    public class Foot {
        public Rigidbody footRB;
        public Vector3 localPosition;
        public Vector3 localEuler;
        public Vector3 localScale;
    }
}