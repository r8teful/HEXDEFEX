using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float moveSpeed;
    private Vector2 curPosition;
    void FixedUpdate()
    {
        moveTowardsShip();
    }
    

    // Calculates in what direction the enemy has to move in order to get to the ship
    // Should be called each timestep unless the enemy has other intentions
    void moveTowardsShip() {
        transform.position = Vector2.MoveTowards(transform.position, Vector3.zero, Time.deltaTime * moveSpeed);
    }
    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, Vector3.zero);
    }
}
