using UnityEngine;

public class Player : MonoBehaviour
{
    // ���߿� ���� �÷��̾�� �ٲܰ���

    [SerializeField] private PlayerStats _playerStat;
    public PlayerStats Stat => _playerStat;
}
