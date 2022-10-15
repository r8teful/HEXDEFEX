using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : StaticInstance<CameraController> {

    private Camera thisCamera;
    // These two variable store the previous position of the camera so we can set it when a scene is done loading
    private float cameraSize;
    private Vector3 cameraPosition;
    private readonly float sizeBattle = 3f * Screen.height / Screen.width;
    private readonly float sizeShop = 1.5f * Screen.height / Screen.width;

    protected override void Awake() {
        // Dont detroy object when we make new one
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        base.Awake();
        thisCamera = FindObjectOfType<Camera>();
        // Do this just for now TODO remove this and put it as first scene
        if (GameManager.Instance.State.Equals(GameState.Shop)) {
            thisCamera.orthographicSize = sizeShop;
        } else {
            thisCamera.orthographicSize = sizeBattle;
        }
        cameraSize = thisCamera.orthographicSize;
        cameraPosition = thisCamera.transform.position;
        Debug.Log($"CameraSize is equal to: {cameraSize} and cameraPos is {cameraPosition}");
        SceneManager.sceneLoaded += SceneChanged;
    }

    private void SceneChanged(Scene arg0, LoadSceneMode arg1) {
        StopAllCoroutines();
        thisCamera = FindObjectOfType<Camera>();
        thisCamera.transform.position = cameraPosition;
        thisCamera.orthographicSize = cameraSize;
        if (GameManager.Instance.State.Equals(GameState.Shop)) {
            // Put camera in shop view
            cameraSize = sizeShop;
            cameraPosition = new Vector3(0, -0.75f, -10);
            StartCoroutine(MoveCamera(sizeShop, new Vector3(0, -0.75f, -10)));
        } else if (GameManager.Instance.State.Equals(GameState.Battle)) {
            // First put the camera in the position it whas when we are in the shop, this is because it gets instatiated at 0,0,-10 at the start of each scene
            cameraSize = sizeBattle;
            cameraPosition = new Vector3(0, 0, -10);
            //cameraSize = 5f;
            StartCoroutine(MoveCamera(sizeBattle, new Vector3(0, 0, -10)));
        }
    
    }
    private void OnDestroy() {
        SceneManager.sceneLoaded -= SceneChanged;
    }

    private IEnumerator MoveCamera(float size, Vector3 pos) {
        while ((thisCamera.orthographicSize != size) &&  (thisCamera.transform.position != pos)) {
            thisCamera.orthographicSize = Mathf.Lerp(thisCamera.orthographicSize, size, Time.deltaTime * 5);
            thisCamera.transform.position = Vector3.Lerp(thisCamera.transform.position, pos, Time.deltaTime * 5);
            yield return null;
        }
        yield break;
    }
}
