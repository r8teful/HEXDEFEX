using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : StaticInstance<CameraController> {

    private Camera thisCamera;
    private float cameraSize;

    protected override void Awake() {
        // Dont detroy object when we make new one
        Debug.Log("New Camera instance");
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        base.Awake();
        SceneManager.sceneLoaded += SceneChanged;
    }

    private void SceneChanged(Scene arg0, LoadSceneMode arg1) {
        StopAllCoroutines();
        thisCamera = FindObjectOfType<Camera>();
        if (GameManager.Instance.State.Equals(GameState.Shop)) {
            // Put camera in shop view
            
            cameraSize = 2.5f;
            StartCoroutine(MoveCamera(cameraSize,new Vector3(0, -0.75f, -10)));
        } else if (GameManager.Instance.State.Equals(GameState.Battle)) {
        StopAllCoroutines();
            // First put the camera in the position it whas when we are in the shop, this is because it gets instatiated at 0,0,-10 at the start of each scene
            thisCamera.orthographicSize = cameraSize;
            thisCamera.transform.position = new Vector3(0, -0.75f, -10);
            cameraSize = 5f;
            StartCoroutine(MoveCamera(cameraSize, new Vector3(0, 0, -10)));
        }
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= SceneChanged;
    }
    private void Start() {
        Debug.Log("Starting");
        cameraSize = thisCamera.orthographicSize;
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
