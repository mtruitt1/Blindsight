using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Space : MonoBehaviour {
    private BoxCollider room;
    public List<Space> connections = new List<Space>();
    public List<Space> manualConnections = new List<Space>();

    private void Start() {
        room = GetComponent<BoxCollider>();
        room.isTrigger = true;
        gameObject.AddComponent<Rigidbody>().isKinematic = true;
        CheckConnections();
    }

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

    public bool CheckPointInsideRoom(Vector3 point) {
        Vector3 closest = room.ClosestPoint(point);
        Debug.Log(name + " has point " + point + (closest == point ? " inside" : " not inside"));
        return closest == point;
    }

    public List<Vector3> PathTo(Vector3 goal) {
        List<Space> getPath = SpacesTo(goal, null);
        List<Vector3> goals = new List<Vector3>();
        if (getPath?.Count > 0) {
            foreach (Space space in getPath) {
                goals.Add(space.transform.position);
            }
            goals.Add(goal);
        }
        return goals;
    }

    public List<Space> SpacesTo(Vector3 goal, List<Space> previous) {
        List<Space> allPlusThis = new List<Space>();
        if (previous != null) {
            allPlusThis.AddRange(previous);
        }
        allPlusThis.Add(this);
        if (CheckPointInsideRoom(goal)) {
            if (previous == null) {
                return null;
            }
            return allPlusThis;
        } else {
            List<Space> noBackwards = new List<Space>();
            if (previous != null) {
                foreach (Space connection in connections) {
                    if (!previous.Contains(connection)) {
                        noBackwards.Add(connection);
                    }
                }
            }
            if (noBackwards.Count > 0) {
                List<Space> shortestPath = null;
                foreach (Space connection in noBackwards) {
                    List<Space> foundPath = connection.SpacesTo(goal, allPlusThis);
                    if (foundPath != null) {
                        if (shortestPath == null) {
                            shortestPath = foundPath;
                        } else if (foundPath.Count < shortestPath.Count) {
                            shortestPath = foundPath;
                        }
                    }
                }
                return shortestPath;
            } else {
                return null;
            }
        }
    }
}