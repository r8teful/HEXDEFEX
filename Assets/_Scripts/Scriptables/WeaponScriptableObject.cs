using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/WeaponScriptableObject", order = 2)]
public class WeaponScriptableObject : ScriptableObject {
    private Stats _stats;
    public Stats BaseStats => _stats; // We dont want to directly change the struct from code, so make a copy which we will use to change to give buffs 
    public WeaponClass weaponClass;
    public WeaponName weaponName;
    public ShootMode shootMode;
    public Weapon prefab;
    public Bullet specialBullet; // Have one extra "Special" Bullet the gun can shoot, could be an explosion, freeze, homing etc. 
    public int level;
    public string description;


    private void OnEnable() {
        SetWeaponStat();
    }


    public void SetWeaponStat() {
        // This is the switch statement that checks the weapon class
        switch (weaponClass) {
            // If the weapon class is Sword, set the weapon stat values accordingly
            case WeaponClass.Assualt:
                _stats.fireRate     = 1;
                _stats.bulletDamage = 1;
                _stats.bulletSpeed  = 1;
                _stats.bulletSize   = 1;
                _stats.accuracy     = 1;
                _stats.range        = 1;
                _stats.knockback    = 1;
                break;

            // If the weapon class is Axe, set the weapon stat values accordingly
            case WeaponClass.SMG:
                _stats.fireRate     = 1.5f;
                _stats.bulletDamage = 0.8f;
                _stats.bulletSpeed  = 1;
                _stats.bulletSize   = 0.8f;
                _stats.accuracy     = 0.8f;
                _stats.range        = 0.7f;
                _stats.knockback    = 0.6f;
                break;

            // If the weapon class is Bow, set the weapon stat values accordingly
            case WeaponClass.Shotgun:
                _stats.fireRate     = 0.8f;
                _stats.bulletDamage = 1.7f;
                _stats.bulletSpeed  = 0.9f;
                _stats.bulletSize   = 1.2f;
                _stats.accuracy     = 0.6f;
                _stats.range        = 0.5f;
                _stats.knockback    = 1f;
                break;

            // If the weapon class is Dagger, set the weapon stat values accordingly
            case WeaponClass.Marksman:
                _stats.fireRate     = 0.7f;
                _stats.bulletDamage = 1.3f;
                _stats.bulletSpeed  = 1.4f;
                _stats.bulletSize   = 1.4f;
                _stats.accuracy     = 1.5f;
                _stats.range        = 1.2f;
                _stats.knockback    = 0.8f;
                break;

            // If the weapon class is none of the above, set the weapon stat values to zero
            default:
                _stats.fireRate = 1;
                _stats.bulletDamage = 1;
                _stats.bulletSpeed = 1;
                _stats.bulletSize = 1;
                _stats.accuracy = 1;
                _stats.range = 1;
                _stats.knockback = 1;
                break;
        }
    }

}
public struct Stats {
    public int cost;
    public float fireRate;
    public float bulletDamage;
    public float bulletSpeed;
    public float bulletSize;
    public float accuracy;
    public float range;
    public float knockback;
    /*
    public Stats(int cost, float fireRate, float bulletDamage, float bulletSpeed, float bulletSize, float accurasy, float range, float knockback) {
        this.cost = cost;
        this.fireRate = fireRate;
        this.bulletDamage = bulletDamage;
        this.bulletSpeed = bulletSpeed;
        this.bulletSize = bulletSize;
        this.accurasy = accurasy;
        this.range = range;
        this.knockback = knockback;
    }
    */
}


[Serializable]
public enum WeaponClass {
    // Not final but rough idea classes 
    Assualt,
    Tactical,
    Launcher,
    SMG,
    Marksman,
    Sniper,
    Shotgun,
    Special,
    Dart,
}

[Serializable]
public enum ShootMode {
    Single,
    Burst,
    Tripple,
    Quad,
    Spread,
    Lazer
}

// We got to have a unique name so that the game knows what type of gun it is
[Serializable]
public enum WeaponName {
    Ripper,
    Boomstick,
    Schredder,
    Silencer,
    Avanger,
    Big_Bertha,
    Sweeper,
    Eagle_Eye,
    Tranquilizer,
    Lazer,
    Freezer,
    Pyro,
    Slammer,
    Big_Shot,
    Fireworks
}

