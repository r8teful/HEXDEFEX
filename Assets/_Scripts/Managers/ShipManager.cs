using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.CullingGroup;

public class ShipManager : StaticInstance<ShipManager> {

    [SerializeField] private GameObject originalPlayer;
    private GameObject player;
    private float shipHealth= 100f;
    private float shipMaxHealth =100f;
    private float shipShield;
    private float shipMaxShield;
    private float shipLVL;
    private float shipExp;
    private float shipExpToNextLVL;
    private int currency = 2000; 

    public async Task SpawnPlayer() {
        player = Instantiate(originalPlayer, Vector3.zero, Quaternion.identity);
        await Task.CompletedTask;
    }

    public GameObject GetPlayer() {
        return player;
    }

    public void EnemyHit(float damage) {
        if (shipShield > 0) {
            shipShield -= damage;
        } else if (shipHealth > 0) {
            shipHealth -= damage;
        } else {
            // TODO game over
            Debug.Log("Game Over!");
        }
    }
    public void AddHealth(float health) {
        shipHealth += health;
        if (shipHealth > shipMaxHealth) {
            shipHealth = shipMaxHealth;
        }
    }

    public void levelTracker() { 
        // check how much xp we need to reach the next level

    }

    public void AddCurrency(int currency) {
        this.currency += currency;
        shipExp += currency;
    }
    public void RemoveCurrency(int currency) {
        this.currency -= currency;
    }

    internal float GetShipHealth() { 
        return shipHealth;
    }

    internal float GetCurrency() {
        return currency;
    }

    internal float GetShipXP() {
        return shipExp;
    }

    internal float GetShipLVL() {
        return shipLVL;
    }
}
