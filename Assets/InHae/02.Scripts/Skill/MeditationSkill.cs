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
    [SerializeField] private SoundSO _sfxSO;
    
    private Player _otherPlayer;
    private GroundTiltied _ground;
    private SkinnedMeshRenderer[] _targetRenderers;

    private void Awake() {
    }

    private void Start()
    {
        Player bluePlayer = ManagerManager.GetManager<PlayerManager>().GetPlayer(BallAreaType.Blue);
        Player redPlayer = ManagerManager.GetManager<PlayerManager>().GetPlayer(BallAreaType.Red);
        _ground = FindAnyObjectByType<GroundTiltied>();

        _otherPlayer = bluePlayer == _player ? redPlayer : bluePlayer;
        _targetRenderers = _otherPlayer.GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    public override void UseSkill()
    {
        StartCoroutine(Meditation());
    }
    
    private IEnumerator Meditation()
    {
        _ground.enabled = false;
        _otherPlayer.IsMeditation = true;
        
        foreach (var item in _targetRenderers)
        {
            List<Material> mats = item.sharedMaterials.ToListPooled();
            mats.Add(_mat);

            item.sharedMaterials = mats.ToArray();
        }

        // 이펙트
        Instantiate(_particlePrefab, _otherPlayer.transform.position, Quaternion.identity);

        // 소리
        SoundManager.Instance.PlaySFX(Vector3.zero, _sfxSO);

        yield return new WaitForSeconds(_meditationTime);

        foreach (var item in _targetRenderers)
        {
            List<Material> mats = item.sharedMaterials.ToListPooled();
            mats.Remove(_mat);

            item.sharedMaterials = mats.ToArray();
        }

        _ground.enabled = true;
        _otherPlayer.IsMeditation = false;
    }
}
