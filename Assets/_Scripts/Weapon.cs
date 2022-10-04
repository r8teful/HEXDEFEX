using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    private readonly int burstAmount = 3;
    private readonly float burstDelay = 0.2f;
    private int posPrefered;
    //public WeaponType weaponType;
    [SerializeField] private WeaponScriptableObject weaponData;
    [SerializeField] private GameObject bullet;
    private bool sellected;

    public Stats Stats { get; private set; }

    private void Awake() {
        // Subscribe to GameState Event
        GameManager.OnGameStateChanged += GameStateChanged;
    }
    private void OnDestroy() => GameManager.OnGameStateChanged -= GameStateChanged;


    private void GameStateChanged(GameState t) {
        if (t.Equals(GameState.Shop)) {
            // Dont shoot guns in shop
            StopAllCoroutines();
            // But change weapon pos if it gets moved
            StartCoroutine(MoveWeapon());
        } else if (t.Equals(GameState.Battle)) {
            StartShooting();
        }
    }

    private void Start() {
        if (GameManager.Instance.State.Equals(GameState.Battle)) StartShooting();
        if (GameManager.Instance.State.Equals(GameState.Shop)) StartCoroutine(MoveWeapon());
    }

    private void StartShooting() {
        switch (weaponData.WeaponType) {
            case WeaponType.Single:
                StartCoroutine(SpawnBulletsSingle());
                break;
            case WeaponType.Burst:
                StartCoroutine(SpawnBulletsBurst());
                break;
            case WeaponType.Multi:
                StartCoroutine(SpawnBulletsMulti());
                break;
            default:
                throw new NotImplementedException();
        }
    }

    // -----------------Single--------------------- //  
    private IEnumerator SpawnBulletsSingle() {
        while (true) {
            Instantiate(bullet, transform.position, transform.rotation).GetComponent<Bullet>().SetData(weaponData);
            yield return new WaitForSeconds(1/weaponData.BaseStats.fireRate);
        }
    }
    // -----------------Single--------------------- // 

    // -----------------Burst--------------------- // 
    private IEnumerator SpawnBulletsBurst() {
        while (true) {
            StartCoroutine(ShootBurst());
            yield return new WaitForSeconds(1/weaponData.BaseStats.fireRate);
        }
    }
    private IEnumerator ShootBurst() {
        for (int i = 0; i < burstAmount; i++) {
            Instantiate(bullet, transform.position,transform.rotation).GetComponent<Bullet>().SetData(weaponData);
            yield return new WaitForSeconds(burstDelay);
        }
    }
    // -----------------Burst--------------------- // 


    // -----------------Multi--------------------- // 
    private IEnumerator SpawnBulletsMulti() {
        while (true) {
            var offsetLeft = Quaternion.Euler(0,0,transform.rotation.eulerAngles.z - 30);
            var offsetRight = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 30);
            Instantiate(bullet, transform.position,transform.rotation).GetComponent<Bullet>().SetData(weaponData);
            Instantiate(bullet, transform.position,offsetLeft).GetComponent<Bullet>().SetData(weaponData);
            Instantiate(bullet, transform.position,offsetRight).GetComponent<Bullet>().SetData(weaponData);
            yield return new WaitForSeconds(1/ weaponData.BaseStats.fireRate);
        }
    }
    // -----------------Multi--------------------- // 


    public IEnumerator MoveWeapon() {
        // This function makes handles the movement of the weapons on the ship. Each weapon has its "desired" position, being the slot it is suposed to be in. This can be updated
        // from other scripts depending on how the player moves them in the shop. 
        while (true) {
            if ((!sellected) && (transform.position != WeaponManager.Instance.weaponPos[posPrefered])) {
                transform.position = Vector3.Slerp(transform.position, WeaponManager.Instance.weaponPos[posPrefered], 10f * Time.deltaTime);
                transform.up = Vector3.Slerp(transform.up,transform.position, 40f * Time.deltaTime);
            } else {
                //yield break;
            }
            yield return null;
        }
    }

    public void SetposPrefered(int value) {
        posPrefered = value;
    }
    public void Setsellected(bool value) {
        sellected = value;
    }

    public virtual void SetStats(Stats stats) => Stats = stats;
}