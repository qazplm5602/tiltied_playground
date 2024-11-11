using System;
using UnityEngine;
using DG.Tweening;

public class MoonStone : MonoBehaviour
{
    private Sequence _sequence;

    public void GravityOff()
    {
        
    }

    public void GravityOn(float loopTime)
    {
        
        
        float defaultY = transform.position.y;

        _sequence = DOTween.Sequence();
        
        _sequence.Append(transform.DOMoveY(defaultY + 15f, loopTime).SetEase(Ease.InOutQuad));
        _sequence.AppendInterval(0.05f);
        _sequence.Append(transform.DOMoveY(defaultY, loopTime).SetEase(Ease.InOutQuad));

        _sequence.SetLoops(-1, LoopType.Restart);
        
        
    }
    
    
}
