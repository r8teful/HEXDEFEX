using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneOperator : StaticInstance<SceneOperator> {

    protected override void Awake() {
        base.Awake();
        GameManager.OnGameStateChanged += ChangeScene;
    }
    private void OnDestroy() {
        GameManager.OnGameStateChanged -= ChangeScene;
    }

    private void ChangeScene(GameState s) {
        if (s == GameState.Battle) {
            Debug.Log("Scene Change to Battle!");
            SceneManager.LoadScene(0);
        } else if (s == GameState.Shop) {
            Debug.Log("Scene Change To shop!");
            SceneManager.LoadScene(1);

        }
    }
    public int GetSceneIndex() {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
