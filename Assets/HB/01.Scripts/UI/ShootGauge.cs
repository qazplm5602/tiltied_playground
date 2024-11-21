using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShootGauge : MonoBehaviour
{
    [SerializeField] private RectTransform _fill;
    private Image _fillImage;
    private CanvasGroup _canvasGroup;
    private Player _player;

    private float _redValue = 1.0f;
    private float _greenValue = 0.0f;
    private bool _isShooting = false;
    private bool _isBlind = false;

    private Sequence _colorSequence;
    private Tweener _scaleTweener;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _player = GetComponentInParent<Player>();
        _fillImage = _fill.GetComponent<Image>();

        _player.ShootingStartEvent += HandleShootingStart;
        _player.ShootingEndEvent += HandleShootingEnd;
        _player.BlindEvent += HandleBlindSkill;

        _canvasGroup.alpha = 0;
        _fillImage.color = new Color(1, 0, 0);
    }

    private void Update()
    {
        if (_isShooting && !_isBlind)
        {
            _fillImage.color = new Color(_redValue, _greenValue, 0);
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                         Camera.main.transform.rotation * Vector3.up);
    }

    private void HandleShootingStart()
    {
        _isShooting = true;

        _fill.localScale = new Vector3(0, 1, 1);
        _canvasGroup.alpha = 1;

        _colorSequence?.Kill();
        _scaleTweener?.Kill();

        if (_isBlind)
        {
            _fillImage.color = Color.black;
            _fill.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            _colorSequence = DOTween.Sequence()
                .Append(DOTween.To(() => _redValue, r => _redValue = r, 0, 5))
                .Join(DOTween.To(() => _greenValue, g => _greenValue = g, 1, 5))
                .SetEase(Ease.Linear);

            _scaleTweener = _fill.DOScaleX(1, 5).SetEase(Ease.Linear);
        }
    }

    private void HandleShootingEnd()
    {
        _isShooting = false;
        StartCoroutine(ShootingEnd());
    }

    private IEnumerator ShootingEnd()
    {
        _colorSequence?.Kill();
        _scaleTweener?.Kill();

        yield return new WaitForSeconds(1);

        _redValue = 1.0f;
        _greenValue = 0.0f;
        _canvasGroup.alpha = 0;
        _fill.localScale = new Vector3(0, 1, 1);
    }

    private void HandleBlindSkill(float skillTime)
    {
        StartCoroutine(BlindSkill(skillTime));
    }

    private IEnumerator BlindSkill(float skillTime)
    {
        SetSpecialMode(true);
        yield return new WaitForSeconds(skillTime);
        SetSpecialMode(false);
    }

    public void SetSpecialMode(bool isSpecial)
    {
        _isBlind = isSpecial;
    }

    private void OnDestroy()
    {
        _player.ShootingStartEvent -= HandleShootingStart;
        _player.ShootingEndEvent -= HandleShootingEnd;

        _colorSequence?.Kill();
        _scaleTweener?.Kill();
    }
}
