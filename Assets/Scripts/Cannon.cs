using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    [SerializeField] private GameObject enemy;
    public float spawnDelay;
    private Rigidbody2D rigid2D;

    void Start() {
       
        StartCoroutine(SpawnEnemy());
    }

    void Update() {
    }
    private IEnumerator SpawnEnemy() {
        while (true) {
            GameObject localEnemy = Instantiate(enemy, transform.position, transform.rotation);
            localEnemy.GetComponent<Rigidbody2D>().AddForce(transform.up, ForceMode2D.Impulse);

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
