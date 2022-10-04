using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script is used to manage the weapons at the 6 different spots on the ship
// Also manages and places down the ship
public class WeaponManager : StaticInstance<WeaponManager> {

    // The different rotation and position the gun has to have depending on the slot
    // private Vector2[] GunPositions = {new Vector2(0.3f,0.5f), new Vector2(0.6f,0), new Vector2(0.3f,-0.5f), 
    //new Vector2(-0.3f,-0.5f), new Vector2(-0.6f,0), new Vector2(-0.3f,0.5f)};
    public readonly Vector3[] weaponPos = { new Vector2(0f, 0.5f), new Vector2(0.433f, 0.25f), new Vector2(0.433f, -0.25f), new Vector2(-0.0f, -0.5f), new Vector2(-0.433f, -0.25f), new Vector2(-0.433f, 0.25f) };
    private readonly float[] gunRotationsFlatTop = { (0), (-60), (-120), (-180), (-240), (-300) };
    //private float[] GunRotations =          { (-30), (-90), (-150), (-210), (-270), (-330) };

    private GameObject[] weapons;
    private GameObject[] weaponClones;
    
    [SerializeField] private GameObject originalPlayer;
    private GameObject player;
    private Quaternion prevRotation;

    // Has to know when we go into shop so we can change the view
    protected override void Awake() {
        base.Awake();
        weapons = new GameObject[6];
        weaponClones = new GameObject[6];
        WeaponEntry(0, WeaponType.Single);
        WeaponEntry(2, WeaponType.Burst);
        WeaponEntry(1, WeaponType.Multi);
        SceneManager.sceneLoaded += SceneDoneLoading;
        InputManager.OnWeaponRelease += WeaponRelease;
    }
    private void OnDestroy() {
        SceneManager.sceneLoaded -= SceneDoneLoading;
        InputManager.OnWeaponRelease -= WeaponRelease;
    }

    private void SceneDoneLoading(Scene s, LoadSceneMode m) {
        // Scene changed, spawn weapons on correct positions 
        player = Instantiate(originalPlayer, Vector3.zero, Quaternion.identity);
        InstantiateWeapons();
    }
    private void WeaponRelease(GameObject o) {
        // Find out what object that got released
        var indexFrom = Array.IndexOf(weaponClones, o);
        // What slot is this object closest to?
        Vector2 pos = new Vector2(o.transform.position.x, o.transform.position.y);
        int indexTo = (int)GetClosestEdge(weaponPos, pos).z;
        // Smoothly move this object to the fixed slot. If there is already an object there, move that to the slot the object we want to move in
        MoveWeaponClones(indexFrom, indexTo);
        MoveWeaponData(indexFrom, indexTo);
    }
    // Maybe to do later, tried to make the rotation stay after a battle, but it will just be complicated with all the fixed rotation+position values
    //private void GameStateChanged(GameState s) {
    //    if(s==GameState.Shop) {
    //    Debug.Log("Storing battle rotation");
    //        prevRotation = player.transform.rotation;
    //    }
    //}


    // z value is index, inspired from https://forum.unity.com/threads/clean-est-way-to-find-nearest-object-of-many-c.44315/#post-7595566
    private Vector3 GetClosestEdge(Vector3[] positions, Vector3 myPos) {
        Vector3 bestTarget = Vector3.zero;
        float closestDistanceSqr = Mathf.Infinity;
        for (int i = 0; i < positions.Length; i++) {
            Vector2 directionToTarget = positions[i] - myPos;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr) {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = positions[i];
                bestTarget.z = i;
            }
        }
        return bestTarget;
    }
    private void WeaponEntry(byte i, WeaponType t) {
        //  Should take data from array, not modify it
        var gun = ResourceSystem.Instance.GetWeapon(t);
        weapons[i] = gun.Prefab.gameObject;
    }

    private void InstantiateWeapons() {
        for (int i = 0; i < 6; i++) {
            if (weapons[i] != null) {
                weaponClones[i] = Instantiate(weapons[i], weaponPos[i], Quaternion.Euler(0, 0, gunRotationsFlatTop[i] + player.transform.eulerAngles.z), player.transform);
                weaponClones[i].GetComponent<Weapon>().SetposPrefered(i);
            }
        }
    }
    private void MoveWeaponClones(int from, int to) {
        // Moves a gun from from to to, if to is not empty spaw from and to 
        if (weaponClones[from] == null) return;
        if (weaponClones[to] == null) {
            // Just move
            weaponClones[to] = weaponClones[from];
            weaponClones[from] = null;
            weaponClones[to].GetComponent<Weapon>().SetposPrefered(to);
        } else {
            // Swap
            var temp = weaponClones[from];
            weaponClones[from] = weaponClones[to];
            weaponClones[to] = temp;
            weaponClones[to].GetComponent<Weapon>().SetposPrefered(to);
            weaponClones[from].GetComponent<Weapon>().SetposPrefered(from);
        }
    } 
    private void MoveWeaponData(int from, int to) {
        // Moves a gun from from to to, if to is not empty spaw from and to 
        if (weapons[from] == null) return;
        if (weapons[to] == null) {
            // Just move
            weapons[to] = weapons[from];
            weapons[from] = null;
            weapons[to].GetComponent<Weapon>().SetposPrefered(to);
        } else {
            // Swap
            var temp = weapons[from];
            weapons[from] = weapons[to];
            weapons[to] = temp;
            weapons[to].GetComponent<Weapon>().SetposPrefered(to);
            weapons[from].GetComponent<Weapon>().SetposPrefered(from);
        }
    }
}