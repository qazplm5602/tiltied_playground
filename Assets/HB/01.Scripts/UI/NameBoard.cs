using TMPro;
using UnityEngine;

public class NameBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _p1Name;
    [SerializeField] private TextMeshProUGUI _p2Name;

    private void Start()
    {
        SetPlayerName();
    }

    private void SetPlayerName()
    {
        _p1Name.text = GameDataManager.Instance.player1_StatData.playerName;
        _p2Name.text = GameDataManager.Instance.player2_StatData.playerName;
    }
}
