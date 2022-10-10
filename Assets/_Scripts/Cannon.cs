using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    [SerializeField] private GameObject enemy;
    [SerializeField] private float spawnDelay;
    [SerializeField] private float shootForce;

    void Start() {
       if(!GameManager.Instance.State.Equals(GameState.Battle)) return;
       StartCoroutine(SpawnEnemy());
    }
    private IEnumerator SpawnEnemy() {
        while (true) {
            GameObject localEnemy = Instantiate(enemy, transform.parent.position, transform.rotation);
            localEnemy.GetComponent<Rigidbody2D>().AddForce(transform.up*shootForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
