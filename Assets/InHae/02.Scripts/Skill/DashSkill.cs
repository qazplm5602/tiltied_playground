using DG.Tweening;
using System.Collections;
using UnityEngine;

public class DashSkill : SkillBase
{
    [SerializeField] private SoundSO dashSound;
    [SerializeField] private ParticleSystem dashEffectPrefab;


    [SerializeField] private float _dashDuration;
    [SerializeField] private float _dashSpeed;


    private float _dashTime;
    private Rigidbody _rigid;

    private void Start()
    {
        _rigid = _player.transform.GetComponent<Rigidbody>();
        dashEffectPrefab = Instantiate(dashEffectPrefab, _player.transform.position, Quaternion.identity);
        dashEffectPrefab.transform.forward = -_player.transform.forward;
        dashEffectPrefab.transform.SetParent(_player.transform);
        dashEffectPrefab.Stop();
    }
    public override void UseSkill()
    {
        StartCoroutine(Dash());

        //SoundManager.Instance.PlaySFX(Vector3.zero, dashSound);
    }

    private IEnumerator Dash()
    {

        dashEffectPrefab.Play();

        while (true)
        {
            _dashTime += Time.deltaTime;
            if (_dashTime > _dashDuration)
            {
                _dashTime = 0;
                DOVirtual.DelayedCall(0.2f, () =>
                {
                    dashEffectPrefab.Stop();
                });
                break;
            }
            _rigid.linearVelocity = _player.transform.forward * _dashTime / _dashDuration * _dashSpeed;

            yield return null;
        }
    }
}
