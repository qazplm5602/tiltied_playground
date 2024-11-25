using System.Collections;
using DG.Tweening;
using UnityEngine;

public class SkillWallObj : MonoBehaviour
{
    private ParticleSystem _buildParticle;
    private BuildWallSkill _skill;
    private Vector3 _defaultSpawnPos;
    
    private Coroutine _effectWaitCoroutine;
    private Coroutine _waitCoroutine;
    private Tweener _tweener;

    public void WallInit(BuildWallSkill skill, Vector3 defaultSpawnPos, ParticleSystem buildParticle)
    {
        _skill = skill;
        _defaultSpawnPos = defaultSpawnPos;
        _buildParticle = buildParticle;
        
        Vector3 downPos = _defaultSpawnPos;
        downPos.y -= transform.localScale.y * 0.5f + 1f;
        transform.position = downPos;
        
        WallLocalMove(transform.localScale.y * 0.5f, false, _skill.buildSound);
        _waitCoroutine = StartCoroutine(WaitDownDuration());
    }
    
    private IEnumerator WaitDownDuration()
    {
        yield return new WaitForSeconds(_skill.wallDurationTime);
        WallLocalMove(-(transform.localScale.y * 0.5f) - 0.5f, true, _skill.destroySound);
    }
    
    private void WallLocalMove(float movePosY, bool isDestroy, SoundSO soundSo)
    {
        SoundManager.Instance.PlaySFX(transform.position, soundSo);
        BuildEffectStart();
        
        _tweener = transform.DOLocalMoveY(movePosY, _skill.wallBuildTime).OnComplete(() =>
        {
            _buildParticle.Stop();
            if (isDestroy)
                _effectWaitCoroutine = StartCoroutine(WaitEffect());
        });
    }

    private IEnumerator WaitEffect()
    {
        yield return new WaitUntil(() => !_buildParticle.IsAlive());
        DestroySkill();
    }

    private void BuildEffectStart()
    {
        Vector3 effectPos = _defaultSpawnPos;
        effectPos.y += transform.localScale.y;

        Transform particleTrm = _buildParticle.transform;
        particleTrm.position = effectPos;
        particleTrm.localRotation = Quaternion.LookRotation(transform.forward);
        particleTrm.localRotation = Quaternion.Euler(90f, particleTrm.eulerAngles.y, 0);

        _buildParticle.Play();
    }

    public void DestroySkill()
    {
        if (_tweener != null)
            _tweener.Kill();
        
        if (_waitCoroutine != null)
            StopCoroutine(_waitCoroutine);

        if (_effectWaitCoroutine != null)
            StopCoroutine(_effectWaitCoroutine);

        Destroy(gameObject);
        Destroy(_buildParticle.gameObject);
    }
}
