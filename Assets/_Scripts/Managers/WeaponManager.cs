using System;
using System.Threading.Tasks;
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


    protected override void Awake() {
        base.Awake();
       // Debug.Log("awake is called on weaponmanager!");
        weapons = weaponDataHolder.weaponsData; // TODO I think this will not actually work ( scripteblobjects don't save well in build)
        weaponClones = new GameObject[6];
        //SceneManager.sceneLoaded += SceneDoneLoading;
        GameManager.OnGameStateChanged += StateChanged;
        InputManager.OnWeaponRelease += WeaponRelease;
      
    }
    private void OnDestroy() {
        //SceneManager.sceneLoaded -= SceneDoneLoading;
        GameManager.OnGameStateChanged -= StateChanged;
        InputManager.OnWeaponRelease -= WeaponRelease;
   
    }

    private void Start() {
        // Empy all data from the start of the game
        for (int i = 0; i < 6; i++) {
            weapons[i] = null;
        }
    }

    private async void StateChanged(GameState t) {
        // Wait for Shipmanager to spawn the ship before we spawn the weapons
        await ShipManager.Instance.SpawnPlayer();
        
        InstantiateWeapons();
    }
    private void WeaponRelease(GameObject o) {
        // Find out what object that got released. This assumes that the weapon is already added to the ship. And not from the shop
        var indexFrom = Array.IndexOf(weaponClones, o);
        // What slot is this object closest to?
        Vector2 pos = new Vector2(o.transform.position.x, o.transform.position.y);
        int indexTo = (int)GetClosestEdge(weaponPos, pos).z;
        // Check if the weapon on indexTo is of the same name, if so, upgrade, if not, move
        if (weapons[indexTo] != null && weapons[indexTo].weaponName == weapons[indexFrom].weaponName) {
            WeaponUpgrade(weapons[indexTo], weapons[indexFrom]);
        } else {
            // Smoothly move this object to the fixed slot. If there is already an object there, move that to the slot the object we want to move in
            MoveWeaponData(indexFrom, indexTo);
        }


    }

    private void WeaponUpgrade(WeaponScriptableObject one, WeaponScriptableObject two) {
        // Destroy one weapon convert one weapon to lvl 2 version
        

    }

    public void WeaponBuy(GameObject o) {
        // We assume we have enough money and there is space for the weapon when we call this funciton
        // Where did we release the weapon so we know where to put it on the ship
        Vector2 pos = new Vector2(o.transform.position.x, o.transform.position.y);
        int indexTo = (int)GetClosestEdge(weaponPos, pos).z;
        var wd = IUManagerScreen.Instance.GetShopWeapon(o.GetComponent<Weapon>().GetPosPrefered()); // We get weapon data from the IUManager
        
        // Check if there is already a weapon in the slot we want to buy a weapon to
        if (weapons[indexTo] != null) {
            // There is a weapon in the slot, we need to move it to an empty slot, or upgrade it if it is the same weapon name
            var indexFrom = Array.IndexOf(weapons, weapons[indexTo]);
            // Check for upgrade
            if (weapons[indexTo].weaponName == wd.weaponName) {
               // WeaponUpgrade(); TODO tomorrow 18th aug
                return; 
            }
            // Find an empty slot
            for (int i = 0; i < weapons.Length; i++) {
                if (weapons[i] == null) {
                    MoveWeaponData(indexFrom, i);
                    break;
                }
            }
        }
        weapons[indexTo] = wd; // We send it to the weapon[] array so that we can store the data in the right slot
        weaponClones[indexTo] = o; // To keep track of the gameobjects that are spawned we store them in an array
                                   // Call methods on the actual gameobject
        o.GetComponent<Weapon>().SetPosPrefered(indexTo);
        UpdateNeighbors();
    }

    public void WeaponSell(GameObject o) {
        // Delete data
        var p = o.GetComponent<Weapon>().GetPosPrefered();
        weapons[p] = null;
        weaponClones[p] = null;
        // Delete gameobject
        Destroy(o);
        UpdateNeighbors();
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
        var p = ShipManager.Instance.GetPlayer();
        for (int i = 0; i < 6; i++) {
            if (weapons[i] != null) {
                weaponClones[i] = Instantiate(weapons[i].prefab.gameObject, weaponPos[i], Quaternion.Euler(0, 0, gunRotations[i] + p.transform.eulerAngles.z), p.transform);
                weaponClones[i].GetComponent<Weapon>().SetPosPrefered(i);
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
        // Notify the weapons that someone has moved
        UpdateNeighbors();
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
            var w = weaponClones[to].GetComponent<Weapon>();
            w.SetPosPrefered(to);
            w.ApplyBuffsFromNeighbors();

        } else {
            // Swap
            var temp = weaponClones[from];
            weaponClones[from] = weaponClones[to];
            weaponClones[to] = temp;
            // Position now changed. Let weapons know
            weaponClones[to].GetComponent<Weapon>().SetPosPrefered(to);
            weaponClones[from].GetComponent<Weapon>().SetPosPrefered(from);
        }
    }

    private void UpdateNeighbors() {
        for (int i = 0; i < weaponClones.Length; i++) {
            if (weaponClones[i] != null) {
                weaponClones[i].GetComponent<Weapon>().ApplyBuffsFromNeighbors();
            }
        }
    }

    public bool SpaceForWeapon() {
        // Let's see if there is space for it first
        var emptySlots = 0;
        for (int i = 0; i < weapons.Length; i++) {
            if (weapons[i] == null) {
                emptySlots++;
            }
        }
        if (emptySlots == 0) {
            Debug.Log("ABORT. No space for weapon");
            return false;
        } else {
            return true;
        }
    }

    // nullable struct is a thing
    public WeaponClass? GetWeaponClassByPosPrefered(int pos) {
        if (weaponClones[pos] == null) return null;
        return weapons[pos].weaponClass;
    }
}