using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inspiration from https://www.youtube.com/watch?v=7T-MTo8Uaio
public class WaveManager : StaticInstance<WaveManager> {
    // Manages the transition and keeps track of wave number, spawns the wave prefabs
    private int waveNumber = 1; 

    public void StartWave() {
        DebugManager.Instance.UpdateWave(waveNumber); // TODO Temp
        var wave = ResourceSystem.Instance.GetWave(waveNumber);
        var t = Instantiate(wave.prefab.gameObject, Vector3.zero, Quaternion.identity);
        t.GetComponent<Wave>().StartWave();
        waveNumber++;
    }
}
