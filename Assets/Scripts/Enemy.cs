using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private Rigidbody2D rigid2D;
    public float moveSpeed;
    private Vector2 curPosition;


    private void Awake() {
        rigid2D = GetComponent<Rigidbody2D>();
    }
   
    void FixedUpdate()
    {
        moveTowardsShip();
        transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, rigid2D.velocity));
    }
    
    // Calculates in what direction the enemy has to move in order to get to the ship
    // Should be called each timestep unless the enemy has other intentions
    void moveTowardsShip() {
        rigid2D.AddForce(-100 * Time.fixedDeltaTime * transform.position.normalized);  // A constant force that is "pulling" the enemy towards the center of the screen   
        //transform.position = Vector3.Slerp(transform.position, Vector3.zero, .5f); // Doesn't work
        //transform.position = Vector2.MoveTowards(transform.position, Vector3.zero, Time.deltaTime * moveSpeed); // Normal, straight line towards the center
    }
   
    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, Vector3.zero);
    }
}
