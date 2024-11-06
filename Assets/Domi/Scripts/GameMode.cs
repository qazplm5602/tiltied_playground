using UnityEngine;

public abstract class GameMode : MonoBehaviour
{
    public int RedScore { get; protected set; }
    public int BlueScore { get; protected set; }

    public abstract void GameStart();
}
