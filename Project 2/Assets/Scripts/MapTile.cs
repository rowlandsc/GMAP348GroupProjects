using UnityEngine;
using System.Collections;


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
        if (!IsTraversable) {
            _spriteRenderer.color = ParentMap.BoundaryColor;
        }
        else if (!IsVisible) {
            _spriteRenderer.color = ParentMap.NotVisibleColor;
        }
        else if (HasBomb) {
            _spriteRenderer.color = ParentMap.NextToBombColor;
        }
        else if (!IsExplored) {
            _spriteRenderer.color = ParentMap.NotExploredColor;
        }
        else {
            _spriteRenderer.color = ParentMap.ExploredColor;
        }
    }
}
