using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InputSeriesUI : MonoBehaviour {

    public GameObject Timeline = null;
    List<Image> _windowMarkers = new List<Image>();
    Image _progressBar = null;

	void Start () {
        InputManager.OnStartInputSeries += OnInputSeriesStart;
        InputManager.OnStartInputSeriesWindow += OnInputWindowStart;
        InputManager.OnEndInputSeriesWindow += OnInputWindowEnd;
        InputManager.OnEndInputSeries += OnInputSeriesEnd;
        	
	}
	
	void Update () {
	    if (_progressBar && _progressBar.rectTransform.anchorMax.x < 1) {
            float adjust = (Time.deltaTime / InputManager.Instance.InputWindowLength) / InputManager.Instance.CurrentInputStringLength;
            _progressBar.rectTransform.anchorMax = new Vector2(_progressBar.rectTransform.anchorMax.x + adjust, _progressBar.rectTransform.anchorMax.y);
            if (_progressBar.rectTransform.anchorMax.x > 1) _progressBar.rectTransform.anchorMax = new Vector2(1, _progressBar.rectTransform.anchorMax.y);
        }
	}

    void OnInputSeriesStart() {
        for (int i=0; i<=InputManager.Instance.CurrentInputStringLength; i++) {
            GameObject newWindowMarker = new GameObject("WindowMarker " + i);
            newWindowMarker.transform.SetParent(Timeline.transform);
            Image newImage = newWindowMarker.AddComponent<Image>();

            newImage.rectTransform.anchorMin = new Vector2(((float) i) / (InputManager.Instance.CurrentInputStringLength) - 0.005f, -0.5f);
            newImage.rectTransform.anchorMax = new Vector2(((float) i) / (InputManager.Instance.CurrentInputStringLength) + 0.005f, 1.5f);
            newImage.rectTransform.offsetMin = new Vector2(0, 0);
            newImage.rectTransform.offsetMax = new Vector2(0, 0);
            newImage.color = Color.red;

            _windowMarkers.Add(newImage);
        }
        _windowMarkers[0].color = Color.white;

        GameObject progressBarGO = new GameObject("Progress Bar");
        progressBarGO.transform.SetParent(Timeline.transform);
        progressBarGO.transform.localScale = Vector3.one;
        _progressBar = progressBarGO.AddComponent<Image>();

        _progressBar.rectTransform.anchorMin = new Vector2(0, .1f);
        _progressBar.rectTransform.anchorMax = new Vector2(0, .9f);
        _progressBar.rectTransform.offsetMin = new Vector2(0, 0);
        _progressBar.rectTransform.offsetMax = new Vector2(0, 0);
        _progressBar.color = Color.white;
    }
    void OnInputWindowStart() {

    }
    void OnInputWindowEnd() {
        _windowMarkers[InputManager.Instance.InputString.Count].color = Color.white;
    }
    void OnInputSeriesEnd() {

    }
}
