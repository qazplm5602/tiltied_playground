using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SoccerBallFloor : MonoBehaviour
{
    [SerializeField] float minDistance = 2f; // 이것보단 높아야 데칼이 보임
    [SerializeField] float maxDistance = 100f; // 레이 어디까지 쏨??
    [SerializeField] float margin = 0.5f; // 얼마나 더 내림?
    [SerializeField] LayerMask groundLayer;
    DecalProjector decal;

    Transform soccerBall;
    
    private void Awake() {
        decal = GetComponent<DecalProjector>();
        soccerBall = transform.parent;
        
        // 부모 삭제 ( 욕 아님 )
        transform.SetParent(null);
    }

    private void Update() {
        Vector3 start = soccerBall.position;
        bool success = Physics.Raycast(start, Vector3.down, out var hitInfo, maxDistance, groundLayer);
    
        if (!success || hitInfo.distance < minDistance) {
            decal.enabled = false;
            return;
        }

        decal.enabled = true;
        transform.position = start + Vector3.down * (hitInfo.distance + margin);
    }
}
