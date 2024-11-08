using TMPro;
using UnityEngine;

public class NameBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _p1Name;
    [SerializeField] private TextMeshProUGUI _p2Name;

    private void SetPlayerName(string p1Name, string p2Name)
    {
        _p1Name.text = p1Name;
        _p2Name.text = p2Name;
    }
}
