using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraTransition : MonoBehaviour
{
    [SerializeField] private int basePriority; // 기본 순위
    [SerializeField] private int topPriority; // 보일떄 순위
    [SerializeField] private string screenName = "CamScreen";

    private RenderTexture renderTexture = null;
    private RawImage screen;
    private CameraType currentCamType = CameraType.Main;
    
    private void Awake() {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode) {
        screen = FindObjectsByType<RawImage>(FindObjectsInactive.Include, FindObjectsSortMode.None).First(v => v.gameObject.name == screenName);

        if (screen == null)
            Debug.LogWarning("Fade In/Out을 할 RawImage가 없습니다.");
    }

    public void FadeChangeCam(CameraType cam) {
        // Screen.width
        if (renderTexture == null) {
            CreateRenderTexture();
        }

        // 활성화
        screen.enabled = true;
        screen.color = Color.white;

        CinemachineCamera oldCam = CameraManager.Instance.GetCamera(currentCamType);
        // oldCam.
    }

    private void CreateRenderTexture() {
        renderTexture = new RenderTexture(Screen.width, Screen.height, 16);
        screen.texture = renderTexture;
    }
}
