using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour {
    public float spawnDelay;
    [SerializeField] private CircleCollider2D spawnCircle;
    [SerializeField] private UnityEngine.Object enemy;
    private void Start() {
        StartCoroutine(SpawnEnemy());
    }
    void Update() {
    }

    private IEnumerator SpawnEnemy() {
        while (true) {
            GetNewSpawnPoint();
           yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void GetNewSpawnPoint() {
        var spawnAngle = Random.Range(0, 359);
        var x = Mathf.Cos(spawnAngle * Mathf.Deg2Rad) * spawnCircle.radius;
        var y = Mathf.Sin(spawnAngle * Mathf.Deg2Rad) * spawnCircle.radius;
        Instantiate(enemy,new Vector2(x, y), Quaternion.identity,transform);
    }
}
