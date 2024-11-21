using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShootGauge : MonoBehaviour
{
    [SerializeField] private RectTransform _fill;
    private Image _fillImage;
    private CanvasGroup _canvasGroup;
    private Player _player;

    private bool _isShooting = false;
    private float _parentWidth;

    private Sequence _colorSequence;
    private Tweener _scaleTweener;
    private Tweener _fadeTweener;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _player = GetComponentInParent<Player>();
        _fillImage = _fill.GetComponent<Image>();
        _parentWidth = (_fill.parent as RectTransform).sizeDelta.x;
        print($"_parentWidth {_parentWidth}");

        _player.ShootingStartEvent += HandleShootingStart;
        _player.ShootingEndEvent += HandleShootingEnd;

        _canvasGroup.alpha = 0;
        _fillImage.color = new Color(1, 0, 0);
    }

    private void Update()
    {

    }

    private void LateUpdate()
    {
        // Canvas가 카메라를 바라보도록 회전
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                         Camera.main.transform.rotation * Vector3.up);
    }

    private void HandleShootingStart()
    {
        _isShooting = true;
        TweenClear();

        _fill.offsetMax = new Vector2(-_parentWidth, _fill.offsetMax.y);
        print($"{_parentWidth} / {_fill.offsetMax}");
        // _canvasGroup.alpha = 1;
        _fadeTweener = _canvasGroup.DOFade(1, 0.3f);


        // _scaleTweener = _fill.DOAnchorMax(new Vector2(0, _fill.offsetMax.y), 5).SetEase(Ease.Linear);
        _scaleTweener = DOTween.To(() =>  _fill.offsetMax.x, (value) => {
            _fill.offsetMax = new Vector2(value, _fill.offsetMax.y);
        }, 0, 5).SetEase(Ease.Linear);
    }

    private void HandleShootingEnd()
    {
        _isShooting = false;
        StartCoroutine(ShootingEnd());
    }

    private IEnumerator ShootingEnd()
    {
        TweenClear();

        yield return new WaitForSeconds(1);

        _fadeTweener = _canvasGroup.DOFade(0, 0.8f);
        // _fill.localScale = new Vector3(0, 1, 1);ssss
    }

    private void OnDestroy()
    {
        _player.ShootingStartEvent -= HandleShootingStart;
        _player.ShootingEndEvent -= HandleShootingEnd;

        _colorSequence?.Kill();
        _scaleTweener?.Kill();
    }

    private void TweenClear() {
        _colorSequence?.Kill();
        _scaleTweener?.Kill();
        _fadeTweener?.Kill();
    }
}
