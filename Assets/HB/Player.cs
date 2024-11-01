using UnityEngine;

public class Player : MonoBehaviour
{
    // 나중에 본후 플레이어로 바꿀거임

    [SerializeField] private PlayerStats _playerStat;
    public PlayerStats Stat => _playerStat;
}
