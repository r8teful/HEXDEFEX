using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/EnemyScriptableObject", order = 1)]
public class EnemyScriptableObject : ScriptableObject {

    public float pullSpeed; // speed at wish the enemy will be pulled towards the centre
    public float health; // Health of the enemy
    public float damage; // Damage it will do to the ship
}
