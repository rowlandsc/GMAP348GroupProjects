using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {

    public delegate void StartInputSeriesAction();
    public delegate void StartInputSeriesWindowAction();
    public delegate void EndInputSeriesWindowAction();
    public delegate void EndInputSeriesAction();
    public enum InputType { NONE, PRESS, DOUBLETAP, HOLD, RELEASE };

    public static InputManager Instance = null;
    public static event StartInputSeriesAction OnStartInputSeries;
    public static event StartInputSeriesWindowAction OnStartInputSeriesWindow;
    public static event EndInputSeriesWindowAction OnEndInputSeriesWindow;
    public static event EndInputSeriesAction OnEndInputSeries;

    string InputButton = "OneButton";
    public float InputWindowLength = 1.0f;
    public List<InputType> InputString = new List<InputType>();
    public bool IsGettingInputString = false;
    public int CurrentInputStringLength = 0;

    
    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this);
        }
    } 

	void Start () {
        OnStartInputSeries += TestNewInputSeries;
        OnStartInputSeriesWindow += TestNewInputWindow;
        OnEndInputSeriesWindow += TestEndInputWindow;
        OnEndInputSeries += TestEndInputSeries;
	}
	
	void Update () {
	   
	}

    public IEnumerator GenerateNewInputSeries(int length) {
        IsGettingInputString = true;
        CurrentInputStringLength = length;
        InputString.Clear();

        if (OnStartInputSeries != null) OnStartInputSeries();

        int currentWindow = 0;
        bool currentlyHeld = false;
        while (currentWindow < length) {
            if (OnStartInputSeriesWindow != null) OnStartInputSeriesWindow();

            float windowTime = 0;
            int timesPressed = 0;
            int timesReleased = 0;
            bool startHeld = (InputString.Count > 0 && InputString[InputString.Count - 1] == InputType.HOLD);
            while (windowTime < InputWindowLength) {
                if (Input.GetAxisRaw(InputButton) > float.Epsilon && !currentlyHeld) {
                    currentlyHeld = true;
                    timesPressed++;
                }
                else if (Input.GetAxisRaw(InputButton) <= float.Epsilon && currentlyHeld) {
                    currentlyHeld = false;
                    timesReleased++;
                }

                windowTime += Time.deltaTime;
                yield return null;
            }

            
            if (startHeld && timesReleased > 0) {
                InputString.Add(InputType.RELEASE);
            }
            else if (timesPressed > timesReleased || (timesPressed == timesReleased && startHeld)) {
                InputString.Add(InputType.HOLD);
            }
            else if (timesPressed == 1 && timesReleased == 1) {
                InputString.Add(InputType.PRESS);
            }
            else if (timesPressed > 1 && timesReleased > 1) {
                InputString.Add(InputType.DOUBLETAP);
            }
            else {
                InputString.Add(InputType.NONE);
            }

            if (OnEndInputSeriesWindow != null) OnEndInputSeriesWindow();

            currentWindow++;
        }

        if (OnEndInputSeries != null) OnEndInputSeries();

        IsGettingInputString = false;
        CurrentInputStringLength = 0;
    }

    public void TestNewInputSeries() {
        Debug.Log("New input series");
    }
    public void TestNewInputWindow() {
        Debug.Log("New input window");
    }
    public void TestEndInputWindow() {
        Debug.Log("End input window");
    }
    public void TestEndInputSeries() {
        Debug.Log("End input series");
    }
}
