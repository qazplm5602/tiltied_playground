using UnityEngine;
using UnityEngine.Playables;

public class CutsceneEntity : MonoBehaviour
{
    [SerializeField] private SoundSO _openingSound;
    [SerializeField] private SoundSO _inGameSound;
    
    private PlayableDirector director;
    private System.Action onFinish;
    
    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.stopped += OnStopped;
    }

    public void Play(System.Action cb)
    {
        SoundManager.Instance.PlayBGM(_openingSound);
        CameraManager.Instance.Transition.SetCamType(CameraType.Main);
        
        onFinish += cb;
        director.Play();
    }

    public void Stop()
    {
        director.Stop();
        onFinish = null;
    }

    private void OnStopped(PlayableDirector _) {
        SoundManager.Instance.PlayBGM(_inGameSound);

        onFinish?.Invoke();
        onFinish = null;
    }
}
