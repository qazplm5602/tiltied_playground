using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BuildWallSkill : SkillBase
{
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private ParticleSystem _buildEffect;
    
    [SerializeField] private float _wallBuildTime;
    [SerializeField] private float _wallDurationTime;
    [SerializeField] private float _wallBuildDistance;
    [SerializeField] private SoundSO _buildSound;

    [SerializeField] private LayerMask _groundMask;

    private Transform _wallTrm;
    private Ground _ground;
    
    private Vector3 _rayPos;
    private float _effectPosY;
    
    private ParticleSystem _buildParticle;

    public override bool SkillUseAbleCheck()
    {
        if (_wallTrm is not null)
            return false;
        
        return base.SkillUseAbleCheck();
    }

    public override void UseSkill()
    {
        _rayPos = transform.position;
        Vector3 buildForward = transform.forward * _wallBuildDistance;
        
        _rayPos += buildForward;
        _rayPos.y += 8;

        bool isHit = Physics.Raycast(_rayPos, Vector3.down, out RaycastHit hit, 10, _groundMask);

        if (isHit)
        {
            if(_ground == null)
                _ground = hit.collider.GetComponentInParent<Ground>();
            _ground._clearEvent += DestroyWall;
            
            _wallTrm = Instantiate(_wallPrefab, transform.position, Quaternion.LookRotation(transform.forward)).transform;
            _wallTrm.SetParent(_ground.transform);
            _wallTrm.localRotation = Quaternion.Euler(0, _wallTrm.eulerAngles.y, 0);
                
            _buildParticle = Instantiate(_buildEffect, _ground.transform);
            
            Vector3 defaultSpawnPos = hit.point;
            
            _effectPosY = defaultSpawnPos.y + _wallTrm.localScale.y * 1f;
            
            Vector3 downPos = defaultSpawnPos;
            downPos.y -= _wallTrm.localScale.y * 0.5f + 1f;
            _wallTrm.position = downPos;

            WallLocalMoveProcess(defaultSpawnPos,_wallTrm.localScale.y * 0.5f, false);
            
            DOVirtual.DelayedCall(_wallDurationTime, () => WallLocalMoveProcess(defaultSpawnPos,
                -(_wallTrm.localScale.y * 0.5f) - 0.5f, true));
        }
    }

    private void WallLocalMoveProcess(Vector3 defaultSpawnPos, float movePosY, bool isDestroy)
    {
        SoundManager.Instance.PlaySFX(transform.position, _buildSound);
        BuildEffectStart(defaultSpawnPos, Quaternion.LookRotation(_wallTrm.up));
        
        _wallTrm.DOLocalMoveY(movePosY, _wallBuildTime).OnComplete(() =>
        {
            _buildParticle.Stop();
            if (isDestroy)
                StartCoroutine(WaitEffect());
        });
    }

    private void BuildEffectStart(Vector3 defaultSpawnPos ,Quaternion rotation)
    {
        Vector3 effectPos = defaultSpawnPos;
        effectPos.y = _effectPosY;
        
        _buildParticle.transform.position = effectPos;
        _buildParticle.transform.rotation = rotation;
        _buildParticle.Play();
    }

    private IEnumerator WaitEffect()
    {
        yield return new WaitUntil(() => !_buildParticle.IsAlive());
        DestroyWall();
    }

    private void DestroyWall()
    {
        _ground._clearEvent -= DestroyWall;

        if (_wallTrm is not null)
        {
            Destroy(_wallTrm.gameObject);
            _wallTrm = null;
        }
        if (_buildParticle is not null)
        {
            Destroy(_buildParticle.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_rayPos, Vector3.down * 10);
    }
}
