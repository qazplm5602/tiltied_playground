using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Char_Selector : MonoBehaviour
{
    [SerializeField] private List<Image> _characters = new List<Image>();
    [SerializeField] private PlayerControlSO _inputSO;



    public void ResetSelect()
    {
        for (int i = 0; i < _characters.Count; i++)
        {
            _characters[i].transform.GetChild(0).gameObject.SetActive(false);
        }
    }


    public void SelectCharacter()
    {

    }

}
