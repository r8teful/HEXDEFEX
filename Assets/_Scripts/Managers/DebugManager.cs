using System;
using TMPro;
using UnityEngine;

public class DebugManager : StaticInstance<DebugManager> {

    [SerializeField] private TMP_Text shipHealth;
    [SerializeField] private TMP_Text currency;
    [SerializeField] private TMP_Text shipXP;
    [SerializeField] private TMP_Text shipLVL;
    [SerializeField] private TMP_Text wave;

    internal void UpdateWave(int waveNumber) {
        wave.text= "Wave:" + waveNumber.ToString();
    }

    private void Update() {
        shipHealth.text = "Ship Health: " + ShipManager.Instance.GetShipHealth().ToString();
        currency.text = "Currency: " + ShipManager.Instance.GetCurrency().ToString();
        shipXP.text = "Ship XP: " + ShipManager.Instance.GetShipXP().ToString();
        shipLVL.text = "Ship LVL: " + ShipManager.Instance.GetShipLVL().ToString();
    }
}