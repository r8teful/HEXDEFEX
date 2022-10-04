using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : StaticInstance<InputManager> {
    public static event Action<TouchPhase> OnTouchChanged; 
    public static event Action<GameObject> OnWeaponRelease; 
    private Vector3 positionInitial;
    private Vector2 touchPosition;
    private GameObject sellected = null;
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
                if ((touch.phase == TouchPhase.Moved) || (touch.phase == TouchPhase.Stationary)) {
                    touchPosition.Set(touch.position.x - Screen.width * .5f, touch.position.y - Screen.height * .5f);
                    OnTouchChanged?.Invoke(touch.phase);
                }
                if ((touch.phase == TouchPhase.Ended) || (touch.phase == TouchPhase.Canceled)) {
                    OnTouchChanged?.Invoke(TouchPhase.Ended);
                    positionInitial = Vector3.zero;
                }
            } else if (GameManager.Instance.State.Equals(GameState.Shop)) {
                 // Shop input logic, mostly UI and object moving
                 if (touch.phase == TouchPhase.Began) {
                     touchPosition.Set(touch.position.x, touch.position.y);
                     var worldTouchPos = Camera.main.ScreenToWorldPoint(touchPosition);
                     var hitRay = Physics2D.Raycast(worldTouchPos, -Vector3.forward);
                     if ((hitRay.collider != null) && hitRay.collider.gameObject.CompareTag("Weapon")) {
                         // Event. Mainly for weaponManager and UI stuff
                         sellected = hitRay.collider.gameObject;
                        sellected.GetComponent<Weapon>().Setsellected(true);
                     }
                 } else if ((sellected != null) && ((touch.phase == TouchPhase.Moved) || (touch.phase == TouchPhase.Stationary))) {
                    // A weapon is selected and we are moving
                    // Stop holding the weapon back
                    touchPosition.Set(touch.position.x, touch.position.y);
                    var toVec = new Vector3(Camera.main.ScreenToWorldPoint(touchPosition).x, Camera.main.ScreenToWorldPoint(touchPosition).y, 0);
                    sellected.transform.position = Vector3.Slerp(sellected.transform.position, toVec, 40f * Time.deltaTime);
                    // Rotates face to the middle. Based on a comment from a very generous person here:  http://answers.unity.com/comments/1482425/view.html 
                    sellected.transform.up = Vector3.Slerp(sellected.transform.up, sellected.transform.position, 40f * Time.deltaTime);
                 } else if ((sellected != null) && (touch.phase == TouchPhase.Ended)) {
                     // We released the selected weapon. Leave the rest of movement to the weaponmanager
                     OnWeaponRelease?.Invoke(sellected);
                     sellected.GetComponent<Weapon>().Setsellected(false);
                     sellected = null;
                 }
            }
        }
    }

    public Vector2 GetTouchPosition() {
        return touchPosition;
    }
    public Vector3 GetPositionInitial() {
        return positionInitial;
    }
}