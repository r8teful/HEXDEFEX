using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    private readonly int burstAmount = 3;
    private readonly float burstDelay = 0.2f;
    //public WeaponType weaponType;
    [SerializeField] private WeaponScriptableObject weaponData;

    [SerializeField] private GameObject bullet;
    public Stats Stats { get; private set; }
    // TODO TOMOROW. MAKE A SYSTEM THAT MAKES SENCE TO YOU
    private void Start() {
        if (!GameManager.Instance.State.Equals(GameState.Battle)) return;
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


    public virtual void SetStats(Stats stats) => Stats = stats;
}