using UnityEngine;
using UnityEngine.UI;

public class UI_Map : MonoBehaviour
{
    private UI_Map_Selector _selector;
    public Image _selectImage1;
    public Image _selectImage2;
    public Image _isOnTopImage1;
    public Image _isOnTopImage2;
    public bool IsSelected1;
    public bool IsSelected2;
    public EventMapSO mapData;

    private void Start()
    {
        _selector = GetComponentInParent<UI_Map_Selector>();
        _selectImage1 = transform.GetChild(0).gameObject.GetComponent<Image>();
        _selectImage2 = transform.GetChild(1).gameObject.GetComponent<Image>();
        _isOnTopImage1 = transform.GetChild(2).gameObject.GetComponent<Image>();
        _isOnTopImage2 = transform.GetChild(3).gameObject.GetComponent<Image>();
    }

    public EventMapSO SelectMap1()
    {
        if (IsSelected1)
        {
            IsSelected1 = false;
            _selectImage1.enabled = false;
            return null;
        }
        _selector.ResetSelectMap(1);

        _selectImage1.enabled = true;
        IsSelected1 = true;


        return mapData;
    }

    public EventMapSO SelectMap2()
    {
        if (IsSelected2)
        {
            IsSelected2 = false;
            _selectImage2.enabled = false;
            return null;
        }
        _selector.ResetSelectMap(2);

        _selectImage2.enabled = true;
        IsSelected2 = true;


        return mapData;
    }
}
