using UnityEngine;

public class BlindSkill : SkillBase
{
    private Player _otherPlayer;

    private void Start()
    {
        Player bluePlayer = ManagerManager.GetManager<PlayerManager>().GetPlayer(BallAreaType.Blue);
        Player redPlayer = ManagerManager.GetManager<PlayerManager>().GetPlayer(BallAreaType.Red);

        _otherPlayer = bluePlayer == _player ? redPlayer : bluePlayer;
    }

    public override void UseSkill()
    {

    }
}
