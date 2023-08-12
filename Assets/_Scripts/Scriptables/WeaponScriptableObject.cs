using JetBrains.Annotations;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/WeaponScriptableObject", order = 2)]
public class WeaponScriptableObject : ScriptableObject {
    [SerializeField] private Stats _stats;
    public Stats BaseStats => _stats; // We dont want to directly change the struct from code, so make a coppy which we will use to change to give buffs 
    public WeaponClass weaponClass;
    public WeaponName weaponName;
    public ShootMode shootMode;
    public Weapon prefab;
    public Bullet specialBullet; // Have one extra "Special" Bullet the gun can shoot, could be an explosion, freeze, homing etc. 
}

/* 
* Fire Rate
* Bullet Damage
* Bullet Speed
* Bullet Size
* Defense Penetration
* Critical rate
* Critical effect?
* Area Of Effect Size
* Area Of Effect Damage
* Accuracy? Some guns might not shoot in a straight line
* Range? How far the bullet goes
* Knockback: The knockback effect the bullet has on impact on an enemy
* sbyte signed 8bit
* byte unsigned 8bit
*/
[Serializable]
public struct Stats {
    public int cost;
    public float fireRate;
    public float bulletDamage;
    public float bulletSpeed;
    public float bulletSize;
    public float defensePen;
    public float critRate;
    public float critDamage;
    public float aoeSize;
    public float aoeDamage;
    public float accurasy;
    public float range;
    public float knockback;
}
/*
* Assault:
* Tactical: +1 projectile or Crit?
* Launcher: 20% AOE
* SMG: 15% Fire rate
* LMG: 20% knockback distance
* Pistol: 20%  DoT
* Marksman: 20% less defense
* Sniper: 25% Bullet size, More dmg the further the bullet travels? More speed for the bullet
* Shotgun: 15% Bullet DMG
* Special
* Dart
* Healer
* Heavy
*/
[Serializable]
public enum WeaponClass {
    // Not final but rough idea classes 
    Assualt,
    Tactical,
    Launcher,
    SMG,
    LMG,
    Pistol,
    Marksman,
    Sniper,
    Shotgun,
    Special,
    Dart,
    Healer,
    Heavy
}

[Serializable]
public enum ShootMode {
    Single,
    Burst,
    Tripple,
    Quad,
    Lazer
}

// We got to have a unique name so that the game knows what type of gun it is
[Serializable]
public enum WeaponName {
    Single,
    Burst,
    Multi,
    Freezer,
    Knockback
}