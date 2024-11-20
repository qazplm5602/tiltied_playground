using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeditationSkill : SkillBase
{
    [SerializeField] private float _meditationTime = 5.0f;
    [SerializeField] private Material _mat;
    [SerializeField] private ParticleSystem _particlePrefab;
    
    private Player _otherPlayer;
    private GroundTiltied _ground;
    private SkinnedMeshRenderer[] _renderers;

    private void Awake() {
        _renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

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
        _ground.enabled = false;
        _otherPlayer.IsMeditation = true;
        
        foreach (var item in _renderers)
        {
            List<Material> mats = item.materials.ToListPooled();
            item.materials = mats.ToArray();
        }

        // 이펙트
        Instantiate(_particlePrefab, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(_meditationTime);

        foreach (var item in _renderers)
        {
            List<Material> mats = item.materials.ToListPooled();
            mats.Remove(_mat);

            item.materials = mats.ToArray();
        }

        _ground.enabled = true;
        _otherPlayer.IsMeditation = false;
    }
}
