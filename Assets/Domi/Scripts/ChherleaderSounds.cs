using UnityEngine;

[System.Serializable]
public class CheerleaderSoundClips {
    [SerializeField] SoundSO[] list;

    public SoundSO Get() {
        SoundSO randCheer = list[Random.Range(0, list.Length)];
        return randCheer;
    }
}

public class ChherleaderSounds : MonoBehaviour
{
    [SerializeField] private CheerleaderSoundClips goalCheer; // 이거 골 넣었을때 하는거
    [SerializeField] private CheerleaderSoundClips lowCheer; // 이거 골 넣을거 같다 싶으면

    private SoccerBall soccerBall;
    private SoundPlayer lowSound = null;

    private void Awake() {
        soccerBall = FindAnyObjectByType<SoccerBall>();
        soccerBall.OnGoal += HandleBallGoal;

        var goalSimulate = ManagerManager.GetManager<BallGoalSimulateManager>();
        goalSimulate.onWillGoal += HandleWillGoal;
    }

    private void HandleBallGoal(BallAreaType _) {
        LowSoundStop();

        SoundManager.Instance.PlaySFX(Vector3.zero, goalCheer.Get());
    }

    private void HandleWillGoal(BallAreaType _) {
        LowSoundStop();
        
        lowSound = SoundManager.Instance.PlaySFX(Vector3.zero, lowCheer.Get());
        lowSound.OnPlayEnd += OnRemoveLowSound;
    }

    private void OnRemoveLowSound() {
        lowSound.OnPlayEnd -= OnRemoveLowSound;
        lowSound = null;
    }

    private void OnDestroy() {
        if (lowSound != null)
            lowSound.OnPlayEnd -= OnRemoveLowSound;
    }

    private void LowSoundStop() {
        if (lowSound == null) return;
        lowSound.StopAndGotoPool(true);

        OnRemoveLowSound(); // 바로 이벤트 직접 해제 ㅁㄴㅇㄹ
    }
}
