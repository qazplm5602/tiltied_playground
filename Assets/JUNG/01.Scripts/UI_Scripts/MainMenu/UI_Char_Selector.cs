using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Char_Selector : MonoBehaviour
{
    //adding input select character;

    [SerializeField] private UI_Characters[] _characters;
    [SerializeField] private PlayerControlSO _inputSO1;
    [SerializeField] private PlayerControlSO _inputSO2;

    private int charIndex1 = 0;
    private int charIndex2 = 0;

    private void Start()
    {
        _characters = GetComponentsInChildren<UI_Characters>();
        _inputSO1.ItemUseEvent += HandleSelectCharacter1;
        _inputSO1.MoveEvent += HandleMoveEvent1;

        _inputSO2.ItemUseEvent += HandleSelectCharacter2;
        _inputSO2.MoveEvent += HandleMoveEvent2;
    }

    
    private void HandleMoveEvent1()
    {
        Vector2 dir = _inputSO1.GetMoveDirection();
        charIndex1 += (int)dir.x;
        charIndex1 += (int)dir.y;
    }

    private void HandleSelectCharacter1()
    {

    }


    private void HandleMoveEvent2()
    {

        Vector2 dir = _inputSO2.GetMoveDirection();
        charIndex2 += (int)dir.x;
        charIndex2 += (int)dir.y;
    }

    private void HandleSelectCharacter2()
    {

    }


    private void OnDestroy()
    {
        _inputSO1.ItemUseEvent -= HandleSelectCharacter1;
        _inputSO2.ItemUseEvent -= HandleSelectCharacter2;
    }

    public void ResetSelect()
    {
        for (int i = 0; i < _characters.Length; i++)
        {
            _characters[i].transform.GetChild(0).gameObject.SetActive(false); // 선택 된거 표현 해줄 그림 꺼주기.
        }
    }


    public void SelectCharacter()
    {

    }

}
