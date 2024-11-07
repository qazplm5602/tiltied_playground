using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Char_Selector : MonoBehaviour
{

    [SerializeField] private GameObject mapSelectUI;

    [SerializeField] private UI_Characters[] _characters;
    [SerializeField] private PlayerControlSO _inputSO1;
    [SerializeField] private PlayerControlSO _inputSO2;
    [SerializeField] private int rightMaxIdx = 0;  // 오른쪽으로 몇개까지 캐릭터가 있는가. 아래있는 캐릭터를 charIndex % 이값으로 나타날 예정.


    [SerializeField] private PlayerStatsSO selectSO1 = null;
    [SerializeField] private PlayerStatsSO selectSO2 = null;

    private int charIndex1 = 0;
    private int charIndex2 = 0;

    private event Action isReady;
    private void OnEnable()
    {
        _characters = GetComponentsInChildren<UI_Characters>();

        _inputSO1.ItemUseEvent += HandleSelectCharacter1;
        _inputSO1.MoveEvent += HandleMoveEvent1;

        _inputSO2.ItemUseEvent += HandleSelectCharacter2;
        _inputSO2.MoveEvent += HandleMoveEvent2;
        isReady += HandleGoToMapSelect;
    }
    private void HandleGoToMapSelect()
    {
        UI_Manager.Instance.UIOpenOrClose(mapSelectUI, true);
        gameObject.SetActive(false);
    }

    private void HandleMoveEvent1()
    {
        if (_characters[charIndex1].IsSelected1)
        {
            return;
        }
        Vector2 dir = _inputSO1.GetMoveDirection().normalized;
        int tmpindex = charIndex1;
        charIndex1 += (int)dir.x;
        charIndex1 += rightMaxIdx * -(int)dir.y;  //-1 이 들어오면..
        if (charIndex1 >= _characters.Length || charIndex1 < 0)
        {
            charIndex1 = tmpindex;

        }
        IsOnUp(1);
    }

    private void HandleSelectCharacter1()
    {
        selectSO1 = _characters[charIndex1].SelectCharacter1();
        if (_characters[charIndex1].IsSelected1 == true && _characters[charIndex2].IsSelected2 == true)
        {
            isReady?.Invoke();
        }
    }

    private void HandleMoveEvent2()
    {
        if (_characters[charIndex2].IsSelected2)
        {
            return;
        }
        Vector2 dir = _inputSO2.GetMoveDirection();
        int tmpindex = charIndex2;
        charIndex2 += (int)dir.x;
        charIndex2 += rightMaxIdx * -(int)dir.y;  //-1 이 들어오면..
        if (charIndex2 >= _characters.Length || charIndex2 < 0)
        {
            charIndex2 = tmpindex;
        }
        IsOnUp(2);
    }

    private void HandleSelectCharacter2()
    {
        selectSO2 = _characters[charIndex2].SelectCharacter2();

        if (_characters[charIndex1].IsSelected1 == true && _characters[charIndex2].IsSelected2 == true)
        {
            isReady?.Invoke();
        }
    }
    public void IsOnUp(int objIdx)  // 여기서 능력치를 보여줘야해..
                                    // 움직일때마다 캐릭터를 바꿔주니까
                                    // _character[charIndex1]에서 statSO값 가져와서 이미지에 넣어주자..
    {
        if (objIdx == 1)
        {
            for (int i = 0; i < _characters.Length; i++)
            {
                _characters[i]._isOnTopImage1.enabled = false;
            }
            _characters[charIndex1]._isOnTopImage1.enabled = true;
        }
        else
        {
            for (int i = 0; i < _characters.Length; i++)
            {
                _characters[i]._isOnTopImage2.enabled = false;
            }
            _characters[charIndex2]._isOnTopImage2.enabled = true;
        }
    }


    private void OnDisable()
    {
        _inputSO1.ItemUseEvent -= HandleSelectCharacter1;
        _inputSO2.ItemUseEvent -= HandleSelectCharacter2;
        isReady -= HandleGoToMapSelect;
    }

    public void ResetSelect(int idx)
    {
        if (idx == 1)
        {
            for (int i = 0; i < _characters.Length; i++)
            {
                _characters[i].IsSelected1 = false;
                _characters[i]._selectImage1.enabled = false; // 선택 된거 표현 해줄 그림 꺼주기.
            }
        }
        else
        {
            for (int i = 0; i < _characters.Length; i++)
            {
                _characters[i].IsSelected2 = false;
                _characters[i]._selectImage2.enabled = false; // 선택 된거 표현 해줄 그림 꺼주기.
            }
        }
    }
}
