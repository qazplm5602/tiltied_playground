using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CameraType {
    Main,
    Blue_R,
    Blue_L,
    Orange_R,
    Orange_L,
    Result_Player,
    Custom_1, // 이건 그냥 자유롭게 쓰는거
    Custon_2
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

    public enum NearType { Near, Far }

    // 그니까 카메라 타입들을 넣으면 가까운거 순으로 함 (아님 말고)
    public List<CameraType> GetNearCam(NearType type, CameraType[] types, Vector3 pos) {
        List<CameraType> list = types.ToList();
        list.Sort((a, b) => {
            float dist1 = Vector3.Distance(cameraList[a].transform.position, pos);
            float dist2 = Vector3.Distance(cameraList[b].transform.position, pos);
        
            if (dist1 > dist2) {
                return type == NearType.Near ? -1 : 1;
            } else if (dist1 < dist2) {
                return type == NearType.Near ? 1 : -1;
            } else return 0;
        });

        return list;
    }
}
