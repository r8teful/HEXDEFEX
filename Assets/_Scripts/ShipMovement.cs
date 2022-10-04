using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour {
    public float turnspeed;
    private float angleInitial;
    private float input;
    private Quaternion angleTargetQ;
    private void Awake() {
        // Subscribe to Input Event
        InputManager.OnTouchChanged += TouchStateChanged;
    }
    private void OnDestroy() {
        InputManager.OnTouchChanged -= TouchStateChanged;
    }
    private void TouchStateChanged(TouchPhase state) {
        if(state == TouchPhase.Began) {
            angleInitial = transform.rotation.eulerAngles.z;
        } else if ((state == TouchPhase.Moved) || (state == TouchPhase.Stationary)) {
            float angle = Vector2.SignedAngle(InputManager.Instance.GetPositionInitial(), InputManager.Instance.GetTouchPosition());
            angleTargetQ = Quaternion.Euler(0, 0, angle + angleInitial);
            transform.rotation = Quaternion.Slerp(transform.rotation, angleTargetQ, 7.5f * Time.deltaTime);
        } else if (state == TouchPhase.Ended) {
            // Should we have something here?
        }
    }
}
