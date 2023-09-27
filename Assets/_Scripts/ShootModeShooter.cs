using System;
using System.Collections;
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
            // TODO take accuracy into account
            var stats = thisWeapon.GetWeaponData().BaseStats;
            var shootAngle = Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z + GetAngle(stats.accuracy));
            thisWeapon.Shoot(shootAngle); // Tell the weapon attached to the gameobject to shoot its bullet
            yield return new WaitForSeconds(0.5f/stats.fireRate);
        }
    }



    // -----------------Burst--------------------- // 
    private IEnumerator SpawnBulletsBurst() {
        while (true) {
            StartCoroutine(ShootBurst());
            yield return new WaitForSeconds(thisWeapon.GetWeaponData().BaseStats.fireRate);
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
            yield return new WaitForSeconds(thisWeapon.GetWeaponData().BaseStats.fireRate);
        }
    }


    static float GetAngle(float accuracy) {
        // Calculate the maximum deviation from 0 based on the accuracy
        // 0 accuracy is 45 degrees, 1.5 accuracy is 0 degrees
        float maxDeviation = 45 - 30 * accuracy;
        // If the accuracy is out of range, set the maxDeviation to the extreme values
        if (accuracy < 0) {
            maxDeviation = 45;
        } else if (accuracy > 1.5) {
            maxDeviation = 0;
        }

        // Generate a random angle between -maxDeviation and maxDeviation
        var angle = UnityEngine.Random.Range(-maxDeviation, maxDeviation);
        Debug.Log("Angle: " + angle);
        return angle; 
    }
}



