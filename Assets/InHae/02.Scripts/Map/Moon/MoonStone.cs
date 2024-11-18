using UnityEngine;
using DG.Tweening;

public class MoonStone : MonoBehaviour
{
    [SerializeField] private float _firstUpValue;
    
    private Sequence _sequence;
    private float _defaultY;
    private float _loopTime;
    
    public void GravityOff()
    {
        _sequence.Kill();
        transform.DOMoveY(_defaultY, _loopTime).OnComplete(() => gameObject.SetActive(false));
    }

    public void GravityOn(float loopTime)
    {
        _loopTime = loopTime;
        _defaultY = transform.position.y;
        gameObject.SetActive(true);

        transform.DOMoveY(_defaultY + _firstUpValue, _loopTime).OnComplete(() =>
        {
            SequenceCreate(transform.position.y);
            _sequence.Play();
        });
    }

    private void SequenceCreate(float defaultY)
    {
        _sequence = DOTween.Sequence();
        
        _sequence.Append(transform.DOMoveY(defaultY + 15f, _loopTime).SetEase(Ease.InOutQuad));
        _sequence.AppendInterval(0.05f);
        _sequence.Append(transform.DOMoveY(defaultY, _loopTime).SetEase(Ease.InOutQuad));

        _sequence.SetLoops(-1, LoopType.Restart);
    }
}
