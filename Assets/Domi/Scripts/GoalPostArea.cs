using UnityEngine;

public class GoalPostArea : MonoBehaviour
{
    [SerializeField] BallAreaType areaType;
    [SerializeField] LayerMask pointLayer;
    BoxCollider boxCollider;

    private void Awake() {
        boxCollider = GetComponent<BoxCollider>();
    }

    Collider[] detectColliders = new Collider[1]; // 이거 쓸떄가 있지 않을가????
    public bool HasPointIn() {
        int amount = Physics.OverlapBoxNonAlloc(transform.position + boxCollider.center, boxCollider.size / 2f, detectColliders, transform.rotation, pointLayer);
        return amount > 0;
    }

    public BallAreaType GetArea() => areaType;
}
