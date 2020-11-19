using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatrolPath {
    public List<PatrolSpot> spotsOnPath = new List<PatrolSpot>();
    public int spotCount => spotsOnPath.Count;

    public PatrolSpot this[int i] => spotsOnPath[i];

    public void MoveToBack(int i) {
        PatrolSpot spot = spotsOnPath[i];
        spotsOnPath.RemoveAt(i);
        spotsOnPath.Add(spot);
    }

    [System.Serializable]
    public class PatrolSpot {
        public Space area;
        public float timeToStay = 5f;
    }
}