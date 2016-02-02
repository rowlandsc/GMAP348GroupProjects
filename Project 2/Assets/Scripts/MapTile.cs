using UnityEngine;
using System.Collections;


public class MapTile : MonoBehaviour {

    public int X;
    public int Y;
    public Map ParentMap;

    public bool IsTraversable = true;
    public bool HasBomb = false;
    public bool IsGoal = false;

    void Start() {

    }

    void Update() {

    }
}
