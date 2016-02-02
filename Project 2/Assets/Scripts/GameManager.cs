using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Map GameMap;
    public int GridWidth = 45;
    public int GridHeight = 20;
    public float PercentBombs = 0.15f;
    public int NumberOfPlayers = 2;

	void Start () {
        GameMap.Initialize(GridWidth, GridHeight, PercentBombs, NumberOfPlayers);
	}
	
	void Update () {
	
	}
}
