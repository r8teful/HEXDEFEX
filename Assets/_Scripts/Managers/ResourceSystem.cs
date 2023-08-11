using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Based on https://youtu.be/tE1qH8OxO2Y
// Basically just runs once at the start of the program and stores a dictionary of the different
// scriptable data which can then be easaly accessed within different scripts
public class ResourceSystem : StaticInstance<ResourceSystem> {
    public List<WeaponScriptableObject> WeaponVariant { get; private set; }
    public List<EnemyScriptableObject> EnemyVariant { get; private set; }
    public List<WaveScriptableObject> WaveVariant { get; private set; }
    private Dictionary<WeaponName, WeaponScriptableObject> _WeaponVariantDict;
    private Dictionary<EnemyName, EnemyScriptableObject> _EnemyVariantDict;
    private Dictionary<int, WaveScriptableObject> _WaveVariantDict; // int is the wave number

    protected override void Awake() {
        base.Awake();
        AssembleResources();
    }
    private void AssembleResources() {
        WeaponVariant = Resources.LoadAll<WeaponScriptableObject>("WeaponScriptables").ToList();
        _WeaponVariantDict = WeaponVariant.ToDictionary(r => r.weaponName, r => r);

        // Read all enemy scriptable data from the file
        EnemyVariant = Resources.LoadAll<EnemyScriptableObject>("EnemyScriptables").ToList();
        _EnemyVariantDict = EnemyVariant.ToDictionary(r => r.enemyName, r => r);

        // Read all wave scriptable data from the file
        WaveVariant = Resources.LoadAll<WaveScriptableObject>("WaveScriptables").ToList();
        _WaveVariantDict = WaveVariant.ToDictionary(r => r.waveNumber, r => r);

    }
    
    public WeaponScriptableObject GetWeapon(WeaponName t) => _WeaponVariantDict[t];
    public EnemyScriptableObject GetEnemy(EnemyName t) => _EnemyVariantDict[t];
    public WaveScriptableObject GetWave(int t) => _WaveVariantDict[t];

    
    // Maybe change this at one point so the randomnes can be changed depending on some luck factor
    public WeaponScriptableObject GetRandomWeapon() => WeaponVariant[Random.Range(0, WeaponVariant.Count)];



}