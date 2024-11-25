using UnityEngine;

public class PlayListBGM : MonoBehaviour
{
    [SerializeField] SoundSO[] playList;

    private SoundPlayer currentSound;

    private void Start() {
        Play();
    }

    private void OnDisable() {
        if (currentSound) {
            currentSound.OnPlayEnd -= OnEndSound;
            SoundManager.Instance.StopBGM();
        }
    }
    
    private void Play() {
        SoundSO sound = GetRandomBGM();
        currentSound = SoundManager.Instance.PlayBGM(sound);

        currentSound.OnPlayEnd += OnEndSound;
    }

    private void OnEndSound() {
        currentSound.OnPlayEnd -= OnEndSound;
        Play();
    }

    private SoundSO GetRandomBGM() => playList[Random.Range(0, playList.Length)];
}
