using UnityEngine;
using UnityEngine.UI;

public class BG_Mover : MonoBehaviour
{
    [SerializeField] private float _x, _y;
    private RawImage _rawImage;

    private void Start()
    {
        _rawImage = GetComponent<RawImage>();
    }
    void Update()
    {
        _rawImage.uvRect = new Rect(_rawImage.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, _rawImage.uvRect.size);
    }
}
