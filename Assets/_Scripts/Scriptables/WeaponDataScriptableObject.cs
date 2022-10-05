using UnityEngine;

[CreateAssetMenu]
public class WeaponDataScriptableObject : ScriptableObject {
    public WeaponScriptableObject[] weaponsData = new WeaponScriptableObject[6];
}
