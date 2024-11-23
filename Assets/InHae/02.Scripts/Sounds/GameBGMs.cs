using UnityEngine;

public class GameBGMs : BaseSoundHelper
{
    [SerializeField] private SoundSO _gameEndSound;

    public void GameEndSound() => SoundManager.Instance.PlayBGM(_gameEndSound);
}
