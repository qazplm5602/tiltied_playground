using UnityEngine;

public class CutSceneHandler : MonoBehaviour
{
    public void FadeCamHandle(string camName)
    {
        CameraManager.Instance.Transition.FadeChangeCam(camName);
    }
}
