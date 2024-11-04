using UnityEngine;

public class HB_Player : MonoBehaviour
{
    // 나중에 본후 플레이어로 바꿀거임

    [SerializeField] private PlayerStatsSO _playerStat;
    public PlayerStatsSO Stat => _playerStat;

    private void Awake()
    {
        _playerStat = Instantiate(_playerStat);
        _playerStat.SetOwner(this);
    }
}
