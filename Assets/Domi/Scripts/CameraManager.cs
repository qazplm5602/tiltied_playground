using Unity.Cinemachine;
using UnityEngine;

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

public class CameraManager : MonoBehaviour
{
    [SerializeField] CameraData[] cameraList;
    [field: SerializeField] public CameraTransition Transition { get; private set; }

    private void Awake() {
        
    }
}
