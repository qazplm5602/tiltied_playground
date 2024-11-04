using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CameraType {
    Main,
    Blue_R,
    Blue_L,
    Orange_R,
    Orange_L
}

[System.Serializable]
public struct CameraData {
    CameraType type;
    CinemachineCamera cam;
}

public class CameraManager : MonoSingleton<CameraManager>
{
    private Dictionary<CameraType, Camera> cameraList;
    [field: SerializeField] public CameraTransition Transition { get; private set; }

    protected override void Awake() {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode) {
        cameraList = new(); // 다시 파
        
        Camera[] cameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);
        print($"find camera length: {cameras.Length}");
        
        foreach (var item in cameras)
        {
            string entityName = item.gameObject.name;
            try {
                CameraType type = (CameraType)Enum.Parse(typeof(CameraType), entityName);
                cameraList[type] = item;
            } catch (ArgumentException) {
                // 그냥 다른 카메라 인가봄
            }
            
        }

        print($"camera insert count: {cameraList.Count}");
    }

    public Camera GetCamera(CameraType type) {
        if (cameraList.TryGetValue(type, out var cam))
            return cam;

        return null;
    }
}