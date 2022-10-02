using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is used to place a gun type at one of the 6 different spots on the ship
public class WeaponPlacer : MonoBehaviour {

    // The different rotation and position the gun has to have depending on the slot
   // private Vector2[] GunPositions = {new Vector2(0.3f,0.5f), new Vector2(0.6f,0), new Vector2(0.3f,-0.5f), 
                                     //new Vector2(-0.3f,-0.5f), new Vector2(-0.6f,0), new Vector2(-0.3f,0.5f)};
    private Vector2[] weaponPos = {new Vector2(0f,0.5f), new Vector2(0.433f,0.25f), new Vector2(0.433f,-0.25f),
                                   new Vector2(-0.0f,-0.5f), new Vector2(-0.433f,-0.25f), new Vector2(-0.433f,0.25f)};

    private GameObject[] weapons;
    //private float[] GunRotations =          { (-30), (-90), (-150), (-210), (-270), (-330) };
    private float[] GunRotationsFlatTop = { (0)  , (-60), (-120), (-180), (-240), (-300) };

    private void Start() {
        weapons = new GameObject[6];
        PlaceWeapon(0, WeaponType.Single);
        PlaceWeapon(2, WeaponType.Burst);
        PlaceWeapon(1, WeaponType.Multi);
    }

    private void PlaceWeapon(byte i, WeaponType t) {
        var gun = ResourceSystem.Instance.GetWeapon(t);
        var w = Instantiate(gun.Prefab, weaponPos[i], Quaternion.Euler(0, 0, GunRotationsFlatTop[i]), transform);
        weapons[i] = w.gameObject;
    }

    private void MoveWeapon(byte from, byte to) {
        // Moves a gun from from to to, if to is not empty spaw from and to 
        if (weapons[from] == null) return;
        if (weapons[to] == null) {
            // Just move
            weapons[to] = weapons[from];
            weapons[from] = null;
            // Change position & rotation in space
            weapons[to].transform.SetPositionAndRotation(weaponPos[to], Quaternion.Euler(0, 0, GunRotationsFlatTop[to]));
        } else {
            // Swap
            var temp = weapons[from];
            weapons[from] = weapons[to];
            weapons[to] = temp;
            // Change position & rotation in space
            weapons[from].transform.SetPositionAndRotation(weaponPos[to], Quaternion.Euler(0, 0, GunRotationsFlatTop[to]));
            weapons[to].transform.SetPositionAndRotation(weaponPos[from], Quaternion.Euler(0, 0, GunRotationsFlatTop[from]));
        }
    }
}