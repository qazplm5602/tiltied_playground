using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] private PoolManagerSO _poolManager;
    [SerializeField] private AudioMixer _audioMixer;
    
    private SoundPlayer _currentBGMPlayer = null;
    
    public void StopBGM()
    {
        _currentBGMPlayer?.StopAndGotoPool(true);
        _currentBGMPlayer = null;
    }

    public SoundPlayer PlayBGM(SoundSO clip)
    {
        _currentBGMPlayer?.StopAndGotoPool(true);
        _currentBGMPlayer = _poolManager.Pop(PoolType.SoundPlayer) as SoundPlayer;
        _currentBGMPlayer.PlaySound(clip);
        return _currentBGMPlayer;
    }

    public SoundPlayer PlaySFX(Vector3 pos, SoundSO clip)
    {
        SoundPlayer player = _poolManager.Pop(PoolType.SoundPlayer) as SoundPlayer;
        player.transform.position = pos;
        player.PlaySound(clip);
        
        return player;
    }

    public void SetAudioValue(string audioType, float value)
    {
        _audioMixer.SetFloat(audioType, Mathf.Log10(value) * 20);
    }
    
    public float GetAudioValue(string audioType)
    {
        _audioMixer.GetFloat(audioType, out float v);
        return v;
    }
}
