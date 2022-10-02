using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private Rigidbody2D rigid2D;
    private WeaponScriptableObject myData;
    // Start is called before the first frame update
    void Start() {
        rigid2D = GetComponent<Rigidbody2D>();
        rigid2D.velocity = transform.up * myData.BaseStats.bulletSpeed;
        StartCoroutine(DestroyAfterSeconds());
    }

    private IEnumerator DestroyAfterSeconds() {
        // Destroy the object after x seconds
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            // Destroy Enemy
            // TODO -- TAKE HEALTH INTO ACCOUNT ETC.
            Destroy(collision.gameObject);
            // Destroy bullet
            Destroy(gameObject);
        }
    }

    public void SetData(WeaponScriptableObject data) {
        myData = data;
    }
}