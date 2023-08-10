using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script is used to manage the weapons at the 6 different spots on the ship
// Also manages and places down the ship
public class WeaponManager : StaticInstance<WeaponManager> {

    // The different rotation and position the gun has to have depending on the slot
    public readonly Vector3[] weaponPos = { new Vector2(0f, 0.5f), new Vector2(0.433f, 0.25f), new Vector2(0.433f, -0.25f), new Vector2(-0.0f, -0.5f), new Vector2(-0.433f, -0.25f), new Vector2(-0.433f, 0.25f) };
    private readonly float[] gunRotations = { (0), (-60), (-120), (-180), (-240), (-300) };

    [SerializeField] private WeaponDataScriptableObject weaponDataHolder;
    private WeaponScriptableObject[] weapons;
    private GameObject[] weaponClones;

    [SerializeField] private GameObject originalPlayer;
    private GameObject player;

    protected override void Awake() {
        base.Awake();
        Debug.Log("awake is called on weaponmanager!");
        weapons = weaponDataHolder.weaponsData;
        weaponClones = new GameObject[6];
        SceneManager.sceneLoaded += SceneDoneLoading;
        InputManager.OnWeaponRelease += WeaponRelease;
        InputManager.OnWeaponBuy += WeaponBuy;
    }
    private void OnDestroy() {
        SceneManager.sceneLoaded -= SceneDoneLoading;
        InputManager.OnWeaponRelease -= WeaponRelease;
        InputManager.OnWeaponBuy -= WeaponBuy;
    }

    private void Start() {
        // Empy all data from the start of the game
        for (int i = 0; i < 6; i++) {
            weapons[i] = null;
        }
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
        // MoveWeaponClones(indexFrom, indexTo);
        MoveWeaponData(indexFrom, indexTo);
    }

    private void WeaponBuy(GameObject o) {
        // YEY! You bought a weapon, how great. Let's see if there is space for it first
        var emptySlots = 0;
        for (int i = 0; i < weapons.Length; i++) {
            if (weapons[i] == null) {
                emptySlots++;
            }
        }
        if (emptySlots == 0) {
            // TODO ABORT
            Debug.Log("ABORT. No space for weapon");
        }


        // Okey there is a slot, phew! Now where did we release the weapon so we know where to put it on the ship
        Vector2 pos = new Vector2(o.transform.position.x, o.transform.position.y);
        int indexTo = (int)GetClosestEdge(weaponPos, pos).z;
        var wd = IUManagerScreen.Instance.GetShopWeapon(o.GetComponent<Weapon>().GetPosPrefered()); // We get weapon data from the IUManager
        
        // Check if there is already a weapon in the slot we want to buy a weapon to
        if (weapons[indexTo] != null) {
            // There is a weapon in the slot, we need to move it to an empty slot
            var indexFrom = Array.IndexOf(weapons, weapons[indexTo]);
            // Find an empty slot
            for (int i = 0; i < weapons.Length; i++) {
                if (weapons[i] == null) {
                    MoveWeaponData(indexFrom, i);
                    break;
                }
            }
            // Add the data
          //  weapons[indexTo] = wd;
            //MoveWeaponData(indexFrom, 6); // 6 is the empty slot
       // } else {
            // Move it to the correct slot and add data
            
        }
        weapons[indexTo] = wd; // We send it to the weapon[] array so that we can store the data in the right slot
        weaponClones[indexTo] = o; // To keep track of the gameobjects that are spawned we store them in an array
                                   // Call methods on the actual gameobject
        o.GetComponent<Weapon>().SetposPrefered(indexTo);
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
    private void WeaponEntryDirect(byte i, WeaponName t) {
        //  Should take data from array, not modify it
        weapons[i] = ResourceSystem.Instance.GetWeapon(t);
        // weapons[i] = gun.Prefab.gameObject; // Reference to prefab 
    }

    private void InstantiateWeapons() {
        for (int i = 0; i < 6; i++) {
            if (weapons[i] != null) {
                weaponClones[i] = Instantiate(weapons[i].Prefab.gameObject, weaponPos[i], Quaternion.Euler(0, 0, gunRotations[i] + player.transform.eulerAngles.z), player.transform);
                weaponClones[i].GetComponent<Weapon>().SetposPrefered(i);
            }
        }
    }

    private void MoveWeaponData(int from, int to) {
        // Moves a gun from from to to, if to is not empty spaw from and to 
        MoveWeaponClones(from, to);
        if (weapons[from] == null) {
            Debug.Log($"returning: {from} is null");
            return;
        }
        if (weapons[to] == null) {
            // Just move
            weapons[to] = weapons[from];
            weapons[from] = null;
            Debug.Log($"slot: {to} is null, moving {from} to this slot");
        } else {
            // Swap
            var temp = weapons[from];
            weapons[from] = weapons[to];
            weapons[to] = temp;
            Debug.Log($"Both slot {to} and slot {from} are full, swapping");
        }
    }
    private void MoveWeaponClones(int from, int to) {
        // Moves a gun from from to to, if to is not empty spaw from and to 
        if (weaponClones[from] == null) {
            return;
        }
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
}