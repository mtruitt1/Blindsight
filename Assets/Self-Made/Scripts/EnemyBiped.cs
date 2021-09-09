using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the enemy biped class which handles all sound detection, pathfinding, etc. to make the enemies move between patrol points and heard sounds
//not sure why this is serializeable but at the time of writing this comment, Unity is not open so I will not remove it for fear something may break
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

    //makes sure that the footstep colliders are owned by this, to prevent the enemy from detecting its own footsteps
    protected override void Start() {
        base.Start();
        foreach (StrikingObject striker in strikers) {
            striker.owner = this;
        }
    }

    //react to a sound by checking to make sure it's not from another enemy, then seeing if it can pathfind to it. if yes, it updates its forget timer and changes the last hear sound to the one it's reacting to
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

    //finds a path from the point the enemy biped is at to the point it heard a sound(or needs to reach for patrolling)
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

    //uses a linecast with layermask to check if the biped can see the point passed in, generally used to shorten the path
    protected bool CheckCanSee(Vector3 pos) {
        int layerMask = ~((1 << 11) | (1 << 12) | (1 << 14) | (1 << 16) | (1 << 17)); //all layers except player, enemies, sound waves, player trigger zones, and spaces
        bool canSee = !Physics.Linecast(castPoint.position, pos, layerMask, QueryTriggerInteraction.Ignore);
        //Debug.Log(canSee ? "Can see point" : "Can't see point");
        return canSee;
    }

    //used to get the total distance of the remaining path
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

    //used to reduce the path as much as possible-- essentially "if I can draw a direct line to the end point, why bother stopping at the ones between?"
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

    //controls the timers for forgetting sounds, updating patrol, and actually changing goals and forward/turn goals
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
        //Debug.Log("Goal count: " + goals.Count);
        if (goals.Count > 0) {
            float distance = Vector3.Distance(transform.position, goals[0]);
            sprint = distance + SumRemainingPath() >= sprintDistance;
            if (distance <= distanceTolerance) {
                forwardGoal = 0f;
                Debug.Log("Within tolerance to goal " + goals[0]);
                goals.RemoveAt(0);
            } else {
                Vector3 flatHere = transform.position;
                flatHere.y = 0f;
                Vector3 flatThere = goals[0];
                flatThere.y = 0f;
                angleToTurn = Vector3.Angle(root.forward, flatThere - flatHere) <= degreesTolerance ? 0f : Vector3.SignedAngle(root.forward, flatThere - flatHere, transform.up);
                forwardGoal = 1f;
            }
        } else {
            //Debug.Log("Waiting at patrol point...");
            forwardGoal = 0f;
            angleToTurn = 0f;
            patrolTimer -= Time.deltaTime;
            if (patrolTimer <= 0f) {
                Debug.Log("Selecting new patrol point");
                int random = Random.Range(0, predefinedPath.spotCount - 1);
                PatrolPath.PatrolSpot spot = predefinedPath[random];
                predefinedPath.MoveToBack(random);
                DeterminePathToPoint(spot.area.transform.position);
                patrolTimer = spot.timeToStay;
                Debug.Log("Headed to " + spot.area.transform.position);
            }
        }
    }
}