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

    private void Start() {
        operation = SceneManager.LoadSceneAsync(toScene);
        operation.allowSceneActivation = false;
    }

    private void Update() {
        if (operation.isDone) return;
        operation.allowSceneActivation = true;

        lastScreenTex = ScreenCapture.CaptureScreenshotAsTexture(); // 혹시 쓸 수도 있으니..
    }
}
