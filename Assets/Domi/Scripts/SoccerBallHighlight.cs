using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SoccerBallHighlight : MonoBehaviour
{
    private SoccerBall soccerBall;
    private BallGoalSimulateManager simulateManager;
    private Rigidbody rigid;
    [SerializeField] private float expireTime = 8f;

    private bool isHighlight = false;
    private Vector3 lastGoalPostPos;
    
    private void Awake() {
        soccerBall = transform.GetComponent<SoccerBall>();
        simulateManager = ManagerManager.GetManager<BallGoalSimulateManager>();
        rigid = GetComponent<Rigidbody>();

        simulateManager.onWillGoal += HandleWillGoal;
        soccerBall.OnReset += HandleBallReset;
        soccerBall.OnGoal += HandleBallGoal;
        soccerBall.OnOut += DisableHighlight;
    }

    private float lastVelocity;
    private float lastGoalPostDist;
    private float distWarning = 0; // 골대랑 멀리 가는거 시간 체크
    private float highlightTime = 0;
    
    private void Update() {
        if (!isHighlight) return;

        float velocity = rigid.linearVelocity.magnitude;
        float distance = Vector3.Distance(lastGoalPostPos, transform.position);

        highlightTime += Time.deltaTime;
        if (highlightTime > expireTime) { // 시간끗.
            CancelHighlight();
            return;
        }

        if (lastVelocity > velocity && (lastVelocity - velocity) >= 10) { // 힘이 급격하게 줄어듬 ㅅㄱ
            print("velocity 즉시 slow.");
            CancelHighlight();
            return;
        }

        if (lastGoalPostDist < distance) { // 거리가 멀어짐 ㄷㄷ
            distWarning += Time.deltaTime;
        } else distWarning = 0;

        if (distWarning >= 2f) // 2초동안 계속 멀어짐..
            CancelHighlight();

        lastVelocity = velocity;
        lastGoalPostDist = distance;
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
        lastGoalPostPos = ManagerManager.GetManager<BallGoalSimulateManager>().GetGoalPost(type).transform.position;

        lastVelocity = rigid.linearVelocity.magnitude;
        lastGoalPostDist = Vector3.Distance(lastGoalPostPos, transform.position);
        distWarning = highlightTime = 0;

        Time.timeScale = 0.1f; // 시간 느리게
        List<CameraType> cameras = CameraManager.Instance.GetNearCam(CameraManager.NearType.Near, new CameraType[] { type == BallAreaType.Blue ? CameraType.Blue_L : CameraType.Orange_L, type == BallAreaType.Blue ? CameraType.Blue_R : CameraType.Orange_R }, transform.position);

        print($"[{type}] {(type == BallAreaType.Blue ? CameraType.Blue_L : CameraType.Orange_L)}, {(type == BallAreaType.Blue ? CameraType.Blue_R : CameraType.Orange_R)}");

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

    private void CancelHighlight() {
        DisableHighlight();
        CameraManager.Instance.Transition.FadeChangeCam(CameraType.Main);
    }
}
