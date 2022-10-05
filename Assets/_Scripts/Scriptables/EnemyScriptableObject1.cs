using UnityEngine;

[CreateAssetMenu(fileName = "Enemy2", menuName = "ScriptableObjects/EnemyScriptableObject1", order = 3)]
public class EnemyScriptableObject1 : ScriptableObject {

    public float pullSpeed; // speed at wish the enemy will be pulled towards the centre
    public float health; // Health of the enemy
    public float damage; // Damage it will do to the ship
}
