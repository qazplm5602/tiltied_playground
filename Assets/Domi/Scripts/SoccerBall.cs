using UnityEngine;

public class SoccerBall : MonoBehaviour
{
    public event System.Action<BallAreaType> OnGoal; // 골대에 들어감
    public event System.Action OnOut; // 공 나가짐
    public event System.Action OnReset;

    [SerializeField] private string outAreaTag = "OutArea";
    [SerializeField] private string spawnPointName = "BallSpawnPoint";

    private Transform spawnPoint;
    private Rigidbody rigid;

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

    private void Awake() {
        rigid = GetComponent<Rigidbody>();

        spawnPoint = GameObject.Find(spawnPointName)?.transform;

        if (spawnPoint == null)
            Debug.LogWarning("Not Found Ball Spawn Point");
    }

    public void BallReset() {
        rigid.linearVelocity = Vector3.zero;
        transform.position = spawnPoint.position;
        OnReset?.Invoke();
    }
}