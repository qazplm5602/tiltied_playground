using UnityEngine;

public class BlindSkill : SkillBase
{
    [SerializeField] private SoundSO _sound;
    [SerializeField] private float _blindTime = 5.0f;
    private Player _otherPlayer;

    private void Start()
    {
        Player bluePlayer = ManagerManager.GetManager<PlayerManager>().GetPlayer(BallAreaType.Blue);
        Player redPlayer = ManagerManager.GetManager<PlayerManager>().GetPlayer(BallAreaType.Red);

        _otherPlayer = bluePlayer == _player ? redPlayer : bluePlayer;
    }

    public override void UseSkill()
    {
        SoundManager.Instance.PlaySFX(transform.position, _sound);
        _otherPlayer.BlindSkill(_blindTime);
    }
}
