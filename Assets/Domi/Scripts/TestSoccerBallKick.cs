using UnityEngine;

public class TestSoccerBallKick : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private SimulationBallViewer viewer;
    [SerializeField] private BallGoalSimulateManager ballSimulater;

    [ContextMenu("kick")]
    private void KickBall() {
        rigid.AddForce(direction, ForceMode.Impulse);
        ballSimulater.SimulateBall(rigid.transform.position, direction);
        // viewer.pos = rigid.transform.position;
        // viewer.velocity = direction;
    }
}
