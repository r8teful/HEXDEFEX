using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneOperator : StaticInstance<SceneOperator> {

    //  protected override void Awake() {
    //      base.Awake();
    //      GameManager.OnGameStateChanged += ChangeScene;
    //  }
    //  private void OnDestroy() {
    //      GameManager.OnGameStateChanged -= ChangeScene;
    //  }


    public void ChangeScene(GameState s) {
        if (s == GameState.Battle) {
            Debug.Log("Scene Change to Battle!");
            StartCoroutine(LoadScene(s));
        } else if (s == GameState.Shop) {
            Debug.Log("Scene Change To shop!");
            StartCoroutine(LoadScene(s));
        }
    }

    private IEnumerator LoadScene(GameState gameState) {
        
        SceneManager.LoadScene(gameState.ToString());
        yield return new WaitUntil(() => SceneManager.GetSceneByName(gameState.ToString()).isLoaded);

        // Scene is fully loaded
        //GameManager.Instance.State
        GameManager.Instance.ChangeState(gameState);
    }


    public int GetSceneIndex() {
        return SceneManager.GetActiveScene().buildIndex;
    }
}