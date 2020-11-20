using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyBiped : BipedWalker {
    public Transform castPoint;
    public int degreesTolerance = 1;
    public float distanceTolerance = 0.1f;
    public float sprintDistance = 0f;
    public PatrolPath predefinedPath = new PatrolPath();
    public float forgetTime = 5f;
    public float interestMultiplier = 0.5f;
    protected List<Vector3> goals = new List<Vector3>();
    protected float forgetTimer = 0f;
    protected float patrolTimer = 0f;

    protected override void Start() {
        base.Start();
        foreach (StrikingObject striker in strikers) {
            striker.owner = this;
        }
    }

    protected override void SoundReact(SoundWave wave, SoundObject highest, SoundObject maker) {
        base.SoundReact(wave, highest, maker);
        bool notAnotherEnemy = true;
        if (highest != null) {
            notAnotherEnemy = highest.tag != "Enemy";
        }
        if (notAnotherEnemy) {
            HeardSound newSound = new HeardSound(wave.strength, wave.strength / wave.maxStrength, wave.transform.position);
            if (DeterminePathToPoint(newSound.position)) {
                forgetTimer += forgetTime * interestMultiplier;
                lastHeard = newSound;
            }
        }
    }

    protected bool DeterminePathToPoint(Vector3 position) {
        List<Space> currentRooms = GameManager.local.GetRoomsForPoint(castPoint.position);
        if (currentRooms.Count > 0) {
            List<Vector3> shortestPath = currentRooms[0].PathTo(position);
            //Debug.Log("Starting from " + currentRooms[0].name);
            if (currentRooms.Count > 1) {
                for (int i = 1; i < currentRooms.Count; i++) {
                    List<Vector3> foundPath = currentRooms[i].PathTo(position);
                    //Debug.Log("Starting from " + currentRooms[i].name);
                    if (foundPath != null) {
                        if (shortestPath == null || foundPath.Count < shortestPath.Count) {
                            shortestPath = foundPath;
                        }
                    }
                }
            }
            if (shortestPath?.Count > 0) {
                goals = shortestPath;
                //Debug.Log("Found path to " + position);
                return true;
            }
        }
        //Debug.Log("Could not find path to " + position);
        return false;
    }

    protected bool CheckCanSee(Vector3 pos) {
        int layerMask = ~((1 << 11) | (1 << 12) | (1 << 14) | (1 << 16) | (1 << 17)); //all layers except player, enemies, sound waves, player trigger zones, and spaces
        bool canSee = !Physics.Linecast(castPoint.position, pos, layerMask);
        //Debug.Log(canSee ? "Can see point" : "Can't see point");
        return canSee;
    }

    protected float SumRemainingPath() {
        if (goals.Count > 1) {
            float returnVal = 0f;
            for (int i = 0; i < goals.Count - 1; i++) {
                int j = i + 1;
                returnVal += Vector3.Distance(goals[i], goals[j]);
            }
            return returnVal;
        }
        return 0f;
    }

    protected void ReducePath() {
        int runningIndex = goals.Count - 1;
        while (runningIndex > 0) {
            if (CheckCanSee(goals[runningIndex])) {
                for (int i = 0; i < runningIndex; i++) {
                    goals.RemoveAt(0);
                }
                runningIndex = goals.Count - 1;
            } else {
                runningIndex--;
            }
        }
    }

    protected override void Update() {
        base.Update();
        if (PlayerControls.local.dead) {
            goals.Clear();
            forwardGoal = 0f;
            angleToTurn = 0f;
            return;
        }
        if (lastHeard != null) {
            patrolTimer = 0f;
            forgetTimer -= Time.deltaTime;
            if (forgetTimer <= 0f) {
                lastHeard = null;
                forgetTimer = forgetTime;
            }
        } else {
            forgetTimer = forgetTime;
        }
        if (goals.Count > 0) {
            ReducePath();
            if (!CheckCanSee(goals[0])) {
                goals.Clear();
            }
        }
        if (goals.Count > 0) {
            float distance = Vector3.Distance(transform.position, goals[0]);
            sprint = distance + SumRemainingPath() >= sprintDistance;
            if (distance <= distanceTolerance) {
                forwardGoal = 0f;
                goals.RemoveAt(0);
                if (goals.Count == 0) {
                    forgetTimer -= forgetTime / 2;
                }
            } else {
                Vector3 flatHere = transform.position;
                flatHere.y = 0f;
                Vector3 flatThere = goals[0];
                flatThere.y = 0f;
                if (Vector3.Angle(root.forward, flatThere - flatHere) <= degreesTolerance) {
                    angleToTurn = 0f;
                } else {
                    angleToTurn = Vector3.SignedAngle(root.forward, flatThere - flatHere, transform.up);
                }
                forwardGoal = 1f;
            }
        } else {
            forwardGoal = 0f;
            angleToTurn = 0f;
            patrolTimer -= Time.deltaTime;
            if (patrolTimer <= 0f) {
                int random = Random.Range(0, predefinedPath.spotCount - 1);
                PatrolPath.PatrolSpot spot = predefinedPath[random];
                predefinedPath.MoveToBack(random);
                DeterminePathToPoint(spot.area.transform.position);
                patrolTimer = spot.timeToStay;
            }
        }
    }
}