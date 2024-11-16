using UnityEngine;
using UnityEngine.Playables;

public class CutsceneEntity : MonoBehaviour
{
    PlayableDirector director;

    private void Awake() {
        director = GetComponent<PlayableDirector>();
    }
}
