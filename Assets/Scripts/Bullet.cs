using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed;
    [SerializeField] private Rigidbody2D rb;
    

    // Start is called before the first frame update
    void Start() {
        rb.velocity = transform.up*speed;
        StartCoroutine(DestroyAfterSeconds());
    }

    private IEnumerator DestroyAfterSeconds() {
        // Destroy the object after x seconds
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            // Destroy Enemy & Bullet
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
