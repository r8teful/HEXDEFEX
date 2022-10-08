using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Based on https://youtu.be/tE1qH8OxO2Y
// Basically just runs once at the start of the program and stores a dictionary of the different scriptable data which can then be easaly accessed within different scripts
public class ResourceSystem : StaticInstance<ResourceSystem> {
    public List<WeaponScriptableObject> WeaponVariant { get; private set; }
    private Dictionary<WeaponName, WeaponScriptableObject> _WeaponVariantDict;

    protected override void Awake() {
        base.Awake();
        AssembleResources();
    }
    private void AssembleResources() {
        WeaponVariant = Resources.LoadAll<WeaponScriptableObject>("WeaponScriptables").ToList();
        _WeaponVariantDict = WeaponVariant.ToDictionary(r => r.WeaponName, r => r);
    }

    public WeaponScriptableObject GetWeapon(WeaponName t) => _WeaponVariantDict[t];
    
    // Maybe change this at one point so the randomnes can be changed depending on some luck factor
    public WeaponScriptableObject GetRandomWeapon() => WeaponVariant[Random.Range(0, WeaponVariant.Count)]; 
}
