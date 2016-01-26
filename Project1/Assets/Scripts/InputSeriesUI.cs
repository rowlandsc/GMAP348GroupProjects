using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InputSeriesUI : MonoBehaviour {

    public GameObject Timeline = null;
    public Font ActionTextFont;
    List<Image> _windowMarkers = new List<Image>();
    List<Text> _windowText = new List<Text>();
    Image _progressBar = null;

	void Start () {
        InputManager.OnStartInputSeries += OnInputSeriesStart;
        InputManager.OnStartInputSeriesWindow += OnInputWindowStart;
        InputManager.OnEndInputSeriesWindow += OnInputWindowEnd;
        InputManager.OnEndInputSeries += OnInputSeriesEnd;

        Timeline.SetActive(false);
	}
	
	void Update () {
	    if (_progressBar && _progressBar.rectTransform.anchorMax.x < 1) {
            float adjust = (Time.deltaTime / InputManager.Instance.InputWindowLength) / InputManager.Instance.CurrentInputStringLength;
            _progressBar.rectTransform.anchorMax = new Vector2(_progressBar.rectTransform.anchorMax.x + adjust, _progressBar.rectTransform.anchorMax.y);
            if (_progressBar.rectTransform.anchorMax.x > 1) _progressBar.rectTransform.anchorMax = new Vector2(1, _progressBar.rectTransform.anchorMax.y);
        }
	}

    void OnInputSeriesStart() {
        Timeline.SetActive(true);

        for (int i = 0; i < _windowMarkers.Count; i++) {
            Destroy(_windowMarkers[i].gameObject);
        }
        for (int i=0; i<_windowText.Count; i++) {
            Destroy(_windowText[i].gameObject);
        }
        _windowMarkers.Clear();
        _windowText.Clear();
        if (_progressBar) Destroy(_progressBar.gameObject);

        for (int i=0; i<=InputManager.Instance.CurrentInputStringLength; i++) {
            GameObject newWindowMarker = new GameObject("WindowMarker " + i);
            newWindowMarker.transform.SetParent(Timeline.transform);
            newWindowMarker.transform.localScale = Vector3.one;
            Image newImage = newWindowMarker.AddComponent<Image>();

            newImage.rectTransform.anchorMin = new Vector2(((float) i) / (InputManager.Instance.CurrentInputStringLength) - 0.005f, -0.5f);
            newImage.rectTransform.anchorMax = new Vector2(((float) i) / (InputManager.Instance.CurrentInputStringLength) + 0.005f, 1.5f);
            newImage.rectTransform.offsetMin = new Vector2(0, 0);
            newImage.rectTransform.offsetMax = new Vector2(0, 0);
            newImage.color = Color.white;

            _windowMarkers.Add(newImage);
        }
        _windowMarkers[0].color = Color.black;   

        GameObject progressBarGO = new GameObject("Progress Bar");
        progressBarGO.transform.SetParent(Timeline.transform);
        progressBarGO.transform.localScale = Vector3.one;
        _progressBar = progressBarGO.AddComponent<Image>();

        for (int i = 0; i < InputManager.Instance.CurrentInputStringLength; i++) {
            GameObject newWindowText = new GameObject("WindowText " + i);
            newWindowText.transform.SetParent(Timeline.transform);
            newWindowText.transform.localScale = Vector3.one;
            Text newText = newWindowText.AddComponent<Text>();

            newText.rectTransform.anchorMin = new Vector2(_windowMarkers[i].rectTransform.anchorMax.x, 1.0f);
            newText.rectTransform.anchorMax = new Vector2(_windowMarkers[i + 1].rectTransform.anchorMin.x, 2.0f);
            newText.rectTransform.offsetMin = new Vector2(0, -29);
            newText.rectTransform.offsetMax = new Vector2(0, -29);
            newText.text = "";
            //newText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            newText.resizeTextForBestFit = true;
            newText.color = Color.black;
            newText.font = ActionTextFont;
            newText.alignment = TextAnchor.MiddleCenter;

            newText.resizeTextMinSize = 4;
            newText.resizeTextMaxSize = 20;

            _windowText.Add(newText);
        }

        _progressBar.rectTransform.anchorMin = new Vector2(0, .1f);
        _progressBar.rectTransform.anchorMax = new Vector2(0, .9f);
        _progressBar.rectTransform.offsetMin = new Vector2(0, 0);
        _progressBar.rectTransform.offsetMax = new Vector2(0, 0);
        _progressBar.color = Color.white;
    }
    void OnInputWindowStart() {

    }
    void OnInputWindowEnd() {
        _windowMarkers[InputManager.Instance.InputString.Count].color = Color.black;
    }
    void OnInputSeriesEnd() {
        
    }

    public void SetWindowText(int i, string text) {
        _windowText[i].text = text;
    }

    void OnDestroy() {
        InputManager.OnStartInputSeries -= OnInputSeriesStart;
        InputManager.OnStartInputSeriesWindow -= OnInputWindowStart;
        InputManager.OnEndInputSeriesWindow -= OnInputWindowEnd;
        InputManager.OnEndInputSeries -= OnInputSeriesEnd;
    }
}
