using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKnock : Weapon {
    public override void Shoot(Quaternion orientation) {
        // Random chanse that it will be a frozen bullet, could also make it not random ??
        if (Random.Range(0, 6) == 0)
        { // Just shoot freeze bullet al the time now for debuggin 
            var rbu = Instantiate(weaponData.specialBullet, transform.position, orientation);
            rbu.GetComponent<Bullet>().SetData(weaponData);
            rbu.GetComponent<Bullet>().SetBulletType(BulletType.Knockback);
        }
        else
        {
            Instantiate(bulletDefault, transform.position, transform.rotation).GetComponent<Bullet>().SetData(weaponData);
        }
    }
}
