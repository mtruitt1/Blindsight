﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//spaces are areas which enemies can navigate in
//the heavy lifting for pathfinding is all done in here
[RequireComponent(typeof(BoxCollider))]
public class Space : MonoBehaviour {
    private BoxCollider room;
    public List<Space> connections = new List<Space>();
    public List<Space> manualConnections = new List<Space>();

    //get the collider, make sure this space won't collide, and then add a rigidbody for collision tracking purposes. finally, find all connected spaces
    private void Start() {
        room = GetComponent<BoxCollider>();
        room.isTrigger = true;
        gameObject.AddComponent<Rigidbody>().isKinematic = true;
        CheckConnections();
    }

    //finds all overlapping colliders, adds them to the connections list if they're spaces
    public void CheckConnections() {
        int layerMask = ~0; //everything, since the check is going to only add colliders with a Space component attached
        Collider[] adjacent = Physics.OverlapBox(transform.position + room.center, room.bounds.extents, transform.rotation, layerMask, QueryTriggerInteraction.Collide);
        foreach (Collider col in adjacent) {
            //Debug.Log(name + " checking " + col.name);
            if (col.TryGetComponent(out Space other)) {
                if (other != this) {
                    connections.Add(other);
                }
            }
        }
        connections.AddRange(manualConnections);
    }

    //check if the input point is inside this space using ClosestPoint()
    public bool CheckPointInsideRoom(Vector3 point) {
        Vector3 closest = room.ClosestPoint(point);
        //Debug.Log(name + " has point " + point + (closest == point ? " inside" : " not inside"));
        return closest == point;
    }

    //find the shortest possible path from this room to the goal position
    public List<Vector3> PathTo(Vector3 goal) {
        List<Space> endSpaces = GameManager.local.GetRoomsForPoint(goal);
        //foreach (Space end in endSpaces) {
        //    Debug.Log("Point " + goal + " may be inside " + end.name);
        //}
        if (endSpaces.Count == 0) {
            return null;
        }
        List<Space> getPath = SpacesTo(endSpaces, new List<Space>());
        List<Vector3> goals = new List<Vector3>();
        if (getPath?.Count > 0) {
            if (getPath.Count > 1) {
                foreach (Space space in getPath) {
                    goals.Add(space.transform.position);
                }
            }
            goals.Add(goal);
        }
        return goals;
    }

    //find the shortest possible space combination to reach one of the end goal spaces
    public List<Space> SpacesTo(List<Space> goals, List<Space> previous) {
        List<Space> allPlusThis = new List<Space>();
        if (previous != null) {
            allPlusThis.AddRange(previous);
        }
        allPlusThis.Add(this);
        if (goals.Contains(this)) {
            return allPlusThis;
        }
        List<Space> noBackwards = new List<Space>();
        if (previous != null) {
            foreach (Space space in connections) {
                if (!previous.Contains(space)) {
                    noBackwards.Add(space);
                }
            }
        }
        if (noBackwards.Count > 0) {
            List<Space> shortestPath = noBackwards[0].SpacesTo(goals, allPlusThis);
            if (noBackwards.Count > 1) {
                for (int i = 1; i < noBackwards.Count; i++) {
                    List<Space> foundPath = noBackwards[i].SpacesTo(goals, allPlusThis);
                    if (foundPath != null) {
                        if (shortestPath == null || foundPath.Count < shortestPath.Count) {
                            shortestPath = foundPath;
                        }
                    }
                }
            }
            if (shortestPath?.Count > 0) {
                return shortestPath;
            }
        }
        return null;
    }
}