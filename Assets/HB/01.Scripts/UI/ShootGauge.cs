using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ShootGauge : MonoBehaviour
{
    [SerializeField] private RectTransform _fillImage;
    private CanvasGroup _canvasGroup;
    private Player _player;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _player = GetComponentInParent<Player>();

        _player.ShootingStartEvent += HandleShootingStart;
        _player.ShootingEndEvent += HandleShootingEnd;

        _canvasGroup.alpha = 0;
    }

    void LateUpdate()
    {
        // Canvas가 카메라를 바라보도록 회전
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                         Camera.main.transform.rotation * Vector3.up);
    }

    private void HandleShootingStart()
    {
        _fillImage.localScale = new Vector3(0, 1, 1);
        _canvasGroup.alpha = 1;
        _fillImage.DOScaleX(1, 5);
    }

    private void HandleShootingEnd()
    {
        StartCoroutine(ShootingEnd());
    }

    private IEnumerator ShootingEnd()
    {
        _fillImage.DOKill(false);

        yield return new WaitForSeconds(1);

        _canvasGroup.alpha = 1;
        _fillImage.DOScaleX(0, 0);

        yield return null;
    }

    private void OnDestroy()
    {
        _player.ShootingStartEvent -= HandleShootingStart;
        _player.ShootingEndEvent -= HandleShootingEnd;
    }
}
