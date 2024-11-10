using System;
using UnityEngine;
using DG.Tweening;

public class MoonStone : MonoBehaviour
{
    [SerializeField] private float _loopTime;

    private Tween _currentTween;
    
    private void Awake()
    {
        _currentTween = transform.DOMoveY(transform.position.y + 15f, _loopTime).SetLoops(-1, LoopType.Yoyo);
    }
}
