using System;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/WaveScriptableObject", order = 3)]

public class WaveScriptableObject : ScriptableObject {
   
    public int waveNumber;
    public int waveStrength; //says how much currency we can spend on enemies
    public float waveDuration; // how long the wave lasts
    //public List<Pattern> posiblePatterns; // list of patterns that will be used in the wave
    public List<Enemy> posibleEnemies;
    public Wave prefab; 
    public Pattern pattern;
}

[Serializable]
public enum Pattern {
    Point,         // No pattern, randomly around the ship 
    Cluster,      // Clusters and then scatter
    Line,
    Arc,          
    Sync,         // Synchronized patterns
    Spiral,       // Spiral patterns that gradually close in on the player
}