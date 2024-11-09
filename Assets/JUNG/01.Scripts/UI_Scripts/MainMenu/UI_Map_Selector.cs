using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class UI_Map_Selector : MonoBehaviour
{

    [SerializeField] private PlayerControlSO _inputSO1;
    [SerializeField] private PlayerControlSO _inputSO2;
    [SerializeField] private UI_Map[] _maps;

    [SerializeField] private int rightMaxIdx = 0;  // 오른쪽으로 몇개까지 캐릭터가 있는가. 아래있는 캐릭터를 charIndex % 이값으로 나타날 예정.


    [SerializeField] private EventMapSO selectSO1 = null;
    [SerializeField] private EventMapSO selectSO2 = null;

    private int charIndex1 = 0;
    private int charIndex2 = 0;

    private event Action IsReady;

    private void OnEnable()
    {
        _inputSO1.ItemUseEvent += HandleMapSelectEvent1;
        _inputSO1.MoveEvent += HandleMoveEvent1;

        _inputSO2.ItemUseEvent += HandleSelectCharacter2;
        _inputSO2.MoveEvent += HandleMoveEvent2;
    }
    private void OnDisable()
    {
        _inputSO1.ItemUseEvent -= HandleMapSelectEvent1;
        _inputSO1.MoveEvent -= HandleMoveEvent1;

        _inputSO2.ItemUseEvent -= HandleSelectCharacter2;
        _inputSO2.MoveEvent -= HandleMoveEvent2;
    }

    private void HandleMoveEvent2()
    {
        if (_maps[charIndex2].IsSelected2)
        {
            return;
        }
        Vector2 dir = _inputSO2.GetMoveDirection().normalized;
        int tmpindex = charIndex2;
        charIndex2 += (int)dir.x;
        charIndex2 += rightMaxIdx * -(int)dir.y;  //-1 이 들어오면..
        if (charIndex2 >= _maps.Length || charIndex2 < 0)
        {
            charIndex2 = tmpindex;
        }
        IsOnUp(2);
    }

    private void HandleSelectCharacter2()
    {
        selectSO2 = _maps[charIndex2].SelectMap2();
        if (_maps[charIndex1].IsSelected1 == true && _maps[charIndex2].IsSelected2 == true)
        {
            IsReady?.Invoke();
        }
    }

    private void HandleMoveEvent1()
    {
        if (_maps[charIndex1].IsSelected1)
        {
            return;
        }
        Vector2 dir = _inputSO1.GetMoveDirection().normalized;
        int tmpindex = charIndex1;
        charIndex1 += (int)dir.x;
        charIndex1 += rightMaxIdx * -(int)dir.y;  //-1 이 들어오면..
        if (charIndex1 >= _maps.Length || charIndex1 < 0)
        {
            charIndex1 = tmpindex;

        }
        IsOnUp(1);
    }
    private void HandleMapSelectEvent1()
    {
        selectSO1 = _maps[charIndex1].SelectMap1();
        if (_maps[charIndex1].IsSelected1 == true && _maps[charIndex2].IsSelected2 == true)
        {
            IsReady?.Invoke();
        }
    }

    private void IsOnUp(int objIdx)
    {
        if (objIdx == 1)
        {
            for (int i = 0; i < _maps.Length; i++)
            {
                _maps[i]._isOnTopImage1.enabled = false;
            }
            _maps[charIndex1]._isOnTopImage1.enabled = true;
        }
        else
        {
            for (int i = 0; i < _maps.Length; i++)
            {
                _maps[i]._isOnTopImage2.enabled = false;
            }
            _maps[charIndex2]._isOnTopImage2.enabled = true;
        }
    }


    private void PercentSelect()
    {

    }
}
