using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    ///////////////////// static
    private static string toScene = null;
    public static Texture lastScreenTex = null;
    public static void LoadScene(string sceneName) {
        toScene = sceneName;
        SceneManager.LoadScene("Loading");
    }


    ///////////////////// instance
    private AsyncOperation operation;
    private float forceWaitTime = 3f; // 3초는 기다려야함 (로딩시간 포함)

    private void Start() {
        operation = SceneManager.LoadSceneAsync(toScene);
        operation.allowSceneActivation = false;
    }

    private void Update() {
        forceWaitTime -= Time.deltaTime;
        if (!operation.isDone || forceWaitTime > 0) return;

        operation.allowSceneActivation = true;

        lastScreenTex = ScreenCapture.CaptureScreenshotAsTexture(); // 혹시 쓸 수도 있으니..
    }
}
