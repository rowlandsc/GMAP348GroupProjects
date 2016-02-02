using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapTile : MonoBehaviour {

    public int X;
    public int Y;
    public Map ParentMap;

    public bool IsVisible = false;
    public bool IsExplored = false;
    public bool IsTraversable = true;
    public bool HasBomb = false;
    public bool IsGoal = false;

    private SpriteRenderer _spriteRenderer;

    void Start() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (IsGoal) {
            _spriteRenderer.color = ParentMap.GoalColor;
        }
        else if (IsExplored) {
            _spriteRenderer.color = ParentMap.ExploredColor;
        }
        else if (!IsTraversable) {
            _spriteRenderer.color = ParentMap.BoundaryColor;
        }
        else if (!IsVisible) {
            _spriteRenderer.color = ParentMap.NotVisibleColor;
        }
        else if (IsNearBomb()) {
            _spriteRenderer.color = ParentMap.NextToBombColor;
        }
        else {
            _spriteRenderer.color = ParentMap.NotExploredColor;
        }       
    }

    public bool IsNearBomb() {
        if (HasBomb) return true;

        MapTile left = ParentMap.GetTile(X - 1, Y);
        MapTile right = ParentMap.GetTile(X + 1, Y);
        MapTile up = ParentMap.GetTile(X, Y + 1);
        MapTile down = ParentMap.GetTile(X, Y - 1);
        if (left && left.HasBomb) return true;
        if (right && right.HasBomb) return true;
        if (up && up.HasBomb) return true;
        if (down && down.HasBomb) return true;

        return false;
    }

    public void Explore() {
        IsExplored = true;
        if (HasBomb) {
            HasBomb = false;
            Debug.Log("Stepped on bomb!");
        }

        MapTile left = ParentMap.GetTile(X - 1, Y);
        MapTile right = ParentMap.GetTile(X + 1, Y);
        MapTile up = ParentMap.GetTile(X, Y + 1);
        MapTile down = ParentMap.GetTile(X, Y - 1);
        if (left) left.IsVisible = true;
        if (right) right.IsVisible = true;
        if (up) up.IsVisible = true;
        if (down) down.IsVisible = true;
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(MapTile))]
public class MapTileCustomEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        MapTile tile = (MapTile)target;

        if (GUILayout.Button("Explore")) {
            tile.Explore();
        }

    }
}

#endif