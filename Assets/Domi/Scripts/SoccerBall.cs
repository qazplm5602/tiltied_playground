using UnityEngine;

public class SoccerBall : MonoBehaviour
{
    public event System.Action<BallAreaType> OnGoal; // 골대에 들어감
    public event System.Action OnOut; // 공 나가짐

    [SerializeField] string outAreaTag = "OutArea";

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(outAreaTag)) {
            OnOut?.Invoke();
            print("Out!!");
        }

        if (other.TryGetComponent<GoalPostArea>(out var compo)) {
            BallAreaType area = compo.GetArea();
            OnGoal?.Invoke(area);
            print($"Goal!! {area}");
        }
    }
}
