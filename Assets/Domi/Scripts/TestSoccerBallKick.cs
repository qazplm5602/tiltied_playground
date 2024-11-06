using System;
using UnityEngine;

public class TestSoccerBallKick : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private SimulationBallViewer viewer;
    [SerializeField] private BallGoalSimulateManager ballSimulater;

    [SerializeField] LayerMask obstacleLayer;
    SoccerBall soccerBall;

    bool isHighlight = false;
    
    private void Awake() {
        soccerBall = rigid.GetComponent<SoccerBall>();

        ballSimulater.onWillGoal += HandleWillGoal;
        soccerBall.OnReset += HandleBallReset;
        soccerBall.OnGoal += HandleBallGoal;
        soccerBall.OnOut += DisableHighlight;
    }

    private void OnDestroy() {
        ballSimulater.onWillGoal -= HandleWillGoal;
        soccerBall.OnReset -= HandleBallReset;
        soccerBall.OnGoal -= HandleBallGoal;
        soccerBall.OnOut -= DisableHighlight;
    }

    private void HandleBallReset()
    {
        DisableHighlight(); // 리셋 ㄱㄱㄱ
    }

    private void HandleWillGoal(BallAreaType type)
    {
        isHighlight = true;
    }

    [ContextMenu("kick")]
    public void KickBall() {
        rigid.AddForce(direction, ForceMode.Impulse);
        ballSimulater.SimulateBall(rigid.transform.position, direction);
        // viewer.pos = rigid.transform.position;
        // viewer.velocity = direction;
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
