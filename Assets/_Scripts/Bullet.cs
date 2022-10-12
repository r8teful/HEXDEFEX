using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private Rigidbody2D rigid2D;
    private WeaponScriptableObject myData;
    private BulletType bulletType;


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
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            var hitEnemy = collision.gameObject.GetComponent<Enemy>();
            // Destroy Enemy
            // TODO -- TAKE HEALTH INTO ACCOUNT ETC.
            hitEnemy.Hit(myData.BaseStats.bulletDamage);
            if (bulletType == BulletType.Freeze) {
                Debug.Log("FREEZEE!!");
                hitEnemy.Freeze(3.5f);
            } else {
                hitEnemy.AddKnockbackForce(myData.BaseStats.knockback, rigid2D.velocity.normalized);
            }

            //Destroy(collision.gameObject);
            //Destroy(this);
            Destroy(gameObject);
        }
    }

    public void SetData(WeaponScriptableObject data) {
        myData = data;
    }
    public void SetBulletType(BulletType type) {
        bulletType = type;
    }
}

[Serializable]
public enum BulletType {
    Normal,
    Freeze,
    Exploding,
    Homing,
    Piercing,
    Scatter,
    Knockback
}