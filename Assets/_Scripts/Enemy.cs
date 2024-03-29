using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private Rigidbody2D rigid2D;
    [SerializeField] private EnemyScriptableObject enemyData;
    private float myHP;
    private bool frozen;

    private void Awake() {
        rigid2D = GetComponent<Rigidbody2D>();
        myHP = enemyData.health;
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

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            ShipManager.Instance.EnemyHit(enemyData.damage);
            Destroy(gameObject);
        }
    }
    public void Hit(float dmg) {
        myHP -= dmg;
        //Debug.Log($"I got hit! Took {dmg} damage and current HP is now {myHP}!");
        if (myHP <= 0) {
            ShipManager.Instance.AddCurrency(enemyData.cost); // TODO you get the cost of the enemy as money
            Destroy(gameObject);
        }
    }

    public int GetEnemyCost() {
        return enemyData.cost;
    }
}
