using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float turnspeed;
    private float angleInitial;
    private Quaternion angleInitalQ;
    private Vector3 positionInitial;
    private float input;
    private Quaternion angleTargetQ;
    private int inc;

    void Update() {
        input = Input.GetAxisRaw("Horizontal");

        if (Input.touchCount > 0) {  
            Touch touch = Input.GetTouch(0);
            if ((touch.phase == TouchPhase.Began) && (positionInitial == Vector3.zero)) {
                // Set new current, only once every time we have touched
                // -- TODO -- // Make it so that one of the six sides are sellected when touching close to it, no reason to make it exact
                Vector2 pos = touch.position;

                pos.x -= Screen.width / 2;
                pos.y -= Screen.height / 2;

                positionInitial = new Vector3(pos.x, pos.y, 0.0f);
                angleInitial = transform.rotation.eulerAngles.z;
                angleInitalQ = transform.rotation;
            }
            if (touch.phase == TouchPhase.Moved) {
                // Move the ship if the screen has the finger moving
                // Calculate new finger position and rotate the ship towards that position
                Vector2 pos = touch.position;
                pos.x -= Screen.width / 2;
                pos.y -= Screen.height / 2;

                float angle = Vector2.SignedAngle(positionInitial, pos);
                angleTargetQ = Quaternion.Euler(0, 0, angle + angleInitial);
                Debug.Log(angle + angleInitial);


                transform.rotation = Quaternion.Slerp(transform.rotation, angleTargetQ, 7.5f * Time.deltaTime);

                //var a = Mathf.MoveTowardsAngle(angleInitial, angle + angleInitial, 0.1f * Time.deltaTime);
                //transform.eulerAngles = new Vector3(0, 0, a);
                //transform.rotation = Quaternion.RotateTowards(angleInitalQ, angleTargetQ, 1000*Time.deltaTime);
                //transform.rotation = Quaternion.Euler(0,0, angleInitial + angle);
            }
            if ((touch.phase == TouchPhase.Ended) || (touch.phase == TouchPhase.Canceled)) {
                positionInitial = Vector3.zero;
            }
        }
       
        if (Input.GetKeyDown(KeyCode.Space)) {
            inc += 100;
            Debug.Log(inc);
        }

        //var a = Mathf.MoveTowardsAngle(transform.eulerAngles.z, inc, 10f * Time.deltaTime);
        //transform.eulerAngles = new Vector3(0, 0, a);
       // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, inc), 20 * Time.deltaTime);
        Debug.DrawRay(Vector3.zero, positionInitial*10, Color.red);
        
    }


    void FixedUpdate() {
        
        transform.Rotate(new Vector3(0,0,-input*turnspeed));
    }
}
