using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Based on https://youtu.be/tE1qH8OxO2Y
// Basically just runs once at the start of the program and stores a dictionary of the different scriptable data which can then be easaly accessed within different scripts
public class ResourceSystem : MonoBehaviour {

    public List<WeaponScriptableObject> WeaponVariant { get; private set; }
    private Dictionary<WeaponType, WeaponScriptableObject> _WeaponVariantDict;
    public static ResourceSystem Instance;

    private void Awake() {
        Instance = this;
        AssembleResources();
    }
    private void OnApplicationQuit() {
        Instance = null;
        Destroy(gameObject);
    }

    private void AssembleResources() {
        WeaponVariant = Resources.LoadAll<WeaponScriptableObject>("WeaponScriptables").ToList();
        _WeaponVariantDict = WeaponVariant.ToDictionary(r => r.WeaponType, r => r);
    }

    public WeaponScriptableObject GetWeapon(WeaponType t) => _WeaponVariantDict[t];
}
