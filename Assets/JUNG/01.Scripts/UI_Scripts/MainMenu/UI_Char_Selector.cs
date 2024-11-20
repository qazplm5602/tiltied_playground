using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Char_Selector : MonoBehaviour
{
    [SerializeField] private Image _player1PosImg;
    [SerializeField] private Image _player2PosImg;


    [SerializeField] private GameObject mapSelectUI;

    [SerializeField] private UI_Characters[] _characters;
    [SerializeField] private PlayerControlSO _inputSO1;
    [SerializeField] private PlayerControlSO _inputSO2;
    [SerializeField] private int rightMaxIdx = 0;  // ���������� ����� ĳ���Ͱ� �ִ°�. �Ʒ��ִ� ĳ���͸� charIndex % �̰����� ��Ÿ�� ����.

    [SerializeField] private UI_PlayerStatShow showingStat1;
    [SerializeField] private UI_PlayerStatShow showingStat2;


    private int charIndex1 = 0;
    private int charIndex2 = 0;

    private event Action IsReady;
    private void Start()
    {
        _characters = GetComponentsInChildren<UI_Characters>();

        showingStat1.OnStatChange(_characters[charIndex1].playerStat);
        showingStat2.OnStatChange(_characters[charIndex2].playerStat);
    }
    private void OnEnable()
    {

        _inputSO1.ItemUseEvent += HandleSelectCharacter1;
        _inputSO1.MoveEvent += HandleMoveEvent1;

        _inputSO2.ItemUseEvent += HandleSelectCharacter2;
        _inputSO2.MoveEvent += HandleMoveEvent2;

        _inputSO1.CloseUIEvent += HandleCloseUIEvent;
        IsReady += HandleGoToMapSelect;
    }

    private void HandleCloseUIEvent()
    {
        UI_Manager.Instance.UIOpenOrClose(mapSelectUI, false, transform.parent.gameObject);
    }

    private void HandleGoToMapSelect()
    {
        UI_Manager.Instance.UIOpenOrClose(mapSelectUI, true, transform.parent.gameObject);
    }

    private void HandleMoveEvent1()
    {
        if (_characters[charIndex1].IsSelected1)
        {
            return;
        }
        Vector2 dir = _inputSO1.GetMoveDirection().normalized;
        int tmpindex = charIndex1;
        Debug.Log(charIndex1);
        charIndex1 += (int)dir.x;
        charIndex1 += rightMaxIdx * -(int)dir.y;  //-1 �� ������..
        if (charIndex1 >= _characters.Length || charIndex1 < 0)
        {
            charIndex1 = tmpindex;

        }
        IsOnUp(1);
    }

    private void HandleSelectCharacter1()
    {
        GameDataManager.Instance.player1_ObjData = _characters[charIndex1].SelectCharacter1();
        GameDataManager.Instance.player1_StatData = _characters[charIndex1].SelectStat1();


        if (_characters[charIndex1].IsSelected1 == true && _characters[charIndex2].IsSelected2 == true)
        {
            IsReady?.Invoke();
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
        charIndex2 += rightMaxIdx * -(int)dir.y;  //-1 �� ������..
        if (charIndex2 >= _characters.Length || charIndex2 < 0)
        {
            charIndex2 = tmpindex;
        }
        IsOnUp(2);
    }

    private void HandleSelectCharacter2()
    {
        GameDataManager.Instance.player2_ObjData = _characters[charIndex2].SelectCharacter2();
        GameDataManager.Instance.player2_StatData = _characters[charIndex2].SelectStat2();

        if (_characters[charIndex1].IsSelected1 == true && _characters[charIndex2].IsSelected2 == true)
        {
            IsReady?.Invoke();
        }
    }
    public void IsOnUp(int objIdx)  // ���⼭ �ɷ�ġ�� ���������..
                                    // �����϶����� ĳ���͸� �ٲ��ִϱ�
                                    // _character[charIndex1]���� statSO�� �����ͼ� �̹����� �־�����..
    {
        if (objIdx == 1)
        {
            //_player1PosCam = _characters[charIndex1].   ���� �ϱ�   /   stat SO �ȿ� Pos������ �־������ �Ѻ�
            _player1PosImg.sprite = _characters[charIndex1].playerStat.playerIcon;
            for (int i = 0; i < _characters.Length; i++)
            {
                _characters[i]._isOnTopImage1.gameObject.SetActive(false);
            }
            showingStat1.OnStatChange(_characters[charIndex1].playerStat);
            _characters[charIndex1]._isOnTopImage1.gameObject.SetActive(true);

        }
        else
        {
            _player2PosImg.sprite = _characters[charIndex2].playerStat.playerIcon;
            for (int i = 0; i < _characters.Length; i++)
            {
                _characters[i]._isOnTopImage2.gameObject.SetActive(false);
            }
            showingStat2.OnStatChange(_characters[charIndex2].playerStat);
            _characters[charIndex2]._isOnTopImage2.gameObject.SetActive(true);
        }
    }


    private void OnDisable()
    {
        _inputSO1.ItemUseEvent -= HandleSelectCharacter1;
        _inputSO1.MoveEvent -= HandleMoveEvent1;

        _inputSO2.ItemUseEvent -= HandleSelectCharacter2;
        _inputSO2.MoveEvent -= HandleMoveEvent2;

        _inputSO1.CloseUIEvent -= HandleCloseUIEvent;
        IsReady -= HandleGoToMapSelect;
    }

    public void ResetSelectCharacter(int idx)
    {
        if (idx == 1)
        {
            for (int i = 0; i < _characters.Length; i++)
            {
                _characters[i].IsSelected1 = false;
                _characters[i]._selectImage1.enabled = false; // ���� �Ȱ� ǥ�� ���� �׸� ���ֱ�.
            }
        }
        else
        {
            for (int i = 0; i < _characters.Length; i++)
            {
                _characters[i].IsSelected2 = false;
                _characters[i]._selectImage2.enabled = false; // ���� �Ȱ� ǥ�� ���� �׸� ���ֱ�.
            }
        }
    }
}
