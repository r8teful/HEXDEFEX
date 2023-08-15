using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles shoot mode for weapons
public class ShootModeShooter : MonoBehaviour {

    public ShootMode ShootMode { get; set; }
    private  Weapon thisWeapon;
    private void Awake() {
        GameManager.OnGameStateChanged += GameStateChanged;
    }
    private void Start() {
        thisWeapon = gameObject.GetComponent<Weapon>();
        if (GameManager.Instance.State.Equals(GameState.Battle)) StartShooting();

    }
    private void OnDestroy() {
        GameManager.OnGameStateChanged -= GameStateChanged;
    }
    private void GameStateChanged(GameState t) {
        if (t.Equals(GameState.Shop)) {
            // Dont shoot guns in shop
            StopAllCoroutines();
        } else if (t.Equals(GameState.Battle)) {
            StartShooting();
        }
    }


    private void StartShooting() {
        switch (ShootMode) {
            case ShootMode.Single:
                StartCoroutine(SpawnBulletsSingle());
                break;
            case ShootMode.Burst:
                StartCoroutine(SpawnBulletsBurst());
                break;
            case ShootMode.Tripple:
                StartCoroutine(SpawnBulletsMulti());
                break;
            default:
                throw new NotImplementedException();
        }
    }
    private IEnumerator SpawnBulletsSingle() {
        while (true) {
            thisWeapon.Shoot(gameObject.transform.rotation); // Tell the weapon attached to the gameobject to shoot its bullet
            yield return new WaitForSeconds(1 / thisWeapon.GetWeaponData().BaseStats.fireRate);
        }
    }



    // -----------------Burst--------------------- // 
    private IEnumerator SpawnBulletsBurst() {
        while (true) {
            StartCoroutine(ShootBurst());
            yield return new WaitForSeconds(1 / thisWeapon.GetWeaponData().BaseStats.fireRate);
        }
    }
    private IEnumerator ShootBurst() {
        // TODO CHANGE BURST 
        for (int i = 0; i < 3; i++) {
            thisWeapon.Shoot(gameObject.transform.rotation);
            yield return new WaitForSeconds(.1f);
        }
    }
    // -----------------Burst--------------------- // 


    private IEnumerator SpawnBulletsMulti() {
        while (true) {
            // BEware bullets will collide with eachother because they get spawned in the same spot (:
            thisWeapon.Shoot(gameObject.transform.rotation);
            thisWeapon.Shoot(Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z - 30)); // Offset left
            thisWeapon.Shoot(Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z + 30)); // Offset right
            yield return new WaitForSeconds(1 / thisWeapon.GetWeaponData().BaseStats.fireRate);
        }
    }
}
