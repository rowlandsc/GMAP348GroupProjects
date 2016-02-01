using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Map GameMap;
    public int GridWidth = 45;
    public int GridHeight = 20;

	void Start () {
        GameMap.Initialize(GridWidth, GridHeight);
	}
	
	void Update () {
	
	}
}
