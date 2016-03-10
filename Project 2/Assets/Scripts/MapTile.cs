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

    public bool P1Marked = false;
    public bool P2Marked = false;

    public SpriteRenderer Marker;

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

        if (P1Marked) {
            Marker.sprite = Map.Instance.P1MarkerSprite;
        }
        else if (P2Marked) {
            Marker.sprite = Map.Instance.P2MarkerSprite;
        }
        else {
            Marker.sprite = null;
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

    public void Explore(PlayerMovement player) {
        if (IsExplored) return;

        if (HasBomb) {
            HasBomb = false;
            Debug.Log("Stepped on bomb!");
            if (player != null) {
                player.Kill();
                player.Score += ScoreManager.Instance.SCORE_BOMB;
                ScoreManager.Instance.CreateScoreText(ScoreManager.Instance.Player2ScoreText, transform.position, ScoreManager.Instance.SCORE_BOMB);
            }

            ScreenShake.Instance.ShakeCamera();
            Instantiate(ParentMap.ExplosionPrefab, transform.position + new Vector3(0, 0, -1), transform.rotation);
            SoundManager.Instance.Explosion();
        }
        else {
            if (player != null) {
                player.Score += ScoreManager.Instance.SCORE_EXPLORE;
                SoundManager.Instance.Step();
            }
        }

        if (IsGoal) {
            player.Score += ScoreManager.Instance.SCORE_GOAL;
            ScoreManager.Instance.CreateScoreText(ScoreManager.Instance.Player2ScoreText, transform.position, ScoreManager.Instance.SCORE_GOAL);
            StartCoroutine(GameManager.Instance.GameOver());
        }

        IsExplored = true;

        P1Marked = false;
        P2Marked = false;

        MapTile left = ParentMap.GetTile(X - 1, Y);
        MapTile right = ParentMap.GetTile(X + 1, Y);
        MapTile up = ParentMap.GetTile(X, Y + 1);
        MapTile down = ParentMap.GetTile(X, Y - 1);
        if (left) left.IsVisible = true;
        if (right) right.IsVisible = true;
        if (up) up.IsVisible = true;
        if (down) down.IsVisible = true;
    }

    public void Mark(bool player1) {
        if (IsExplored) return;

        if (player1) {
            if (!P2Marked) {
                P1Marked = !P1Marked;
            }
        }
        else {
            if (!P1Marked) {
                P2Marked = !P2Marked;
            }
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(MapTile))]
public class MapTileCustomEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        MapTile tile = (MapTile)target;

        if (GUILayout.Button("Explore")) {
            tile.Explore(null);
        }

    }
}

#endif