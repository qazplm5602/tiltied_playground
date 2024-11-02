using UnityEngine;

public class TestSoccerBallKick : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private SimulationBallViewer viewer;

    [ContextMenu("kick")]
    private void KickBall() {
        rigid.AddForce(direction, ForceMode.Impulse);
        viewer.pos = rigid.transform.position;
        viewer.velocity = direction;
    }
}
