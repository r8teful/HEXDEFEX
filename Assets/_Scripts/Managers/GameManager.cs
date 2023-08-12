using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : StaticInstance<GameManager> {
    public delegate void GameStateChangedEventHandler(GameState state);

    public static event Action<GameState> OnGameStateChangedBefore;
    //public static event Action<GameState> OnGameStateChanged;
    public static event GameStateChangedEventHandler OnGameStateChanged;
    public GameState State { get; private set; }

    //First state is just the current state we start the scene in right now for debugging purposes TODO 
    private void Start() {
       if (SceneOperator.Instance.GetSceneIndex()==0) ChangeState(GameState.Battle);
       if (SceneOperator.Instance.GetSceneIndex()==1) ChangeState(GameState.Shop);
    }
    public void ChangeState(GameState state) {
        Debug.Log($"New state: {state}");

        State = state;
        OnGameStateChangedBefore?.Invoke(state);
        switch (state) {
            case GameState.MainMenu:
                // Start battle, settings etc
                break;
            // case GameState.StarterCoise
            case GameState.Battle: 
                // Change scene to battle
                WaveManager.Instance.StartWave();
               
                
                break;
            case GameState.Shop:
                // Change scene to shop
               // SceneOperator.Instance.ChangeScene(state);
                // Shop logic
                //WeaponManager.Instance.StartCoroutine("MoveWeapons");
                //Go to next level
                break;
            case GameState.EndMenu:
                break;
            case GameState.Loading:
                // Wait for scene to load
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
        OnGameStateChanged?.Invoke(state);
        //Debug.Log($"New state: {state}");
    }

    public void SceneLoaded(GameState loadedScene) {
        // This method is called when the scene has finished loading
        if (!State.Equals(GameState.Loading)) return;

        if (loadedScene.Equals(GameState.Battle)) {
            ChangeState(GameState.Battle); // Transition to the Battle state
        } else if (loadedScene == GameState.Shop) {
            ChangeState(GameState.Shop); // Transition to the Shop state
        }
    }
    private void Update() {
        // Toggle game state from shop to battle for debugging
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            if (State.Equals(GameState.Shop)) {
                ChangeState(GameState.Loading);
                SceneOperator.Instance.ChangeScene(GameState.Battle);
            } else {
                ChangeState(GameState.Loading);
                SceneOperator.Instance.ChangeScene(GameState.Shop);
            }
        } 
    }
}
