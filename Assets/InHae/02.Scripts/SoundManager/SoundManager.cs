using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] private PoolManagerSO _poolManager;
    
    private SoundPlayer _currentBGMPlayer = null;
    
    public void StopBGM()
    {
        _currentBGMPlayer?.StopAndGotoPool(true);
        _currentBGMPlayer = null;
    }

    public void PlayBGM(SoundSO clip)
    {
        _currentBGMPlayer?.StopAndGotoPool(true);
        _currentBGMPlayer = _poolManager.Pop(PoolType.SoundPlayer) as SoundPlayer;
        _currentBGMPlayer.PlaySound(clip);
    }

    public void PlaySFX(Vector3 pos, SoundSO clip)
    {
        SoundPlayer player = _poolManager.Pop(PoolType.SoundPlayer) as SoundPlayer;
        player.transform.position = pos;
        player.PlaySound(clip);
    }
}
