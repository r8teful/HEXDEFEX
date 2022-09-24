using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    public float gunDelay;
    private int burstAmount = 3;
    private float burstDelay = 0.2f;
    public GunType guntype;
    [SerializeField] GameObject bullet;
    

    private void Start() {
        switch (guntype) {
            case GunType.Single:
                StartCoroutine(spawnBulletsSingle());
                break;
            case GunType.Burst:
                StartCoroutine(spawnBulletsBurst());
                break;
            case GunType.Multi:
                StartCoroutine(spawnBulletsMulti());
                break;
            default:
                throw new NotImplementedException();
        }
    }

    // -----------------Single--------------------- //  
    private IEnumerator spawnBulletsSingle() {
        while (true) {
            Instantiate(bullet, transform.position, transform.rotation);
            yield return new WaitForSeconds(gunDelay);
        }
    }
    // -----------------Single--------------------- // 

    // -----------------Burst--------------------- // 
    private IEnumerator spawnBulletsBurst() {
        while (true) {
            StartCoroutine(ShootBurst());
            yield return new WaitForSeconds(gunDelay*2);
        }
    }
    private IEnumerator ShootBurst() {
        for (int i = 0; i < burstAmount; i++) {
            Instantiate(bullet, transform.position,transform.rotation);
            yield return new WaitForSeconds(burstDelay);
        }
    }
    // -----------------Burst--------------------- // 


    // -----------------Multi--------------------- // 
    private IEnumerator spawnBulletsMulti() {
        while (true) {
            var offsetLeft = Quaternion.Euler(0,0,transform.rotation.eulerAngles.z - 30);
            var offsetRight = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 30);
            Instantiate(bullet, transform.position, transform.rotation);
            Instantiate(bullet, transform.position,offsetLeft);
            Instantiate(bullet, transform.position,offsetRight);
            yield return new WaitForSeconds(gunDelay*2);
        }
    }

    // -----------------Multi--------------------- // 

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            transform.Rotate(new Vector3(0,0,-1));
        }
    }
}