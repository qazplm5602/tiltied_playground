using UnityEngine;

public class Player : MonoBehaviour
{
    // 나중에 본후 플레이어로 바꿀거임

    [SerializeField] private PlayerStatsSO _playerStat;
    public PlayerStatsSO Stat => _playerStat;
}
