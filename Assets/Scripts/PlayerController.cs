using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float turnspeed;
    private float input;
    private void Update() {
        input = Input.GetAxisRaw("Horizontal");
    }
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0,0,-input*turnspeed));
    }
}
