               using UnityEngine;

public class GoalPostArea : MonoBehaviour
{
    [SerializeField] BallAreaType areaType;
    [SerializeField] LayerMask pointLayer;
    [SerializeField] BoxCollider simulateBox;

    private void Update() {
        HasPointIn();
    }

    Collider[] detectColliders = new Collider[1]; // 이거 쓸떄가 있지 않을가????
    public bool HasPointIn() {
        int amount = Physics.OverlapBoxNonAlloc(simulateBox.transform.position + simulateBox.center, simulateBox.size / 2f, detectColliders, simulateBox.transform.rotation, pointLayer);
        return amount > 0;
    }

    public BallAreaType GetArea() => areaType;
}
