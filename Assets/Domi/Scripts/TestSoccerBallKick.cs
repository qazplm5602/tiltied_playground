using System;
using UnityEngine;

public class TestSoccerBallKick : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private SimulationBallViewer viewer;
    [SerializeField] private BallGoalSimulateManager ballSimulater;

    [SerializeField] LayerMask obstacleLayer;

    [ContextMenu("kick")]
    public void KickBall() {
        rigid.AddForce(direction, ForceMode.Impulse);
        ballSimulater.SimulateBall(rigid.transform.position, direction);
        // viewer.pos = rigid.transform.position;
        // viewer.velocity = direction;
    }
}
