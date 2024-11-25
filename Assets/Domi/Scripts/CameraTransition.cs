using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraTransition : MonoBehaviour
{
    [SerializeField] private float fadeOutDuration = 0.3f;
    [SerializeField] private string screenName = "CamScreen";

    private RenderTexture renderTexture = null;
    private RawImage screen;
    private CameraType currentCamType = CameraType.Main;
    private Sequence process;
    
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
        if (cam == currentCamType) return; // 같은 카메라는 안ㅇ해 (인해 아님)

        if (process != null) {
            process.Kill(true);
        }

        // Screen.width
        if (renderTexture == null) {
            CreateRenderTexture();
        }

        Camera oldCam = CameraManager.Instance.GetCamera(currentCamType);
        // oldCam.targetDisplay = 2; // 렌더 텍스쳐 적용하면 필요 없어짐 ㅎㅎ

        Camera nowCam = CameraManager.Instance.GetCamera(cam);

        // print($"{currentCamType} -> {cam}");

        // 활성화
        screen.gameObject.SetActive(true);
        screen.color = Color.white;

        // 애님
        process = DOTween.Sequence();
        process.Append(screen.DOFade(0, fadeOutDuration).SetEase(Ease.Linear).SetUpdate(true)).SetUpdate(true);
        process.OnStart(() => {
            oldCam.targetTexture = renderTexture;
            nowCam.targetDisplay = 0;
        });
        process.OnComplete(() => {
            oldCam.targetDisplay = 2;
            oldCam.targetTexture = null;
            screen.gameObject.SetActive(false);
            process = null;
        });

        currentCamType = cam;
    }
    public void FadeChangeCam(string camName) { // 타임라인 지원
        FadeChangeCam((CameraType)Enum.Parse(typeof(CameraType), camName));
    }

    public void FadeChangeCamNoLive(CameraType cam, System.Action cb) {
        if (process != null) { // 하고 있으면 끈엉
            process.Kill(true);
        }

        if (renderTexture == null) {
            CreateRenderTexture();
        }

        StartCoroutine(FadeScreenshot(cam, cb));
    }

    public void FadeChangeCamNoLive(CameraType cam) {
        FadeChangeCamNoLive(cam, null);
    }

    public CameraType GetCurrentCam() => currentCamType;

    private void CreateRenderTexture() {
        // renderTexture = new RenderTexture(Screen.width, Screen.height, 16);
        renderTexture = new RenderTexture(1920, 1080, 32);
        screen.texture = renderTexture;
    }

    [ContextMenu("Transition Cam")]
    private void TestCode() {
        FadeChangeCam((CameraType)(((int)currentCamType + 1) % 5));
    }

    
    private void Update() {
        // if (Keyboard.current.gKey.wasPressedThisFrame)
            // TestCode();
    }

    IEnumerator FadeScreenshot(CameraType cam, System.Action onNextFrame) {
        Canvas[] uiFrames = FindObjectsByType<Canvas>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        
        // 전부다 ui를 끄고
        foreach (var item in uiFrames)
            item.enabled = false;
    
        yield return new WaitForEndOfFrame(); // 반영될때까지 ㄱㄷ

        var texture = ScreenCapture.CaptureScreenshotAsTexture(); // 찰칵

        onNextFrame?.Invoke();

        // 다시 켜
        foreach (var item in uiFrames)
            item.enabled = true;
        
        Camera oldCam = CameraManager.Instance.GetCamera(currentCamType);
        Camera newCam = CameraManager.Instance.GetCamera(cam);

        screen.color = Color.white;
        screen.texture = texture;

        process = DOTween.Sequence();
        process.Append(screen.DOFade(0, fadeOutDuration).SetEase(Ease.Linear));
        process.OnStart(() => {
            screen.gameObject.SetActive(true);

            if (oldCam == newCam) return;

            oldCam.targetDisplay = 2;
            newCam.targetDisplay = 0;
        });
        process.OnComplete(() => {
            screen.gameObject.SetActive(false);
        });

        currentCamType = cam;
    }

    public void SetCamType(CameraType cameraType) => currentCamType = cameraType;
}
