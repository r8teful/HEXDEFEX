using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is used to place a gun type at one of the 6 different spots on the ship
public class GunPlacer : MonoBehaviour {
    public GameObject[] GunSlots = new GameObject[6];
    [SerializeField] GameObject Gun; 
    public GameObject Gun1;
    // The different rotation and position the gun has to have depending on the slot
    private Vector2[] GunPositions = {new Vector2(0.3f,0.5f), new Vector2(0.6f,0), new Vector2(0.3f,-0.5f), 
                                     new Vector2(-0.3f,-0.5f), new Vector2(-0.6f,0), new Vector2(-0.3f,0.5f)};
    private float[] GunRotations = {(-30), (-90), (210), (150), (90), (30) };

    private void Start() {
        for (int i = 0; i < 6; i++) {
            // Loop through the list of guns and place them in the right slots
            if (GunSlots[i]!=null) {
                GunSlots[i].gameObject.transform.position = GunPositions[i];
                GunSlots[i].gameObject.transform.Rotate(new Vector3(0, 0, GunRotations[i]), Space.Self); 
            }
        }
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            // Instantate a Gun object on 1st slot. Player has to be in default orientation 
            Gun1 = Instantiate(Gun,GunPositions[0],Quaternion.Euler(0,0,GunRotations[0]),transform);
            Gun1.GetComponent<Gun>().guntype = GunType.Multi;
        }
    }
}
