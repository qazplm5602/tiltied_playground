using UnityEngine;

[CreateAssetMenu(menuName = "SO/GameModeSO")]
public class GameModeSO : ScriptableObject
{
    public string gameName;
    public Sprite icon; // 필요한지는 모르겠음
    public GameMode system;
}
