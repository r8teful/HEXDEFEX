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
    private readonly Vector2[] weaponPos = { new Vector2(0f, 0.5f), new Vector2(0.433f, 0.25f), new Vector2(0.433f, -0.25f), new Vector2(-0.0f, -0.5f), new Vector2(-0.433f, -0.25f), new Vector2(-0.433f, 0.25f) };
    private readonly float[] gunRotationsFlatTop = { (0), (-60), (-120), (-180), (-240), (-300) };
    //private float[] GunRotations =          { (-30), (-90), (-150), (-210), (-270), (-330) };


    private GameObject[] weapons;
    private GameObject[] weaponClones;
    
    [SerializeField] private GameObject originalPlayer;
    private GameObject player;
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
        // Scene changed
        if (player != null) return;
        player = Instantiate(originalPlayer, Vector3.zero, Quaternion.identity);
        PlaceWeapon(0);
        PlaceWeapon(1);
        PlaceWeapon(2);
        //Debug.Log("instantiating player");
        //player = Instantiate(originalPlayer, Vector3.zero, Quaternion.identity);
    }
    private void WeaponRelease(GameObject o) {
        // Find out what object that got released
        Debug.Log($"Index of object {o} is found at: {Array.IndexOf(weaponClones, o)} ");
        var index = Array.IndexOf(weaponClones, o);
        Vector2 pos = new Vector2(o.transform.position.x, o.transform.position.y);
        // What slot is this object closest to?
        int closestIndex = (int)GetClosestEdge(weaponPos, pos).z;
        Debug.Log("Please fuck for the love of god please be the clost one "+ closestIndex);
        // Smoothly move this object to the fixed slot.
        StartCoroutine(MoveWeapon(index,weaponPos[closestIndex]));
        //weaponClones[index].transform.position = Vector3.Slerp(weaponClones[index].transform.position,weaponPos[closestIndex], 10f * Time.deltaTime);
        //weaponClones[index].transform.position = Vector2.MoveTowards(weaponClones[index].transform.position,weaponPos[closestIndex], 10f * Time.deltaTime);
       // weaponClones[index].transform.position =Vector2.zero;
    }

    // z value is index, inspired from https://forum.unity.com/threads/clean-est-way-to-find-nearest-object-of-many-c.44315/#post-7595566
    Vector3 GetClosestEdge(Vector2[] positions, Vector2 myPos) {
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

    private void Start() {
        // Populate placeholder weapon data
        player = Instantiate(originalPlayer, Vector3.zero, Quaternion.identity);
    }

    private void WeaponEntry(byte i, WeaponType t) {
        //  Should take data from array, not modify it
        var gun = ResourceSystem.Instance.GetWeapon(t);
        weapons[i] = gun.Prefab.gameObject;
    }

    private void PlaceWeapon(byte i) {
        //  Should take data from array, not modify it
       weaponClones[i] = Instantiate(weapons[i], weaponPos[i], Quaternion.Euler(0, 0, gunRotationsFlatTop[i]), player.transform);
    }


    private void MoveWeapon(byte from, byte to) {
        // Moves a gun from from to to, if to is not empty spaw from and to 
        if (weapons[from] == null) return;
        if (weapons[to] == null) {
            // Just move
            weapons[to] = weapons[from];
            weapons[from] = null;
            // Change position & rotation in space
            weapons[to].transform.SetPositionAndRotation(weaponPos[to], Quaternion.Euler(0, 0, gunRotationsFlatTop[to]));
        } else {
            // Swap
            var temp = weapons[from];
            weapons[from] = weapons[to];
            weapons[to] = temp;
            // Change position & rotation in space
            weapons[from].transform.SetPositionAndRotation(weaponPos[to], Quaternion.Euler(0, 0, gunRotationsFlatTop[to]));
            weapons[to].transform.SetPositionAndRotation(weaponPos[from], Quaternion.Euler(0, 0, gunRotationsFlatTop[from]));
        }
    }

    private IEnumerator MoveWeapon(int weaponIndex,Vector3 dest) {
        // TODO Pretty bad way of doing this. What if you grab the weapon just before it arives at the destination?The while loop will not break and it will be two running at the same time 
        while ((weaponClones[weaponIndex].transform.position - dest).magnitude > 0.01f) {
            weaponClones[weaponIndex].transform.position = Vector3.Slerp(weaponClones[weaponIndex].transform.position, dest, 10f * Time.deltaTime);
            weaponClones[weaponIndex].transform.up = Vector3.Slerp(weaponClones[weaponIndex].transform.up, (-weaponClones[weaponIndex].transform.position), 40f * Time.deltaTime);
            yield return null;
        }
        yield break;
    }
}