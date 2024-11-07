using UnityEngine;

public class CheerleaderPoint : MonoBehaviour
{
    [field: SerializeField] public bool AllowSeat { get; private set; } = true; // 앉을 수 있는 자리임??
    [field: SerializeField] public BallAreaType Team { get; private set; } // 어디 팀임

}
