using UnityEngine;

[CreateAssetMenu(fileName = "CheerleaderListSO", menuName = "SO/CheerleaderListSO")]
public class CheerleaderListSO : ScriptableObject
{
    [SerializeField] CheerleaderNPC[] characters;

    public CheerleaderNPC GetRandom() {
        return characters[Random.Range(0, characters.Length)];
    }
}
