using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/EnemyScriptableObject", order = 1)]
public class EnemyScriptableObject : ScriptableObject {

    public float pullSpeed; // speed at wish the enemy will be pulled towards the centre
    public float maxSpeed;
    public float health; // Health of the enemy
    public float damage; // Damage it will do to the ship
    public int cost;  
    public EnemyName enemyName;
    public Enemy prefab; 
}


// We got to have a unique name so that the game knows what type of enemy it is
[Serializable]
public enum EnemyName {
    Gray, // Easiest value 1
    Blue, // value 2
    Brown,
    Green,
    Cyan,
    Red,
    Purple,
    Black,
    Silver,
    Gold, // Hardest
}
