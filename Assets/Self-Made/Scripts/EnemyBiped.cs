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
            forgetTimer += forgetTime * interestMultiplier;
            if (DeterminePathToPoint(newSound.position)) {
                lastHeard = newSound;
            }
        }
    }

    protected bool DeterminePathToPoint(Vector3 position) {
        Space currentRoom = GameManager.local.GetRoomForPoint(castPoint.position);
        List<Vector3> path = currentRoom.PathTo(position);
        if (path?.Count > 0) {
            goals = path;
            Debug.Log("Goal is " + goals[0] + ", could find path to point");
            return true;
        } else {
            if (currentRoom.CheckPointInsideRoom(position)) {
                goals.Add(position);
                Debug.Log("Goal is " + goals[0] + ", point in same room");
                return true;
            } else {
                return false;
            }
        }
    }

    protected bool CheckCanSee(Vector3 pos) {
        int layerMask = ~((1 << 11) | (1 << 12) | (1 << 14) | (1 << 16) | (1 << 17)); //all layers except player, enemies, sound waves, player trigger zones, and spaces
        return !Physics.Linecast(castPoint.position, pos, layerMask);
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

    protected override void Update() {
        base.Update();
        if (lastHeard != null) {
            patrolTimer = 0f;
            forgetTimer -= Time.deltaTime;
            if (forgetTimer <= 0f) {
                lastHeard = null;
                goals.Clear();
                forgetTimer = forgetTime;
            }
        } else {
            forgetTimer = forgetTime;
        }
        if (goals.Count == 1) {
            if (!CheckCanSee(goals[0])) {
                forwardGoal = 0f;
                goals.RemoveAt(0);
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