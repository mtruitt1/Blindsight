using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//defines a path for an enemy biped to follow
//serializeable so I don't have to make objects and/or prefabs to handle this work
[System.Serializable]
public class PatrolPath {
    public List<PatrolSpot> spotsOnPath = new List<PatrolSpot>();
    public int spotCount => spotsOnPath.Count;

    //allows accessing the spots like this class is an array with these members
    public PatrolSpot this[int i] => spotsOnPath[i];

    //moves the spot to the back, to prevent staying at the same place for more than the patrol length
    public void MoveToBack(int i) {
        PatrolSpot spot = spotsOnPath[i];
        spotsOnPath.RemoveAt(i);
        spotsOnPath.Add(spot);
    }

    //a spot for the enemy biped to stay at, and how long to do so
    [System.Serializable]
    public class PatrolSpot {
        public Space area;
        public float timeToStay = 5f;
    }
}