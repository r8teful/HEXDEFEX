using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    public GameState State { get; private set; }

    //First state is just the current state we start the scene in right now
    private void Start() {
        ChangeState(GameState.Battle);
    }
    private void Awake() {
        Instance = this;
    }
    private void OnApplicationQuit() {
        Instance = null;
        Destroy(gameObject);
    }

    private void ChangeState(GameState state) {
        
        State = state;
        switch (state) {
            case GameState.MainMenu:
                // Start battle, settings etc
                break;
            case GameState.Battle:
                // Do battle stuff

                //
                break;
            case GameState.Shop:
                // Shop logic

                //Go to next level
                break;
            case GameState.EndMenu:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        Debug.Log($"New state: {state}");
    
    }
}
