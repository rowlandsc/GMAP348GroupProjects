using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Map : MonoBehaviour {

    public static Map Instance = null;
    void Awake() {
        if (!Instance) {
            Instance = this;
        }
        else {
            Destroy(this);
        }
    }

    public List<List<MapTile>> TileMap;
    public Canvas MapCanvas;
    public float TileWidth = 64;
    public float TileHeight = 64;
    public Sprite TileSprite;
    public Sprite P1MarkerSprite;
    public Sprite P2MarkerSprite;

    public Color BoundaryColor;
    public Color NotVisibleColor;
    public Color NotExploredColor;
    public Color NextToBombColor;
    public Color ExploredColor;
    public Color GoalColor;

    public List<MapTile> PlayerStartTiles;

    public void Initialize(int width, int height, float percentBombs, int NumberOfPlayers) {
        TileMap = new List<List<MapTile>>();
        for (int i=0; i< width; i++) {
            TileMap.Add(new List<MapTile>());
            for (int j=0; j< height; j++) {
                GameObject newMapSquare = new GameObject("Map tile " + i + " " + j);
                newMapSquare.transform.SetParent(transform);
                newMapSquare.transform.localPosition = new Vector3(i * TileWidth, j * TileHeight, 0);
                newMapSquare.transform.localScale = new Vector3(TileWidth / TileSprite.bounds.size.x, TileHeight / TileSprite.bounds.size.y, 1);
                SpriteRenderer sprite = newMapSquare.AddComponent<SpriteRenderer>();
                sprite.color = Color.white;
                sprite.sprite = TileSprite;

                GameObject newMapSquareMarker = new GameObject("Marker");
                newMapSquareMarker.transform.SetParent(newMapSquare.transform);
                newMapSquareMarker.transform.localPosition = new Vector3(0, 0, 0);
                newMapSquareMarker.transform.localScale = new Vector3((TileWidth / P1MarkerSprite.bounds.size.x) * (1 / newMapSquare.transform.localScale.x), (TileHeight / P1MarkerSprite.bounds.size.y) * (1 / newMapSquare.transform.localScale.y), 1);
                SpriteRenderer markerSprite = newMapSquareMarker.AddComponent<SpriteRenderer>();
                markerSprite.color = Color.white;
                markerSprite.sprite = null;
                markerSprite.sortingOrder = 1;

                MapTile newTile = newMapSquare.AddComponent<MapTile>();
                TileMap[i].Add(newTile);
                newTile.X = i;
                newTile.Y = j;
                newTile.ParentMap = this;

                newTile.Marker = markerSprite;

                if (Random.Range(0.0f, 1.0f) < percentBombs) {
                    newTile.HasBomb = true;
                }
            }
        }

        PlayerStartTiles = new List<MapTile>();
        int middle = height / 2;
        for (int i = -1 * NumberOfPlayers / 2; i < NumberOfPlayers / 2; i++) {
            TileMap[0][middle + i].HasBomb = false;
            TileMap[0][middle + i].Explore(null);
            TileMap[width - 1][middle + i].HasBomb = false;
            TileMap[width - 1][middle + i].IsGoal = true;

            PlayerStartTiles.Add(TileMap[0][middle + i]);
        }


    }

    public MapTile GetTile(int x, int y) {
        if (x < 0 || x >= TileMap.Count) return null;
        if (y < 0 || y >= TileMap[0].Count) return null;
        return TileMap[x][y];
    }


}
