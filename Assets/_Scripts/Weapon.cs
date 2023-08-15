using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour {

    private protected int posPrefered;
    //public WeaponType weaponType;
    [SerializeField] private protected WeaponScriptableObject weaponData;
    [SerializeField] private protected GameObject bulletDefault; // protect: only classes that enherit can access
    private List<Weapon> neighbors = new List<Weapon>();

    //public abstract void Shoot(Quaternion orientation);//abstract: Enherit class should implement it
    private bool sellected;
    private float damageMultiplier;

    public Stats Stats { get; private set; }

    private protected virtual void Awake() {
        // Subscribe to GameState Event
        GameManager.OnGameStateChanged += GameStateChanged;
        ShootModeShooter sms = gameObject.AddComponent<ShootModeShooter>() as ShootModeShooter;
        sms.ShootMode = weaponData.shootMode;

    }
    private void OnDestroy() => GameManager.OnGameStateChanged -= GameStateChanged;

    public virtual void Shoot(Quaternion orientation) { // Virtual: Can be overwritten in devired child classes
        Instantiate(bulletDefault, transform.position, orientation).GetComponent<Bullet>().SetData(weaponData);
    }

    private void GameStateChanged(GameState t) {
        if (t.Equals(GameState.Shop)) {
            //StopAllCoroutines(); // Dont thing we need this weapons should already be in their positions
            // But change weapon pos if it gets moved
            StartCoroutine(MoveWeapon());
        }
    }

    private void Start() {
        if (GameManager.Instance.State.Equals(GameState.Shop)) StartCoroutine(MoveWeapon());
    }


    public IEnumerator MoveWeapon() {
        // This function makes handles the movement of the weapons on the ship. Each weapon has its "desired" position, being the slot it is suposed to be in. This can be updated
        // from other scripts depending on how the player moves them in the shop. 
        while (true) {
            if (posPrefered >= 0 && (!sellected) && (transform.position != WeaponManager.Instance.weaponPos[posPrefered])) {
                // Weapon is on ship and still not on prefered position
                transform.position = Vector3.Slerp(transform.position, WeaponManager.Instance.weaponPos[posPrefered], 10f * Time.deltaTime);
                transform.up = Vector3.Slerp(transform.up,transform.position, 40f * Time.deltaTime);
            } else if ((posPrefered >= -3 && posPrefered < 0) && (!sellected) && (transform.position != IUManagerScreen.Instance.shopPos[posPrefered + 3])) {
                // Weapon is in shop and not in prefered position
                transform.position = Vector3.Slerp(transform.position, IUManagerScreen.Instance.shopPos[posPrefered+3], 10f * Time.deltaTime);
               // transform.up = Vector3.Slerp(transform.up, transform.position, 40f * Time.deltaTime);
            }
            yield return null;
        }
    }

    // Method to apply buffs based on neighbors 90% of the fun in this game will lay in this function 
    /*
    public void ApplyBuffsFromNeighbors() {
        float damageMultiplier = 1.0f;

        foreach (Weapon neighborWeapon in neighbors) {
            if (neighborWeapon.weaponData.weaponClass == weaponData.weaponClass) {
                // Apply damage boost logic here todo make boost depend on what class 
                damageMultiplier += 0.20f;
            }
        }
        // Create new stats
        var stats = weaponData.BaseStats;
        // Apply the damage multiplier to the weapon's damage
        stats.bulletDamage *= damageMultiplier;
        // Apply other buffs based on the logic you have
        SetStats(stats);
    }*/




    public void ApplyBuffsFromNeighbors() {
        
        int numValidNeighbors = 0;
        WeaponClass? n1, n2;
        // Check wierd edge cases, 0 neigbors is 5 and 1. 5 has neighbor 0 and 4
        if(posPrefered == 0) {
            n1 = WeaponManager.Instance.GetWeaponClassByPosPrefered(5);
            n2 = WeaponManager.Instance.GetWeaponClassByPosPrefered(1);
        } else if (posPrefered==5) {
            n1 = WeaponManager.Instance.GetWeaponClassByPosPrefered(0);
            n2 = WeaponManager.Instance.GetWeaponClassByPosPrefered(4);
        } else {
            n1 = WeaponManager.Instance.GetWeaponClassByPosPrefered(posPrefered-1);
            n2 = WeaponManager.Instance.GetWeaponClassByPosPrefered(posPrefered+1);
        }
       // if (n1 == null && n2 == null) return; // no neighbors :(

        if (n1 == weaponData.weaponClass || n2 == weaponData.weaponClass) {
            // atleast one neighbor
            numValidNeighbors = 1;
        } if (n1 == weaponData.weaponClass && n2 == weaponData.weaponClass) {
            // two valid neihbors
            numValidNeighbors = 2;
        }

        //if (numValidNeighbors != previousNeighborCount) {
        //    damageMultiplier = CalculateDamageMultiplier(numValidNeighbors);
        //    previousNeighborCount = numValidNeighbors;
        //} else {
        //    damageMultiplier = 1.0f;
        //}
        damageMultiplier = CalculateDamageMultiplier(numValidNeighbors);
        // Create new stats
        var stats = weaponData.BaseStats;
        // Apply the damage multiplier to the weapon's damage
        stats.bulletDamage *= damageMultiplier;
        // Apply other buffs based on the logic you have
        Debug.Log($"I'm {gameObject.name} My Bullet damage is ({stats.bulletDamage})");

        SetStats(stats);
    }

    private float CalculateDamageMultiplier(int numValidNeighbors) {
        if (numValidNeighbors == 1) return 1.2f;
        if (numValidNeighbors >= 2) return 1.5f;
        return 1.0f;
    }


    public void SetPosPrefered(int value) {
        //Debug.Log($"Setting prefered pos to: {value}");
        posPrefered = value;
    }
    public void SetSellected(bool value) {
        sellected = value;
    }

    public int GetPosPrefered() {
        return posPrefered;
    }
    public WeaponScriptableObject GetWeaponData() {
        return weaponData;
    }

    public virtual void SetStats(Stats stats) => Stats = stats;

}