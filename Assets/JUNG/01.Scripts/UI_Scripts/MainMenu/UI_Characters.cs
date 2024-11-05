using UnityEngine;
using UnityEngine.UI;

public class UI_Characters : MonoBehaviour
{
    private Image _selectImage;

    private void Start()
    {
        _selectImage = GetComponentInChildren<Image>();
    }


}
