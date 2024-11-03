using UnityEngine;

public class Player : MonoBehaviour
{
    // ���߿� ���� �÷��̾�� �ٲܰ���

    [SerializeField] private PlayerStatsSO _playerStat;
    public PlayerStatsSO Stat => _playerStat;

    private void Awake()
    {
        _playerStat = Instantiate(_playerStat);
        _playerStat.SetOwner(this);
    }
}
