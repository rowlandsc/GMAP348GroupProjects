using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Map : MonoBehaviour {

    public List<List<MapTile>> TileMap;
    public Canvas MapCanvas;
    public float TileWidth = 64;
    public float TileHeight = 64;
    public Sprite TileSprite;

    public void Initialize(int width, int height) {
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
                

                MapTile newTile = newMapSquare.AddComponent<MapTile>();
                TileMap[i].Add(newTile);
                newTile.X = i;
                newTile.Y = j;
                newTile.ParentMap = this;
            }
        }

        for (int i=0; i<=width; i++) {
            for (int j = 0; j <= height; j++) {
                
            }
        }
    }

    public MapTile GetTile(int x, int y) {
        return TileMap[x][y];
    }
}
