using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private Rigidbody2D rigid2D;
    [SerializeField] private EnemyScriptableObject enemyData;
    private bool frozen;

    private void Awake() {
        rigid2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {

        if (!frozen) { 
            moveTowardsShip();
        
        } else {
            Debug.Log("Moviing to fast!");
        }
        // Rotate towards the centre
        transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, rigid2D.velocity));
    }

    // Calculates in what direction the enemy has to move in order to get to the ship
    // Should be called each timestep unless the enemy has other intentions
    void moveTowardsShip() {
        // More force the further away the enemy gets
        var pullforce = -enemyData.pullSpeed * Time.fixedDeltaTime * transform.position.normalized;
        rigid2D.AddForce(pullforce);  // A constant force that is "pulling" the enemy towards the center of the screen   
        //Debug.Log("Pulling with force: " + pullforce);
        //transform.position = Vector3.Slerp(transform.position, Vector3.zero, .5f); // Doesn't work
        //transform.position = Vector2.MoveTowards(transform.position, Vector3.zero, Time.deltaTime * moveSpeed); // Normal, straight line towards the center
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, Vector3.zero);
    }
    public void AddKnockbackForce(float force, Vector2 direction) {
        // Meh IDK if I like this, but it works?
        rigid2D.AddForce(force * direction, ForceMode2D.Impulse);
    }

    public void Freeze(float seconds) {
        if (!frozen) {
            
            StartCoroutine(FreezeForSeconds(seconds));
        }
    
    }
    private IEnumerator FreezeForSeconds(float seconds) {
        frozen = true;
        yield return new WaitForSeconds(seconds);
        frozen = false;
        yield break;
    }
}
