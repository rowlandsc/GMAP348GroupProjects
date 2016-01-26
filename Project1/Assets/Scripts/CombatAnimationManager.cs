using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CombatAnimationManager : MonoBehaviour {

    public static CombatAnimationManager Instance = null;
    public void Awake() {
        if (!Instance) {
            Instance = this;
        }
        else {
            Destroy(this);
        }
    }

    public Canvas CanvasToUse;
    public Font FontToUse;
    public int FontSize;
    public float RiseDistance = 50;

    public GameObject CreateCombatAnimationObject(float lifetime, string text, bool leftSide, Color color, bool rise) {
        GameObject go = new GameObject("Combat Animation (" + text + ")");

        go.AddComponent<RectTransform>();
        go.transform.SetParent(CanvasToUse.transform);

        Vector2 position;
        if (leftSide) {
            position = Camera.main.WorldToViewportPoint(Player.Instance.transform.position);
        }
        else {
            position = Camera.main.WorldToViewportPoint(CombatSystem.Instance.CurrentEnemy.transform.position);
            
        }
        position.y += 0.1f;
        go.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        go.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        go.GetComponent<RectTransform>().localScale = Vector3.one;
        go.GetComponent<RectTransform>().anchorMin = position;
        go.GetComponent<RectTransform>().anchorMax = position;
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(1280, 720);

        Text t = go.AddComponent<Text>();
        t.text = text;
        t.color = color;
        t.font = FontToUse;
        t.fontSize = FontSize;
        t.alignment = TextAnchor.MiddleCenter;

        Outline ol = go.AddComponent<Outline>();
        ol.effectColor = Color.white;
        ol.effectDistance = new Vector2(2, 2);

        CombatAnimation ca = go.AddComponent<CombatAnimation>();
        ca.Lifetime = lifetime;
        ca.RiseOverTime = rise;

        return go;
    }



}
