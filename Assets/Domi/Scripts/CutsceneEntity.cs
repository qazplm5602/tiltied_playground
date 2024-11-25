using UnityEngine;
using UnityEngine.Playables;

public class CutsceneEntity : MonoBehaviour
{
    private PlayableDirector director;
    private System.Action onFinish;
    
    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.stopped += OnStopped;
    }

    public void Play(System.Action cb) {
        CameraManager.Instance.Transition.SetCamType(CameraType.Main);
        
        onFinish += cb;
        director.Play();
    }

    public void Stop() {
        director.Stop();
        onFinish = null;
    }

    private void OnStopped(PlayableDirector _) {
        onFinish?.Invoke();
        onFinish = null;
    }
}
