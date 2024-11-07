using UnityEngine;

[CreateAssetMenu(fileName = "CheerleaderListSO", menuName = "SO/CheerleaderListSO")]
public class CheerleaderListSO : ScriptableObject
{
    [SerializeField] CheerleaderNPC[] characters;
}
