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
    private Transform visual;
    private Player owner; // 공 가지고 잇는 사람
    private BallControlBundle ownerControl;
    private BallGoalSimulateManager ballSimulater;

    private void Awake() {
        rigid = GetComponent<Rigidbody>();
        ballSimulater = ManagerManager.GetManager<BallGoalSimulateManager>();
        visual = GameObject.Find("Visual").transform;

        spawnPoint = GameObject.Find(spawnPointName)?.transform;

        if (spawnPoint == null)
            Debug.LogWarning("Not Found Ball Spawn Point");
    }

    private void Update() {
        if (owner == null) return; // owner 이 없으면 분리 되어있지 않음
        
        transform.position = visual.position;
    }

    public void BallReset() {
        rigid.linearVelocity = rigid.angularVelocity = Vector3.zero;
        transform.position = spawnPoint.position;
        OnReset?.Invoke();
    }

    void OnTriggerEnter(Collider other) {
        if (!ManagerManager.GetManager<GameManager>().GetMode().IsPlay) return; // 정지 상태이면 안함 ㅅㄱ

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

    public Transform TakePlayerBall(Player ballOwner, BallControlBundle ballControl)
    {
        owner = ballOwner;
        ownerControl = ballControl;

        visual.SetParent(ballOwner.transform, true);

        rigid.isKinematic = true;
        rigid.Sleep();

        return visual;
    }

    public void RemoveOwner(bool controlRequest = true) {
        if (owner == null) return;

        visual.SetParent(transform, true);

        if (controlRequest)
            ownerControl.Release(owner);

        rigid.isKinematic = false;
        rigid.WakeUp();
        
        owner = null;
        ownerControl = null;
    }

    public void Kick(Vector3 direction) {
        RemoveOwner(); // 자동 삭제

        rigid.AddForce(direction, ForceMode.Impulse);
        ballSimulater.SimulateBall(rigid.transform.position, direction);
    }
}