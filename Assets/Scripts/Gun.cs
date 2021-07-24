using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    public float gunDelay;
    [SerializeField] GameObject bullet;
    private void OnDrawGizmos() {
       
    }
    private void Start() {
        StartCoroutine(spawnBullets());
    }

    private IEnumerator spawnBullets() {
        while (true) {
            Instantiate(bullet, transform.position, transform.rotation);
            yield return new WaitForSeconds(gunDelay); 
        }
    }
}
