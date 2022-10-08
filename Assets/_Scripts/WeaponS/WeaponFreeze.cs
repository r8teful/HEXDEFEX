using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFreeze : Weapon {
    
    public override void Shoot(Quaternion orientation) {
        // Random chanse that it will be a frozen bullet, could also make it not random ??
        if(Random.Range(0,1)==0) { // Just shoot freeze bullet al the time now for debuggin 
            var rbu = Instantiate(weaponData.SpecialBullet, transform.position, orientation);
            rbu.GetComponent<Bullet>().SetData(weaponData);
            rbu.GetComponent<Bullet>().SetBulletType(BulletType.Freeze);
        } else {
            Debug.Log("PEW");
            Instantiate(bulletDefault, transform.position, transform.rotation).GetComponent<Bullet>().SetData(weaponData);
        }
    }
    // -----------------Freezer--------------------- // TODO MOVE THIS
    //private IEnumerator SpawnBulletFreezer() {
    //    while (true) {
    //        // Random chanse that it will be a frozen bullet, could also make it not random ??
    //        if(Random.Range(0,1)==0) { // Just shoot freeze bullet al the time now for debuggin 
    //            var rbu = Instantiate(weaponData.SpecialBullet, transform.position, transform.rotation);
    //            rbu.GetComponent<Bullet>().SetData(weaponData);
    //            rbu.GetComponent<Bullet>().SetBulletType(BulletType.Freeze);
    //        } else {
    //            Debug.Log("PEW");
    //            Instantiate(bullet, transform.position, transform.rotation).GetComponent<Bullet>().SetData(weaponData);
    //        }
    //        yield return new WaitForSeconds(1 / weaponData.BaseStats.fireRate);
    //
    //    }
    //}
}
