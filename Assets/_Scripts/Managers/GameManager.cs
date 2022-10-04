using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : StaticInstance<GameManager> {
    public static event Action<GameState> OnGameStateChangedBefore;
    public static event Action<GameState> OnGameStateChanged;
    public GameState State { get; private set; }

    //First state is just the current state we start the scene in right now
    private void Start() {
        if (SceneOperator.Instance.GetSceneIndex()==0) ChangeState(GameState.Battle);
        if (SceneOperator.Instance.GetSceneIndex()==1) ChangeState(GameState.Shop);
    }
    public void ChangeState(GameState state) {

        State = state;
      
        OnGameStateChangedBefore?.Invoke(state);
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
                //WeaponManager.Instance.StartCoroutine("MoveWeapons");
                //Go to next level
                break;
            case GameState.EndMenu:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
        OnGameStateChanged?.Invoke(state);
        //Debug.Log($"New state: {state}");
    }
    private void Update() {
        // Toggle game state from shop to battle for debugging
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            if (State.Equals(GameState.Shop)) {
                ChangeState(GameState.Battle);
            } else {
                ChangeState(GameState.Shop);

            }
        } 
    }
}
