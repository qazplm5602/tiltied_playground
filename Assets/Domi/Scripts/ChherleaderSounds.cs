using UnityEngine;

public class ChherleaderSounds : MonoBehaviour
{
    [SerializeField] SoundSO[] goalCheers;

    private SoccerBall soccerBall;
    
    private void Awake() {
        soccerBall = FindAnyObjectByType<SoccerBall>();
        soccerBall.OnGoal += HandleBallGoal;
    }

    private void HandleBallGoal(BallAreaType _) {
        SoundSO randCheer = goalCheers[Random.Range(0, goalCheers.Length)];
        SoundManager.Instance.PlaySFX(Vector3.zero, randCheer);
    }
}
