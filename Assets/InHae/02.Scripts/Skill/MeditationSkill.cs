using System.Collections;
using UnityEngine;

public class MeditationSkill : SkillBase
{
    [SerializeField] private float _meditationTime = 5.0f;
    private Player _otherPlayer;
    private GroundTiltied _ground;

    private void Start()
    {
        Player bluePlayer = ManagerManager.GetManager<PlayerManager>().GetPlayer(BallAreaType.Blue);
        Player redPlayer = ManagerManager.GetManager<PlayerManager>().GetPlayer(BallAreaType.Red);
        _ground = FindAnyObjectByType<GroundTiltied>();

        _otherPlayer = bluePlayer == _player ? redPlayer : bluePlayer;
    }

    public override void UseSkill()
    {
        StartCoroutine(Meditation());
    }

    private IEnumerator Meditation()
    {
        _otherPlayer.IsMeditation = _ground.enabled = false;

        yield return new WaitForSeconds(_meditationTime);

        _otherPlayer.IsMeditation = _ground.enabled = true;
    }
}
