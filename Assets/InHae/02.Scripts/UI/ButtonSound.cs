using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    [SerializeField] private SoundSO _clickSound;
    [SerializeField] private SoundSO _onSound;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySFX(transform.position, _onSound);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySFX(transform.position, _clickSound);
    }
}
