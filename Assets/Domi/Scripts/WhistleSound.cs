using UnityEngine;

public class WhistleSound : MonoBehaviour
{
    [SerializeField] SoundSO gameEndSound;
    [SerializeField] SoundSO ballOutSound;

    private SoccerBall soccerBall;

    private void Awake() {
        soccerBall = FindAnyObjectByType<SoccerBall>();
        soccerBall.OnOut += HandleBallOut;
    }

    private void OnDisable() {
        soccerBall.OnOut -= HandleBallOut;
    }

    public void PlayEndSound() {
        SoundManager.Instance.PlaySFX(Vector3.zero, gameEndSound);
    }

    private void HandleBallOut() {
        SoundManager.Instance.PlaySFX(Vector3.zero, ballOutSound);
    }
}
