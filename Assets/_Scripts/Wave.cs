using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wave : MonoBehaviour {
    // General wave specific behaviour for one wave only 
    // This is the script that will be attached to the wave object

    [SerializeField] private WaveScriptableObject waveData;
    public List<Enemy> enemyList; // todo make private

    public void StartWave() {
        Debug.Log("StartWave");
        enemyList = DetermineEnemies();
        // Switch case for pattern enum
        switch (waveData.pattern) {
            case Pattern.Point:
                StartCoroutine(SpawnEnemiesPoint());
                break;
            case Pattern.Cluster:
                StartCoroutine(SpawnEnemiesCluster());
                break;
            case Pattern.Line:
                StartCoroutine(SpawnEnemiesLine());
                break;
            case Pattern.Arc:
                StartCoroutine(SpawnEnemiesArc());
                break;
            case Pattern.Sync:
                StartCoroutine(SpawnEnemiesSync());
                break;
            default:
                Debug.Log("No pattern found");
                break;
        }

       // StartCoroutine(SpawnEnemiesPoint());
    }

    // Could have these in seperate classes but for now they are here
    private IEnumerator SpawnEnemiesSync() {
        throw new System.NotImplementedException();
    }

    private IEnumerator SpawnEnemiesArc() {
        throw new System.NotImplementedException();
    }

    private IEnumerator SpawnEnemiesLine() {
        throw new System.NotImplementedException();
    }

    private IEnumerator SpawnEnemiesCluster() {
        throw new System.NotImplementedException();
    }

    private IEnumerator SpawnEnemiesPoint() {
        // Calculate spawnDelay
        float spawnDelay = waveData.waveDuration / enemyList.Count;
        while (enemyList.Count>0) {
            // Get random spawn point
            Vector2 spawnPoint = GetNewSpawnPoint();
            int randomIndex = Random.Range(0, enemyList.Count);
            Instantiate(enemyList[randomIndex], spawnPoint, Quaternion.identity, transform);
            enemyList.RemoveAt(randomIndex);
            yield return new WaitForSeconds(spawnDelay);
        }
        Debug.Log("Wave finished!");
    }


    private Vector2 GetNewSpawnPoint() {
        var spawnAngle = Random.Range(0, 359);
        var x = Mathf.Cos(spawnAngle * Mathf.Deg2Rad) * 5; // TODO 5 IS BAD SHOULD DEPEND ON SCREENSIZE
        var y = Mathf.Sin(spawnAngle * Mathf.Deg2Rad) * 5;
        return new Vector2(x, y);
    }

    // Before AI 
    private List<Enemy> DetermineEnemies() {
        List<Enemy> enemyList = new List<Enemy>();
        // Keep picking enemies from posibleEnemies list until im out of currency
        int currency = waveData.waveStrength;
        while (currency > 0) {
            var enemy = waveData.posibleEnemies[Random.Range(0, waveData.posibleEnemies.Count)]; // Can be made weighted somehow? Eg easier enemies are more likely to be picked
            if (enemy.GetEnemyCost() <= currency) {
                currency -= enemy.GetEnemyCost();
                enemyList.Add(enemy);
            }
        }
        return enemyList;
    }
     // After
    private List<Enemy> DetermineEnemiesAI() {
        List<Enemy> enemyList = new List<Enemy>();
        int currency = waveData.waveStrength;

        // Create a list of available enemies that fit within the remaining currency
        List<Enemy> affordableEnemies = waveData.posibleEnemies
            .Where(enemy => enemy.GetEnemyCost() <= currency)
            .ToList();

        while (currency > 0 && affordableEnemies.Count > 0) {
            var randomIndex = Random.Range(0, affordableEnemies.Count);
            var selectedEnemy = affordableEnemies[randomIndex];

            currency -= selectedEnemy.GetEnemyCost();
            enemyList.Add(selectedEnemy);

            // Remove the selected enemy from the affordableEnemies list
            affordableEnemies.RemoveAt(randomIndex);
        }

        return enemyList;
    }
}
