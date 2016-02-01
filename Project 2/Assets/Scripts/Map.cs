using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Map : MonoBehaviour {
    public class Tile {
        public int X;
        public int Y;

        public bool IsTraversable = true;
        public bool HasBomb = false;
        public bool IsGoal = false;
    }

    public List<List<Tile>> TileMap;
    public Canvas MapCanvas;

    public void Initialize(int width, int height) {
        TileMap = new List<List<Tile>>();
        for (int i=0; i< width; i++) {
            TileMap.Add(new List<Tile>());
            for (int j=0; j< height; j++) {
                TileMap[i].Add(new Tile());
                TileMap[i][j].X = i;
                TileMap[i][j].Y = j;
            }
        }

        for (int i=0; i<=height; i++) {
            GameObject newGridLineGO = new GameObject("Grid Line Horizontal " + i);
            RectTransform rt = newGridLineGO.AddComponent<RectTransform>();
            rt.SetParent(MapCanvas.transform);
            rt.anchorMin = new Vector2(0, (float)i / height);
            rt.anchorMax = new Vector2(1, (float)i / height);
            rt.offsetMin = new Vector2(0, rt.offsetMin.y);
            rt.offsetMax = new Vector2(0, rt.offsetMax.y);
            rt.anchoredPosition = new Vector2(0, 0);
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, 3);
            rt.localScale = Vector3.one;
                
            Image newLine = newGridLineGO.AddComponent<Image>();
            newLine.color = Color.black;
        }

        for (int i = 0; i <= width; i++) {
            GameObject newGridLineGO = new GameObject("Grid Line Vertical " + i);
            RectTransform rt = newGridLineGO.AddComponent<RectTransform>();
            rt.SetParent(MapCanvas.transform);
            rt.anchorMin = new Vector2((float)i / width, 0);
            rt.anchorMax = new Vector2((float)i / width, 1);
            rt.offsetMin = new Vector2(rt.offsetMin.x, 0);
            rt.offsetMax = new Vector2(rt.offsetMax.x, 0);
            rt.anchoredPosition = new Vector2(0, 0);
            rt.sizeDelta = new Vector2(3, rt.sizeDelta.y);
            rt.localScale = Vector3.one;

            Image newLine = newGridLineGO.AddComponent<Image>();
            newLine.color = Color.black;
        }
    }

    public Tile GetTile(int x, int y) {
        return TileMap[x][y];
    }
}
