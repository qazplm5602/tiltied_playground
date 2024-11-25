using UnityEngine;
using UnityEngine.UI;

public class KeyBoxUI : DeviceChangeListener
{
    [SerializeField] private Sprite gamePadKey;
    
    private Image boxImage;
    private GameObject childEntity;

    private Sprite defaultSprite;

    private void Awake() {
        boxImage = GetComponent<Image>();
        defaultSprite = boxImage.sprite;

        childEntity = transform.GetChild(0).gameObject;
    }

    protected override void OnChangeGamepad(bool active)
    {
        boxImage.sprite = active ? gamePadKey : defaultSprite;
        childEntity.SetActive(!active);
    }
}