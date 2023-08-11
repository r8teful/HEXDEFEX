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
    //public abstract void Shoot(Quaternion orientation);//abstract: Enherit class should implement it
    private bool sellected;
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

    public void SetposPrefered(int value) {
        //Debug.Log($"Setting prefered pos to: {value}");
        posPrefered = value;
    }
    public void Setsellected(bool value) {
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