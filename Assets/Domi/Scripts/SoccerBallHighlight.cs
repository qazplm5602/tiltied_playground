using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SoccerBallHighlight : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;

    private SoccerBall soccerBall;
    private BallGoalSimulateManager simulateManager;

    private bool isHighlight = false;
    
    private void Awake() {
        soccerBall = GetComponent<SoccerBall>();
        simulateManager = ManagerManager.GetManager<BallGoalSimulateManager>();

        simulateManager.onWillGoal += HandleWillGoal;
        soccerBall.OnReset += HandleBallReset;
        soccerBall.OnGoal += HandleBallGoal;
        soccerBall.OnOut += DisableHighlight;
    }

    private void OnDestroy() {
        simulateManager.onWillGoal -= HandleWillGoal;
        soccerBall.OnReset -= HandleBallReset;
        soccerBall.OnGoal -= HandleBallGoal;
        soccerBall.OnOut -= DisableHighlight;
    }

    private void HandleWillGoal(BallAreaType type)
    {
        if (type != BallAreaType.Blue && type != BallAreaType.Red) return;
        isHighlight = true;

        Time.timeScale = 0.1f; // 시간 느리게
        List<CameraType> cameras = CameraManager.Instance.GetNearCam(CameraManager.NearType.Near, new CameraType[] { type == BallAreaType.Blue ? CameraType.Blue_L : CameraType.Orange_L, type == BallAreaType.Blue ? CameraType.Blue_R : CameraType.Orange_R }, transform.position);

        // 가까운거는
        CameraType nearCam = cameras[0];
        CameraManager.Instance.Transition.FadeChangeCam(nearCam);

        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, 1f).SetEase(Ease.OutQuad).SetUpdate(true);
    }

    private void HandleBallReset()
    {
        DisableHighlight(); // 리셋 ㄱㄱㄱ
    }

    private void HandleBallGoal(BallAreaType type)
    {
        DisableHighlight(); // 골 넣었넹
    }

    private void DisableHighlight() {
        isHighlight = false;
    }

    private void OnCollisionEnter(Collision other) {
        if (!isHighlight) return;

        int layerMask = 1 << other.gameObject.layer;
        print($"{obstacleLayer} & {layerMask}");
        if ((obstacleLayer & layerMask) != 0) { // ㅓ 머야 충돌했네
            isHighlight = false; // 취소 취소 warning 공을 찼는데 아쉽게도 다시 돌아옴

            // 다시 돌려
            CameraManager.Instance.Transition.FadeChangeCam(CameraType.Main);
        }
    }
}
