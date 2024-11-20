using System.Collections;
using UnityEngine;

public class FartSkill : SkillBase
{
    [SerializeField] private GameObject fartEffectPrefab;
    [SerializeField] private GameObject massPrefab;
    [SerializeField] private SoundSO fartSound;

    public override void UseSkill()
    {
        Transform fartTrm = Instantiate(fartEffectPrefab).transform;
        fartTrm.position = transform.position;
        fartTrm.forward = -transform.forward; // 플레이어 반대 방향
        
        StartCoroutine(NextFrameRemove(Instantiate(massPrefab, transform.position, Quaternion.identity)));

        // 소리
        SoundManager.Instance.PlaySFX(Vector3.zero, fartSound);
    }

    private IEnumerator NextFrameRemove(GameObject entity) {
        yield return new WaitForFixedUpdate();
        Destroy(entity);
    }
}
