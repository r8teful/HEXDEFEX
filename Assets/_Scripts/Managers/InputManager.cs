using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public static InputManager Instance;
    public static event Action<TouchPhase> OnTouchChanged; 
    public Vector3 positionInitial;
    private Vector2 touchPosition;

    private void Awake() {
        Instance = this;
    }
    private void OnApplicationQuit() {
        Instance = null;
        Destroy(gameObject);
    }
    private void Update() {

        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if (GameManager.Instance.State.Equals(GameState.Battle)) {
                // Logic for battle mode
                if ((touch.phase == TouchPhase.Began) && (positionInitial == Vector3.zero)) {
                    // Only do this thing once when you place down finger 
                    touchPosition.Set(touch.position.x - Screen.width * .5f, touch.position.y - Screen.height * .5f);
                    positionInitial = touchPosition;
                    OnTouchChanged?.Invoke(TouchPhase.Began);
                }
                if (touch.phase == TouchPhase.Moved) {
                    touchPosition.Set(touch.position.x - Screen.width * .5f, touch.position.y - Screen.height * .5f);
                    OnTouchChanged?.Invoke(TouchPhase.Moved);
                }
                if ((touch.phase == TouchPhase.Ended) || (touch.phase == TouchPhase.Canceled)) {
                    OnTouchChanged?.Invoke(TouchPhase.Ended);
                    positionInitial = Vector3.zero;
                }
            } else if (GameManager.Instance.State.Equals(GameState.Shop)) {
                // Shop input logic, mostly UI and object moving
            }
        }
    }
    public Vector2 GetTouchPosition() {
        return touchPosition;
    }
}